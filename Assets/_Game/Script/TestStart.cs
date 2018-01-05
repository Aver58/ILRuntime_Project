using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestStart : MonoBehaviour {

    void Start()
    {
        GameObject BtnStart = GameObject.Find("Canvas/Titles");
        BtnStart.GetComponent<Button>().onClick.AddListener(() => { SendRequest(); });
    }

    /// <summary>
    /// 向服务器发送请求
    /// </summary>
    void SendRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add(1, 300);
        data.Add(2, "客户端：客户端参数数据");
        PhotonClientManager.peer.OpCustom(1, data, true);
        Debug.Log("客户端：向服务器发送请求");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
