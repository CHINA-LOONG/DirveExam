using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;      //播放控件

    public Button btnStart;     //开始播放
    public Button btnVideoList; //播放列表控制
    public Slider sliderProg;   //播放进度控制
    public Button btnPlay;      //播放状态控制
    public Button btnScreen;    //是否全屏

    public Sprite sprPlay;      //播放按钮图片
    public Sprite sprPause;     //暂停按钮图片

    public Text textPlayTime;   //播放时间
    public Text textTotalTime;  //总长时间


    public GameObject objVideoList;         //列表父节点显隐
    public VideoListItem videoListItem;     //预制，用于创建 

    public RenderTexture videoRender;
    private bool showList = true;


    private void Start()
    {
        btnStart.onClick.AddListener(OnClickStart);
        btnVideoList.onClick.AddListener(OnClickVideoList);
        btnPlay.onClick.AddListener(OnClickPlay);
        btnScreen.onClick.AddListener(OnClickScreen);

        sliderProg.onValueChanged.AddListener(OnSliderChange);
        UIEventListener.Get(sliderProg.gameObject).onDown += (go) => { Debug.Log("down"); };
        UIEventListener.Get(sliderProg.gameObject).onUp += (go) => {Debug.Log("up"); };

        videoPlayer.prepareCompleted += prepareCompleted;
        videoPlayer.frameDropped += frameDropped;

        sliderProg.maxValue = 100f;
        sliderProg.minValue = 0.0f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            sliderProg.value = Random.Range(0.0f, 100f);
        }
    }

    public void InitWith(List<VideoData> videoList)
    {
        videoRender = new RenderTexture(1280, 720, 24);
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoRender;

        for (int i = 0; i < videoList.Count; i++)
        {
            GameObject go = Instantiate(videoListItem.gameObject, videoListItem.transform.parent);
            VideoListItem videoItem = go.GetComponent<VideoListItem>();
            videoItem.InitWith(videoList[i], OnClickVideoItem);
        }
        videoListItem.gameObject.SetActive(false);
        SetVideoPlayData(videoList[0]);
    }

    void SetVideoPlayData(VideoData videoData)
    {

        videoPlayer.GetComponent<RawImage>().texture = ResourcesMgr.Instance.GetTextureWithName(videoData.imgurl);
        videoPlayer.url = videoData.videourl;
        videoPlayer.Prepare();
    }

    void OnClickVideoItem(VideoData videoData)
    {
        btnStart.gameObject.SetActive(true);

        sliderProg.interactable = false;
        SetVideoPlayData(videoData);
    }

    void prepareCompleted(VideoPlayer source)
    {
        sliderProg.interactable = true;

        sliderProg.minValue = 0;
        sliderProg.maxValue = source.frameCount / source.frameRate;

        int time = Mathf.CeilToInt(sliderProg.maxValue);
        int minute = time / 60;
        int second = time % 60;
        textTotalTime.text = string.Format("{0:D2}:{1:D2}", minute.ToString(), second.ToString());
    }
    void frameDropped(VideoPlayer source){
        
    }

    void OnClickStart()
    {
        btnStart.gameObject.SetActive(false);
        videoPlayer.Play();
        videoPlayer.GetComponent<RawImage>().texture = videoRender;
    }

    /// <summary>
    /// Show or Hide VideoList
    /// </summary>
    void OnClickVideoList()
    {
        showList = !showList;
        objVideoList.SetActive(showList);
    }

    void OnSliderChange(float value)
    {
        Debug.Log(value);
        if (videoPlayer.isPrepared)
        {
            videoPlayer.time = (long)value;
            int time = (int)value;
            int minute = time / 60;
            int second = time % 60;
            textPlayTime.text = string.Format("{0:D2}:{1:D2}", minute.ToString(), second.ToString());
        }
    }

    void OnClickPlay()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            (btnPlay.targetGraphic as Image).sprite = sprPlay;
        }
        else
        {
            videoPlayer.Play();
            (btnPlay.targetGraphic as Image).sprite = sprPause;
        }
    }








    /// <summary>
    /// 全屏显示
    /// </summary>
    void OnClickScreen()
    {

    }
}
