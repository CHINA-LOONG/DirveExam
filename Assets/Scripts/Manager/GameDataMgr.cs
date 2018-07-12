using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr : XSingleton<GameDataMgr>
{
    private string accessToken;
    public string AcceccToken
    {
        get
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = PlayerPrefs.GetString("AccessToken", null);
            }
            return accessToken;
        }
        set {
            PlayerPrefs.SetString("AccessToken", value);
            accessToken = value; 
        }
    }
}
