using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {
    string name;
    string psw;
    InputField username;
    InputField password;
    Text ResponseText;

    private LoginRequest loginRequest;

    void Start()
    {
        username = GameObject.Find("Canvas/Login/name").GetComponent<InputField>();
        password = GameObject.Find("Canvas/Login/password").GetComponent<InputField>();
        ResponseText = GameObject.Find("Canvas/Login/ResponseText").GetComponent<Text>();

        GameObject BtnStart = GameObject.Find("Canvas/Start");
        BtnStart.GetComponent<Button>().onClick.AddListener(() => { SendRequest(); });
        GameObject Login = GameObject.Find("Canvas/Login/Login");
        Login.GetComponent<Button>().onClick.AddListener(() => { OnLoginBtn(); });
        GameObject Register = GameObject.Find("Canvas/Login/Register");
        Register.GetComponent<Button>().onClick.AddListener(() => { SendRequest(); });

        loginRequest = GetComponent<LoginRequest>();
    }
    /// <summary>
    /// 登录按钮
    /// </summary>
    private void OnLoginBtn()
    {
        ResponseText.text = "";
        loginRequest.Username = username.text;
        loginRequest.Password = password.text;
        loginRequest.DefaultRequest();
        //Dictionary<byte, object> data = new Dictionary<byte, object>();
        //data.Add((byte)UserCode.Username, name);
        //data.Add((byte)UserCode.Password, psw);
        //PhotonClientManager.peer.OpCustom((byte)OperatedCode.Login, data, true);
        Debug.Log("OnLoginBtn");
    }
    /// <summary>
    /// 登录成功
    /// </summary>
    /// <param name="returnCode"></param>
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            // 跳转到下一个场景 
            // SceneManager.LoadScene("Game");
            Debug.Log("登录成功");
            ResponseText.text = "登录成功";
        }
        else
        {
            ResponseText.text = "用户名或密码错误";
        }
    }
    /// <summary>
    /// 注册按钮
    /// </summary>
    public void OnRegister()
    {
        //goLoginPanel.SetActive(false);
        //RegisterPanel.SetActive(true);
        ResponseText.text = "敬请期待";

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
    
    
}
