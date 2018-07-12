using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigDataMgr : XSingleton<ConfigDataMgr>
{
    public static string ExamStart = "灯光考试开始:";

    public GameConfig gameConfig = new GameConfig();
    public List<QuestionData> questions = new List<QuestionData>();
    public Dictionary<string, string> audioDict = new Dictionary<string, string>();

    public override void OnInit()
    {
        base.OnInit();
        //audioDict.Add("aaa", "bbb");
        //audioDict.Add("ccc", "bbb");
    }

    /// <summary>
    /// Reads the game config data from gameConfig.json.
    /// </summary>
    public void ReadGameConfigData(){
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "gameConfig.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            questions = LitJson.JsonMapper.ToObject<List<QuestionData>>(json);
        }
    }
    /// <summary>
    /// Writes the game config data to gameConfig.json.
    /// </summary>
    public void WriteGameConfigData(){
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "gameConfig.json");
        string json = LitJson.JsonMapper.ToJson(gameConfig);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Reads the audio dict data from audioDict.json.
    /// </summary>
    public void ReadAudioDictData(){
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "audioDict.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            audioDict = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(json);
        }
    }
    /// <summary>
    /// Writes the audio dict data to audioDict.json.
    /// </summary>
    public void WriteAudioDictData(){
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "audioDict.json");
        string json = LitJson.JsonMapper.ToJson(audioDict);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
    }


    public void CheckTurnAudio(string text){
        
    }


}
