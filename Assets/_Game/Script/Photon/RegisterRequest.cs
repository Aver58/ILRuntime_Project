using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using Common;
using UnityEngine;

public class RegisterRequest : Request
{
    [HideInInspector]
    public string username;
    [HideInInspector]
    public string password;

    private LoginPanel loginPanel;
    public override void Start()
    {
        OpCode = OperatedCode.Register;
        base.Start();
        loginPanel = GetComponent<LoginPanel>();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)UserCode.Username, username);
        data.Add((byte)UserCode.Password, password);
        PhotonClientManager.peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        loginPanel.OnRegisterResponse(returnCode);
    }
}
