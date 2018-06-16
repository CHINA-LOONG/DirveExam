using Baidu.Aip.Speech;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AipKit {
    Tts client;
    public AipKit()
    {
        // 设置APPID/AK/SK
        var APP_ID = "你的 App ID";
        var API_KEY = "XmrFwCyGGeknfZcMslbXm3dS";
        var SECRET_KEY = "hNAarHUfVaeF1D2vfKn5r7xG9p7ExPFN";
        client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
    }
    // 合成
    public void Tts()
    {
        // 可选参数
        var option = new Dictionary<string, object>()
    {
        {"spd", 5}, // 语速
        {"vol", 7}, // 音量
        {"per", 4}  // 发音人，4：情感度丫丫童声
    };
        var result = client.Synthesis("众里寻他千百度", option);

        if (result.ErrorCode == 0)  // 或 result.Success
        {
            File.WriteAllBytes("合成的语音文件本地存储地址.mp3", result.Data);
        }
    }
}
