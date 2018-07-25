using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioObject
{
    public AudioObject()
    {
        //flagController = ControlAudioFlag();
    }
    public IEnumerator flagController;

    public AudioSource source;
    public bool isUsed = false;
    public bool useTimeScale = false;
    public float playTime
    {
        get
        {
            if (source != null && source.clip != null)
            {
                return source.clip.length / source.pitch;
            }
            else
            {
                return 0;
            }
        }
    }
    public IEnumerator ControlAudioFlag()
    {
        // Debug.Log("开始");
        //此处时间缩放貌似是在不好用
        isUsed = true;
        source.spatialBlend = 0;
        yield return new WaitForEndOfFrame();
        source.Play();
        yield return new WaitForSeconds(playTime
                                        + 0.2f);

        source.clip = null;
        isUsed = false;
        //  Debug.Log("结束");
    }
}
public class AudioSystemMgr : MonoBehaviour
{
    private static AudioSystemMgr mInst;
    private static byte dontCreate = 0;

    void OnApplicationQuit()
    {
        dontCreate = 1;
    }
    public static AudioSystemMgr Instance
    {
        get
        {
            if (mInst == null && AudioSystemMgr.dontCreate == 0)
            {
                GameObject go = new GameObject("AudioSystemMgr");
                mInst = go.AddComponent<AudioSystemMgr>();
                mInst.Init();
                DontDestroyOnLoad(go);
            }
            return AudioSystemMgr.mInst;
        }
    }

    //背景-  使用音轨音源数量
    private static readonly int audioMusicLength = 2;
    //UI--事件--技能等音效音轨音源数量
    private static readonly int audioSoundLength = 5;

    private AudioObject[] audioMusics;
    private AudioObject[] audioSounds;

    #region -------------设置音量-------------

    private readonly float musicVolumeRatio = 0.2f;
    private readonly float soundVolumeRatio = 0.2f;
    private readonly float speechVolumeRatio = 1.0f;

    private float musicVolume = 0.5f;
    private float soundVolume = 0.5f;
    private float speechVolume = 0.5f;
    public float MusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = Mathf.Clamp01(value); setAudioVolume(audioMusics, musicVolume * musicVolumeRatio); }
    }
    public float SoundVolume
    {
        get { return soundVolume; }
        set { soundVolume = Mathf.Clamp01(value); setAudioVolume(audioSounds, soundVolume * soundVolumeRatio); }
    }

    #endregion

    #region -------------开关声音-------------

    private bool switchMusic = true;
    private bool switchSpeech = true;
    private bool switchSound = true;
    public bool SwitchMusic
    {
        get { return switchMusic; }
        set
        {
            if (switchMusic != value)
            {
                switchMusic = value;
                setAudioMute(audioMusics, !switchMusic);
            }
        }
    }
    public bool SwitchSound
    {
        get { return switchSound; }
        set
        {
            if (switchSound != value)
            {
                switchSound = value;
                setAudioMute(audioSounds, !switchSound);
            }
        }
    }
    //设置声音的开关
    public bool SwitchAudio
    {
        set
        {
            SwitchMusic = value;
            SwitchSound = value;
        }
    }
    #endregion

    private Transform mTrans;

    public void Init()
    {
        mTrans = transform;

        //初始化背景音乐播放器
        audioMusics = new AudioObject[audioMusicLength];
        for (int i = 0; i < audioMusicLength; i++)
        {
            audioMusics[i] = addAudio("audio_music", true);
        }
        setAudioVolume(audioMusics, MusicVolume * musicVolumeRatio);
        setAudioMute(audioMusics, !SwitchMusic);

        //初始化音效播放器
        audioSounds = new AudioObject[audioSoundLength];
        for (int i = 0; i < audioSoundLength; i++)
        {
            audioSounds[i] = addAudio("audio_sound", false);
        }
        setAudioVolume(audioSounds, SoundVolume * soundVolumeRatio);
        setAudioMute(audioSounds, !SwitchSound);
    }

    AudioObject addAudio(string objName, bool loop)
    {
        GameObject obj = new GameObject(objName);
        obj.transform.parent = mTrans;
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = loop;

        AudioObject audioObj = new AudioObject()
        {
            source = source,
            isUsed = false,
            useTimeScale = false
        };
        return audioObj;
    }
    AudioObject getUnUseAudio(AudioObject[] audioList)
    {
        for (int i = 0; i < audioList.Length; i++)
        {
            if (!audioList[i].isUsed)
            {
                return audioList[i];
            }
        }
        return null;
    }

    void setAudioVolume(AudioObject[] audios, float volume)
    {
        AudioObject item;
        for (int i = 0; i < audios.Length; ++i)
        {
            item = audios[i];
            item.source.volume = volume;
        }
    }
    void setAudioMute(AudioObject[] audios, bool mute)
    {
        AudioObject item;
        for (int i = 0; i < audios.Length; ++i)
        {
            item = audios[i];
            item.source.mute = mute;
        }
    }

    /********************************播放效果音效*********************************************/
    public AudioObject PlaySoundByClip(AudioClip audioClip)
    {
        AudioObject audioObj = getUnUseAudio(audioSounds);
        if (audioObj != null)
        {
            if (audioClip == null)
            {
                return null;
            }
            else
            {
                audioObj.source.clip = audioClip;
                audioObj.flagController = audioObj.ControlAudioFlag();
                StartCoroutine(audioObj.flagController);
                return audioObj;
            }
        }
        else
        {
            return null;
        }
    }


    public void StopSoundByAudio(AudioObject audioObj)
    {
        if (audioObj!=null&&audioObj.isUsed)
        {
            StopCoroutine(audioObj.flagController);
            audioObj.isUsed = false;
            audioObj.source.Stop();
            audioObj.source.clip = null;
        }
    }
    public void StopAllSound(bool immediate = true)
    {
        for (int i = 0; i < audioSoundLength; i++)
        {
            if (audioSounds[i].isUsed)
            {
                audioSounds[i].source.clip = null;
                audioSounds[i].source.Stop();
            }
        }
    }
}
