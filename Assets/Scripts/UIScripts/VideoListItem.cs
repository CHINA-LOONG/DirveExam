using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoListItem : MonoBehaviour
{
    public VideoData videoData;
    public Callback<VideoData> onClick;

    private void Start()
    {
        //UIEventListener.Get(gameObject).onClick += OnClickItem;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickItem);
    }

    public void InitWith(VideoData videoData, Callback<VideoData> onClick)
    {
        this.videoData = videoData;
        this.onClick = onClick;
        gameObject.GetComponent<RawImage>().texture = ResourcesMgr.Instance.GetTextureWithName(videoData.imgurl);
    }

    void OnClickItem()
    {
        if (onClick != null)
        {
            onClick(videoData);
        }
    }
}
