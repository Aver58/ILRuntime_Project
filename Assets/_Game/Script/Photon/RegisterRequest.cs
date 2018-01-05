using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using GameEnum;
using UnityEngine;

public class RegisterRequest : Request
{
    [HideInInspector]
    public string username;
    [HideInInspector]
    public string password;

    private RegisterPanel registerPanel;
    public override void Start()
    {
        base.Start();
        registerPanel = GetComponent<RegisterPanel>();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, username);
        data.Add((byte)ParameterCode.Password, password);
        PhotonClientManager.peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        // registerPanel.OnReigsterResponse(returnCode);
    }
}
