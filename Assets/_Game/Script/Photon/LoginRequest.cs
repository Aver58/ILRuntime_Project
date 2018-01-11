//using ExitGames.Client.Photon;
//using Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//public class LoginRequest : Request
//{
//    [HideInInspector]
//    public string Username;
//    [HideInInspector]
//    public string Password;

//    private LoginPanel loginPanel;

//    public override void Start()
//    {
//        base.Start();
//        loginPanel = GetComponent<LoginPanel>();
//    }

//    public override void DefaultRequest()
//    {
//        Dictionary<byte, object> data = new Dictionary<byte, object>();
//        data.Add((byte)ParameterCode.Username, Username);
//        data.Add((byte)ParameterCode.Password, Password);
//        PhotonClientManager.peer.OpCustom((byte)OpCode, data, true);
//    }

//    public override void OnOperationResponse(OperationResponse operationResponse)
//    {
//        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;

//        loginPanel.OnLoginResponse(returnCode);
//    }
//}
