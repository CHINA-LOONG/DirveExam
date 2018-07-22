using System.Collections.Generic;

public enum CarType
{
    DAZHONG = 1,
    AILISHE = 2
}

public class Data_Base { }

/// <summary>
/// 游戏配置文件
/// </summary>
public class GameConfig : Data_Base
{
    /// <summary>
    /// 配置版本
    /// </summary>
    public string version = "0";
    /// <summary>
    /// 试题列表
    /// </summary>
    public List<QuestionData> questions = new List<QuestionData>();
    /// <summary>
    /// 视频教学
    /// </summary>
    public Dictionary<string, List<VideoData>> video = new Dictionary<string, List<VideoData>>();
}

/// <summary>
/// 試題的數據結構
/// </summary>
public class QuestionData : Data_Base
{
    /// <summary>
    /// 轉語音的文字描述
    /// </summary>
    public string question;
    /// <summary>
    /// 试题答案文案.
    /// </summary>
    public string answer;
}

/// <summary>
/// 车型视频数据结构
/// </summary>
public class VideoData : Data_Base
{
    public string imgurl;
    public string videourl;
    public string videoname;
}

////抽奖箱子
//public class pendingChest : Data_Base
//{
//    public int index;
//    public int type;
//    public int value;
//    public int status;
//
//    public pendingCard card;
//
//
//    public static void Parse(LitJson.JsonData json, out pendingChest data)
//    {
//        data = new pendingChest();
//        try
//        {
//            data.index = int.Parse(json["index"].ToString());
//            data.type = int.Parse(json["type"].ToString());
//            data.value = int.Parse(json["value"].ToString());
//            data.status = int.Parse(json["status"].ToString());
//            if (json.Keys.Contains("pendingCard") && data.type != 1 && data.type != 2)
//            {
//                pendingCard.Parse(json["pendingCard"], out data.card, (pendingCard.EChestType)(data.type - 3));
//            }
//            else
//            {
//                data.card = null;
//            }
//        }
//        catch (System.Exception)
//        {
//            data = null;
//            throw;
//        }
//    }
//
//}
