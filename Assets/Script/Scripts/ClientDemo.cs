using UnityEngine;
using System.Collections;
using System.Text;
using System;
/// <summary>
/// 客户端实现
/// </summary>
public class ClientDemo : MonoBehaviour
{
	//待解析地址
	public string wwwAddress = "";

	void Start ()
	{
		//消息处理
		NetUtility.Instance.SetDelegate ((string msg) => {
			Debug.Log (msg + "\r\n");
		});
		//连接服务器
		NetUtility.Instance.ClientConnnect ();
		//开启协程
		StartCoroutine (ServerStart ());
	}

	IEnumerator ServerStart ()
	{
		//加载网页数据
		WWW www = new WWW (wwwAddress);
		yield return www;
		//编码获取内容
		string content = UTF8Encoding.UTF8.GetString (www.bytes);
		//内容测试
		Debug.Log (content);
		//待发送对象
		NetModel nm = new NetModel ();
		//消息体
		nm.senderIp = "127.0.0.1";
		nm.content = content;
		nm.time = DateTime.Now.ToString ();
		//发送数据对象
		NetUtility.Instance.SendMsg (nm);
	}
}
