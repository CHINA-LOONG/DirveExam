using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourcesMgr : Singleton<ResourcesMgr>
{

    public static string ConfigPath
    {
        get
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "config");
#else
            return Path.Combine(Application.persistentDataPath, "config");
#endif
        }
    }
    public static string AudioPath
    {
        get
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "audio");
#else
            return Path.Combine(Application.persistentDataPath, "audio");
#endif
        }
    }



    public GameObject LoadUIPrefab(string assetName){
        return Resources.Load(assetName) as GameObject;
    }
}
