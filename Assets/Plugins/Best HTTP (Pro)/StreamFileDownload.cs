using UnityEngine;
using BestHTTP;
using System.IO;
using System.Collections.Generic;

public class StreamFileDownload : MonoBehaviour
{
    int downloaded;
    int downloadLength;
    FileStream fileWriteStream;
    string status = string.Empty;

    /// <summary>
    /// 下载完成回调
    /// </summary>
    /// <param name="state">state == 0 时 succ</param>  
    /// <param name="errMsg"></param>
    public delegate void OnFinishDownloadDelegate(int state,string errMsg,System.Exception ex);

    OnDownloadProgressDelegate progressCallback;
    OnFinishDownloadDelegate finishCallBack;
    public void RequestDownload(string saveFile, string url, OnDownloadProgressDelegate progressCallback,OnFinishDownloadDelegate finishCallBack)
    {
        downloaded = 0;
        downloadLength = 0;
        this.progressCallback = progressCallback;
        this.finishCallBack = finishCallBack;

        string directoryName = Path.GetDirectoryName(saveFile);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        fileWriteStream = File.Create(saveFile);

        HTTPRequest request = new HTTPRequest(new System.Uri(url), OnDownloadFile);
        request.DisableCache = true;
        request.UseStreaming = true;
        request.StreamFragmentSize = HTTPResponse.MinBufferSize;//4 metabyte

        request.Send();
    }

    void OnDownloadFile(HTTPRequest request,HTTPResponse response)
    {
        switch (request.State)
        {
            // The request is currently processed. With UseStreaming == true, we can get the streamed fragments here
            case HTTPRequestStates.Processing:

                // Set the DownloadLength, so we can display the progress
                if(downloadLength == 0)
                {
                    string value = response.GetFirstHeaderValue("content-length");
                    if (!string.IsNullOrEmpty(value))
                    {
                        downloadLength = int.Parse(value);
                    }
                }
                // Get the fragments, and save them
                ProcessFragments(request, response.GetStreamedFragments());
                break;

            // The request finished without any problem.
            case HTTPRequestStates.Finished:
                if (response.IsSuccess)
                {
                    if (downloadLength == 0)
                    {
                        string value = response.GetFirstHeaderValue("content-length");
                        if (!string.IsNullOrEmpty(value))
                        {
                            downloadLength = int.Parse(value);
                        }
                    }
                    // Save any remaining fragments
                    ProcessFragments(request, response.GetStreamedFragments());

                    // Completly finished
                    if (response.IsStreamingFinished)
                    {
                        request = null;
                        finishDownload(0, null);
                    }
                    else
                    {
                        Debug.Log("Processing - continue");
                    }
                }
                else
                {
                    status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                    response.StatusCode,
                                                    response.Message,
                                                    response.DataAsText);
                    Debug.LogError( status);
                    request = null;
                    finishDownload(-1, status);
                }
                break;

            // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
            case HTTPRequestStates.Error:
                status = "Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception");
                Debug.LogError(status);

                request = null;
                finishDownload(-1, status);
                break;

            // The request aborted, initiated by the user.
            case HTTPRequestStates.Aborted:
                status = "Request Aborted!";
                request = null;
                finishDownload(-1, status);
                break;

            // Ceonnecting to the server is timed out.
            case HTTPRequestStates.ConnectionTimedOut:
                status = "Connection Timed Out!";
                Debug.LogError(status);
                request = null;
                finishDownload(-1, status);
                break;

            // The request didn't finished in the given time.
            case HTTPRequestStates.TimedOut:
                status = "Processing the request Timed Out!";
                Debug.LogError(status);
                request = null;
                finishDownload(-1, status);
                break;
        }
    }

    void ProcessFragments(HTTPRequest originalRequest,List<byte[]> fragments)
    {
        if (fragments != null && fragments.Count > 0)
        {
            for (int i = 0; i < fragments.Count; ++i)
            {
                // Save how many bytes we wrote successfully
                downloaded += fragments[i].Length;
                fileWriteStream.Write(fragments[i], 0, fragments[i].Length);
                if(null!= progressCallback)
                {
                    progressCallback(originalRequest, downloaded, downloadLength);
                }
            }
        }
    }

    void finishDownload(int state,string errmsg)
    {
        System.Exception ex = null;
        try
        {
            if (state == 0)
            {
                fileWriteStream.Flush();
            }
            fileWriteStream.Close();
            fileWriteStream = null;
        }
        catch(System.Exception e)
        {
            ex = e;
            Debug.LogError("file error");
            state = -1;
        }

        finishCallBack(state, errmsg,ex);
    }
}
