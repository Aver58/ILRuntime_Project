using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test1 : MonoBehaviour {
    public Text t;
    private void Start()
    {
        //this.GetComponent<Button>().onClick.AddListener(delegate () { click(); });
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        UnityAction<BaseEventData> click1 = new UnityAction<BaseEventData>(OnPointerUp);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerUp;
        myclick.callback.AddListener(click1);
        trigger.triggers.Add(myclick);

        click1 = new UnityAction<BaseEventData>(click);
        myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerDown;
        myclick.callback.AddListener(click1);
        
        trigger.triggers.Add(myclick);
    }
    AudioClip recordClip;//录音
    public void click(BaseEventData arg0)
    {
        recordClip = StartMicro(out recordClip);
        t.text = "click";
        //GameObject Btn = transform.Find("BtnVoice").gameObject;

    }
    private void OnPointerUp(BaseEventData arg0)
    {
        #region LocalTest
        //注意声道
        //AudioClip mClip = AudioClip.Create("playRecordClip", tempByte.Length, 2, 44100, false);
        //mClip.SetData(tempByte);//将BYTE存入clip
        AudioSource source = this.GetComponent<AudioSource>();
        t.text = "StopRecord";

        source.clip = recordClip;//结束时候播放
        source.Play();
        #endregion
    }
    public static AudioClip StartMicro(out AudioClip audio)
    {
        int lengthSec = 10;//最长10秒
        int frequency = 44100;//44100Hz，可以使用GetDeviceCaps 属性获取支持的样本比率的范围。一般的无损音质是44100，即每秒用44100个float来表示1秒的音频内容
        //AudioClip mCurrentClip;
        //if (Microphone.devices.Length < 0) return 0;

        Microphone.End(null);
        audio = Microphone.Start(null, false, lengthSec, frequency);//开始录音
        //if (mCurrentClip)
        //{
        //    //byte[] wavByte = mCurrentClip.GetData();                       //获得录音BYTE数组
        //    //需要压缩
        //    string recordedAudioPath;
        //    byte[] data = WavUtility.FromAudioClip(mCurrentClip, out recordedAudioPath, true);
        //    string filePath = recordedAudioPath;
        //    return data;
        //}
        return audio;
    }
}
