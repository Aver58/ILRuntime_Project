using UnityEngine;
using System.Collections;
using System.IO;
using System;

/// <summary>
/// 服务器实现.
/// </summary>
public class ServerDemo : MonoBehaviour
{
	//临时消息接收
	string currentMsg = "";
	Vector2 scrollViewPosition;

	void Start ()
	{
		scrollViewPosition = Vector2.zero;
		//消息委托
		NetUtility.Instance.SetDelegate ((string msg) => {
			Debug.Log (msg);
			currentMsg += msg + "\r\n";
		});
		//开启服务器
		NetUtility.Instance.ServerStart();
	}

	void OnGUI ()
	{
		scrollViewPosition = GUILayout.BeginScrollView (scrollViewPosition, GUILayout.Width (500), GUILayout.Height (300));
		//消息展示
		GUILayout.Label (currentMsg);
		GUILayout.EndScrollView ();
	}
}
