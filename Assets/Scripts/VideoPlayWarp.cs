using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayWarp : MonoBehaviour
{
    public Button btnStart;

    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    private void Start()
    {
        RenderTexture renderTexture = new RenderTexture(512, 512, 24);

        rawImage = gameObject.GetComponent<RawImage>();
        rawImage.texture = renderTexture;
        rawImage.SetNativeSize();

        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;

        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += frameReady;
        videoPlayer.started += started;
        videoPlayer.prepareCompleted += prepareCompleted;
        videoPlayer.Prepare();

        btnStart.onClick.AddListener(() => { videoPlayer.Play(); });
    }
    

    void started(VideoPlayer source)
    {
        Debug.Log("started");
    }
    void prepareCompleted(VideoPlayer source)
    {
        Debug.Log("prepareCompleted");
    }
    void frameReady(VideoPlayer source, long frameIdx)
    {
        Debug.Log(frameIdx);
    }
}
