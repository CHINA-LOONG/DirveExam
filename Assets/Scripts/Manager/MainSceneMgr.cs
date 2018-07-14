﻿using System.Collections;
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
        if (!GlobalManager.Instance.isLogin)
        {
            //未登录
            uiLoginWindow = UIManager.Instance.OpenWindow<UILoginWindow>();
        }
        else
        {
            //已登录
            uiLoginWindow = UIManager.Instance.OpenWindow<UILoginWindow>();
        }
        CheckConfigUpdate();
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
                    StartCoroutine(TurnString2Audio(gameConfig));
                }
            }
        }));
    }

    IEnumerator TurnString2Audio(GameConfig gameConfig)
    {
        List<string> turnList = new List<string>();

        //通用语音检测部分
        CheckStringToAudio(ConfigDataMgr.ExamStart, turnList);
        //试题语音检测部分
        for (int i = 0; i < gameConfig.questions.Count; i++)
        {
            QuestionData questionData = gameConfig.questions[i];
            CheckStringToAudio(questionData.question, turnList);
        }
        //设置转换进度
        for (int i = 0; i < turnList.Count; i++)
        {
            yield return StartCoroutine(ttsString2Audio.Synthesis(turnList[i], result =>
            {
                if (result.Success)
                {
                    Debug.LogFormat("Trun Success：{0}", turnList[i]);
                    string fileName = System.Guid.NewGuid().ToString("N");
                    ResourcesMgr.Instance.WriteAudioFile(fileName, result.data);
                    ConfigDataMgr.Instance.audioDict.Add(turnList[i], fileName);
                }
                else
                {
                    Debug.LogErrorFormat("Error:Str2Audio errorno<{0}> errormsg<{1}>", result.err_no, result.err_msg);
                }
            }));
            //更新转换进度
        }
        //记录文件映射表
        ConfigDataMgr.Instance.WriteAudioDictData();
        //更新题库数据
        ConfigDataMgr.Instance.gameConfig = gameConfig;
        ConfigDataMgr.Instance.WriteGameConfigData();
    }

    void CheckStringToAudio(string str2audio,List<string> turnList)
    {
        if (!ConfigDataMgr.Instance.audioDict.ContainsKey(str2audio))
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
