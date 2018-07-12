using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class MainSceneMgr : MonoBehaviour
{
    private void Awake()
    {
        if (!GlobalManager.Instance.isLogin)
        {
            //未登录
            UIManager.Instance.OpenWindow<UILoginWindow>();
        }
        else
        {
            //已登录
            UIManager.Instance.OpenWindow<UILoginWindow>();
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
                    //初始化数据
                    ConfigDataMgr.Instance.gameConfig = gameConfig;
                    ConfigDataMgr.Instance.WriteGameConfigData();
                }
            }
        }));
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
