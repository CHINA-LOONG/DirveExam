using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using System;

public class BaiduAuth
{
    public string access_token;
    public int expires_in;
    public string refresh_token;
    public string scope;
    public string session_key;
    public string session_secret;
}



public class testMove : MonoBehaviour {

    public BaiduAuth baiduAuth;
    AipKit aipKit;
	// Use this for initialization
	void Start () {
        //aipKit = new AipKit();
        //return;
        BaiduAuth();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ClickButton()
    {
        BaiduTts();
        return;
        aipKit.Tts();
    }

    private void BaiduAuth()
    {
        string url = @"https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=XmrFwCyGGeknfZcMslbXm3dS&client_secret=hNAarHUfVaeF1D2vfKn5r7xG9p7ExPFN";
        HTTPRequest authBaidu = new HTTPRequest(new Uri(url),HTTPMethods.Get, BaiduAuthReturn);
        authBaidu.Send();
    }


    void BaiduAuthReturn(HTTPRequest originalRequest, HTTPResponse response)
    {
        if (response == null || response.StatusCode >= 400)
        {
            string errorMsg;
            if (response == null)
            {
                errorMsg = "服务器链接错误状态码：" + "response==null";
            }
            else
            {
                errorMsg = "服务器链接错误状态码：" + response.StatusCode.ToString();
            }
            Debug.LogError(errorMsg);//账号服出现问题，直接获取默认远程配置显示停服公告
            return;
        }
        string errStr = string.Empty;
        switch (originalRequest.State)
        {
            case HTTPRequestStates.Processing:
                break;
            case HTTPRequestStates.Finished:
                if (response.IsSuccess)
                {
                    try
                    {
                        baiduAuth = LitJson.JsonMapper.ToObject<BaiduAuth>(response.DataAsText);
                    }
                    catch (Exception e)
                    {
                        errStr = "ERROR：与账号服json不匹配--" + e.Message;
                    }
                }
                break;
            case HTTPRequestStates.Error:
                errStr = "Request Error!";
                break;
            case HTTPRequestStates.Aborted:
                errStr = "Request Aborted!";
                break;
            case HTTPRequestStates.ConnectionTimedOut:
                errStr = "Connection Timed Out!";
                break;
            case HTTPRequestStates.TimedOut:
                errStr = "Processing the request Timed Out!";
                break;
        }
        if (!string.IsNullOrEmpty(errStr))
        {
            Debug.Log(errStr);
        }
    }


    private void BaiduTts()
    {
        string url = @"http://tsn.baidu.com/text2audio?tex={0}&lan=zh&cuid=123543&ctp=1&tok={1}";
        url = string.Format(url, "重力", baiduAuth.access_token);
        HTTPRequest authBaidu = new HTTPRequest(new Uri(url),HTTPMethods.Get, BaiduTtsRetrun);
        authBaidu.Send();
    }

    void BaiduTtsRetrun(HTTPRequest originalRequest, HTTPResponse response)
    {
        if (response == null || response.StatusCode >= 400)
        {
            string errorMsg;
            if (response == null)
            {
                errorMsg = "服务器链接错误状态码：" + "response==null";
            }
            else
            {
                errorMsg = "服务器链接错误状态码：" + response.StatusCode.ToString();
            }
            Debug.LogError(errorMsg);//账号服出现问题，直接获取默认远程配置显示停服公告
            return;
        }
        AudioClip audioClip = new AudioClip();
        
    }

}
