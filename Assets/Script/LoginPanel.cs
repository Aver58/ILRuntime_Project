//using Common;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//public class LoginPanel : MonoBehaviour
//{
//    public GameObject goLoginPanel;
//    public GameObject RegisterPanel;
//    private LoginRequest loginRequest;
//    public InputField usernameIF;
//    public InputField passwordIF;
//    public Text hintMessage;
//    void Start()
//    {
//        loginRequest = GetComponent<LoginRequest>();
//    }

//    public void OnLoginBtn()
//    {
//        hintMessage.text = "";
//        loginRequest.Username = usernameIF.text;
//        loginRequest.Password = passwordIF.text;
//        loginRequest.DefaultRequest();

//        Debug.Log("OnLoginBtn");
//    }

//    public void OnRegister()
//    {
//        goLoginPanel.SetActive(false);
//        RegisterPanel.SetActive(true);
//    }
//    public void OnLoginResponse(ReturnCode returnCode)
//    {
//        if (returnCode == ReturnCode.Success)
//        {
//            // 跳转到下一个场景 
//            // SceneManager.LoadScene("Game");
//            Debug.Log("登录成功");
//        }
//        else
//        {
//            hintMessage.text = "用户名或密码错误";
//        }
//    }
//}