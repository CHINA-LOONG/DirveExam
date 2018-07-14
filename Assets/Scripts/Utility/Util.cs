using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 将Unicode格式转换中文字符串
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string Unicode2String(string source)
    {
        return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase).Replace(
             source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
    }

    /// <summary>
    /// 将mp3格式的字节数组转换为audioclip
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static AudioClip GetAudioClipFromMP3ByteArray(byte[] mp3Data)
    {
        var mp3MemoryStream = new MemoryStream(mp3Data);
        MP3Sharp.MP3Stream mp3Stream = new MP3Sharp.MP3Stream(mp3MemoryStream);

        //Get the converted stream data
        MemoryStream convertedAudioStream = new MemoryStream();
        byte[] buffer = new byte[2048];
        int bytesReturned = -1;
        int totalBytesReturned = 0;

        while (bytesReturned != 0)
        {
            bytesReturned = mp3Stream.Read(buffer, 0, buffer.Length);
            convertedAudioStream.Write(buffer, 0, bytesReturned);
            totalBytesReturned += bytesReturned;
        }

        Debug.Log("MP3 file has " + mp3Stream.ChannelCount + " channels with a frequency of " +
                  mp3Stream.Frequency);

        byte[] convertedAudioData = convertedAudioStream.ToArray();

        //bug of mp3sharp that audio with 1 channel has right channel data, to skip them
        byte[] data = new byte[convertedAudioData.Length / 2];
        for (int i = 0; i < data.Length; i += 2)
        {
            data[i] = convertedAudioData[2 * i];
            data[i + 1] = convertedAudioData[2 * i + 1];
        }

        Wav wav = new Wav(data, mp3Stream.ChannelCount, mp3Stream.Frequency);

        AudioClip audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
        audioClip.SetData(wav.LeftChannel, 0);

        return audioClip;
    }
}
