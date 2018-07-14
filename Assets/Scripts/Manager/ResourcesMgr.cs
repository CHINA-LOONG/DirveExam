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
            string configPath = Path.Combine(Application.dataPath, "config");
#else
            string configPath = Path.Combine(Application.persistentDataPath, "config");
#endif
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            return configPath;
        }
    }
    public static string AudioPath
    {
        get
        {
#if UNITY_EDITOR
            string audioPath = Path.Combine(Application.dataPath, "audio");
#else
            string audioPath = Path.Combine(Application.persistentDataPath, "audio");
#endif
            if (!Directory.Exists(audioPath))
            {
                Directory.CreateDirectory(audioPath);
            }
            return audioPath;
        }
    }
    
    public GameObject LoadUIPrefab(string assetName){
        return Resources.Load(assetName) as GameObject;
    }

    public AudioClip GetAudioWithStr(string content)
    {
        string fileName = ConfigDataMgr.Instance.audioDict[content];
        return ReadAudioFile(fileName);
    }

    public AudioClip ReadAudioFile(string fileName)
    {
        byte[] data = File.ReadAllBytes(Path.Combine(AudioPath, fileName));
        AudioClip audioClip = Util.GetAudioClipFromMP3ByteArray(data);
        return audioClip;
    }
    public void WriteAudioFile(string fileName, byte[] audio)
    {
        File.WriteAllBytes(Path.Combine(AudioPath, fileName), audio);
    }
}
