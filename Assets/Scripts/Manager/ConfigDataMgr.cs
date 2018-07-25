using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigDataMgr : XSingleton<ConfigDataMgr>
{
    public static string ExamStartTip = "下面将进行模拟夜间行驶场景灯光使用的考试，请按语音指令在5秒内做出相应的灯光操作";
    public static QuestionData ExamEnd = new QuestionData() { question = "模拟夜间考试完成请关闭所有灯光", answer = "答案：关闭所有灯光" };

    public GameConfig gameConfig = new GameConfig();
    public List<QuestionData> questions { get { return gameConfig.questions; } }
    public Dictionary<string, string> resourceDict = new Dictionary<string, string>();

    public override void OnInit()
    {
        base.OnInit();
        ReadGameConfigData();
        ReadAudioDictData();
    }

    /// <summary>
    /// Reads the game config data from gameConfig.json.
    /// </summary>
    public void ReadGameConfigData()
    {
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "gameConfig.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameConfig gameConfig = LitJson.JsonMapper.ToObject<GameConfig>(json);
        }
    }
    /// <summary>
    /// Writes the game config data to gameConfig.json.
    /// </summary>
    public void WriteGameConfigData()
    {
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "gameConfig.json");
        string json = LitJson.JsonMapper.ToJson(gameConfig);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Reads the audio dict data from audioDict.json.
    /// </summary>
    public void ReadAudioDictData()
    {
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "resourceDict.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            resourceDict = LitJson.JsonMapper.ToObject<Dictionary<string, string>>(json);
        }
    }
    /// <summary>
    /// Writes the audio dict data to audioDict.json.
    /// </summary>
    public void WriteAudioDictData()
    {
        string filePath = Path.Combine(ResourcesMgr.ConfigPath, "resourceDict.json");
        string json = LitJson.JsonMapper.ToJson(resourceDict);
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
    }

    public void CheckTurnAudio(string text)
    {

    }


    public QuestionData GetQuestionByIndex(int index)
    {
        if (index >= 0 && index < questions.Count)
        {
            return questions[index];
        }
        else
        {
            Debug.LogError("question index error");
            return null;
        }
    }
}
