using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class NewBehaviourScript : MonoBehaviour
{

    VideoPlayer vp;
    Texture2D videoFrameTexture;
    RenderTexture renderTexture;
    void Start()
    {
        videoFrameTexture = new Texture2D(2, 2);
        vp = GetComponent<VideoPlayer>();
        vp.playOnAwake = false;
        vp.waitForFirstFrame = true;

        vp.sendFrameReadyEvents = true;
        vp.frameReady += OnNewFrame;
        vp.Play();


    }
    int framesValue = 0;//获得视频第几帧的图片
    void OnNewFrame(VideoPlayer source, long frameIdx)
    {
        Debug.Log(frameIdx);
        framesValue++;
        if (framesValue == 100)
        {
            renderTexture = source.texture as RenderTexture;
            if (videoFrameTexture.width != renderTexture.width || videoFrameTexture.height != renderTexture.height)
            {
                videoFrameTexture.Resize(renderTexture.width, renderTexture.height);
            }
            RenderTexture.active = renderTexture;
            videoFrameTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            videoFrameTexture.Apply();
            RenderTexture.active = null;
            vp.frameReady -= OnNewFrame;
            vp.sendFrameReadyEvents = false;
        }
    }

    void OnDisable()
    {
        if (!File.Exists(Application.dataPath + "/temp.jpg"))
        {
            ScaleTexture(videoFrameTexture, 800, 400, (Application.dataPath + "/temp.jpg"));
        }
    }
    //生成缩略图
    void ScaleTexture(Texture2D source, int targetWidth, int targetHeight, string savePath)
    {

        Texture2D result = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        File.WriteAllBytes(savePath, result.EncodeToJPG());
    }

}