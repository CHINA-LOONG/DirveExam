using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Wit.BaiduAip.Speech;

public class MainSceneMgr : MonoBehaviour
{
    public string APIKey = "WondCiSwY0RzHYc7hGS2bFoc";
    public string SecretKey = "NjaCdrsUKPi9xiB6X6F6T46B2TdT8ZcT";

    public UILoginWindow uiLoginWindow;
    public Tts ttsString2Audio;
    private void Awake()
    {
        ttsString2Audio = new Tts(APIKey, SecretKey);


        if (/* 是否有网络 */ true)
        {
            uiLoginWindow = UIManager.Instance.OpenUI<UILoginWindow>();
            uiLoginWindow.SetState("正在检查题库更新...");
            CheckConfigUpdate();
        }
        else
        {
            CheckLoginState();
        }
    }

    void CheckLoginState()
    {
        if (/* 是否已经登录过 */false)
        {
            //看是否更新数据进行网络交互
            UIManager.Instance.OpenUI<UIMainWindow>();
        }
        else
        {
            uiLoginWindow.SetLoginList();
        }
    }

    /// <summary>
    /// Checks the gameConfig update.
    /// </summary>
    void CheckConfigUpdate()
    {
        //检查配置更新
        string questionUrl = "http://localhost/gameConfig.json";
        StartCoroutine(RequestNetworkFile(questionUrl, (result, content, data) =>
        {
            if (result)
            {
                GameConfig gameConfig = LitJson.JsonMapper.ToObject<GameConfig>(content);
                if (gameConfig.version != ConfigDataMgr.Instance.gameConfig.version)
                {
                    StartCoroutine(UpdateResource(gameConfig));
                }
                else
                {
                    CheckLoginState();
                }
            }
            else
            {
                CheckLoginState();
            }
        }));
    }
    /// <summary>
    /// 更新流程  1.转换语音  2.更新Video图片
    /// </summary>
    /// <returns>The resource.</returns>
    /// <param name="gameConfig">Game config.</param>
    IEnumerator UpdateResource(GameConfig gameConfig)
    {
        yield return StartCoroutine(TurnString2Audio(gameConfig));
        yield return StartCoroutine(LoadVideoTexture(gameConfig));

        //记录文件映射表
        ConfigDataMgr.Instance.WriteAudioDictData();
        //更新题库数据
        ConfigDataMgr.Instance.gameConfig = gameConfig;
        ConfigDataMgr.Instance.WriteGameConfigData();

        //更新结束检测登录
        CheckLoginState();
    }

    /// <summary>
    /// 转换语音文件
    /// </summary>
    /// <returns>The string2 audio.</returns>
    /// <param name="gameConfig">Game config.</param>
    IEnumerator TurnString2Audio(GameConfig gameConfig)
    {
        List<string> updateList = new List<string>();

        //通用语音检测部分
        CheckStringToAudio(ConfigDataMgr.ExamStart, updateList);
        //试题语音检测部分
        for (int i = 0; i < gameConfig.questions.Count; i++)
        {
            QuestionData questionData = gameConfig.questions[i];
            CheckStringToAudio(questionData.question, updateList);
        }
        for (int i = 0; i < updateList.Count; i++)
        {
            //设置转换进度
            uiLoginWindow.SetProgress((float)i / updateList.Count);
            yield return StartCoroutine(ttsString2Audio.Synthesis(updateList[i], result =>
            {
                if (result.Success)
                {
                    Debug.LogFormat("Trun Success：{0}", updateList[i]);
                    string fileName = System.Guid.NewGuid().ToString("N");
                    ResourcesMgr.Instance.WriteAudioFile(fileName, result.data);
                    ConfigDataMgr.Instance.resourceDict.Add(updateList[i], fileName);
                }
                else
                {
                    Debug.LogErrorFormat("Error:Str2Audio errorno<{0}> errormsg<{1}>", result.err_no, result.err_msg);
                }
            }));
        }
        uiLoginWindow.SetProgress(1.0f);
    }

    /// <summary>
    /// 下载视频缓冲图
    /// </summary>
    /// <returns>The video texture.</returns>
    /// <param name="gameConfig">Game config.</param>
    IEnumerator LoadVideoTexture(GameConfig gameConfig)
    {
        yield return null;

        List<string> updateList = new List<string>();
        foreach (var item in gameConfig.video)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                if (!ConfigDataMgr.Instance.resourceDict.ContainsKey(item.Value[i].imgurl))
                {
                    updateList.Add(item.Value[i].imgurl);
                }
            }
        }

        for (int i = 0; i < updateList.Count; i++)
        {
            //设置转换进度
            uiLoginWindow.SetProgress((float)i / updateList.Count);
            yield return RequestNetworkFile(updateList[i], (result, content, data) =>
            {
                if (result)
                {
                    string fileName = System.Guid.NewGuid().ToString("N");
                    ResourcesMgr.Instance.WriteTextureFile(fileName, data);
                    ConfigDataMgr.Instance.resourceDict.Add(updateList[i], fileName);
                }
            });
        }
        uiLoginWindow.SetProgress(1.0f);
    }


    void CheckStringToAudio(string str2audio, List<string> turnList)
    {
        if (!ConfigDataMgr.Instance.resourceDict.ContainsKey(str2audio))
        {
            turnList.Add(str2audio);
        }
    }


    /// <summary>
    /// Requests the network file.
    /// </summary>
    /// <returns>The network file.</returns>
    /// <param name="url">URL.</param>
    /// <param name="callback">Callback.</param>
    IEnumerator RequestNetworkFile(string url, Action<bool, string, byte[]> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        bool isError = false;

        if (request.isNetworkError)
        {
            Debug.LogErrorFormat("isNetworkError [{0}] [{1}]", url, request.error);
            isError = true;
        }
        else if (request.isHttpError)
        {
            Debug.LogErrorFormat("isHttpError [{0}] [{1}]", url, request.responseCode);
            isError = true;
        }
        else
        {
            if (request.responseCode == 200 || request.responseCode == 0)
            {
                callback(true, request.downloadHandler.text, request.downloadHandler.data);
            }
            else
            {
                Debug.LogErrorFormat("response code error [{0}]", request.responseCode);
                isError = true;
            }
        }

        if (isError)
        {
            callback(false, null, null);
        }
    }

}
