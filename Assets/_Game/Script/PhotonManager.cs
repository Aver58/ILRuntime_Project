using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;

public class PhotonManager : MonoBehaviour,IPhotonPeerListener{
    PhotonPeer peer;
    // Use this for initialization
    void Start()
    {
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "LoadBalancing");
    }

    // Update is called once per frame
    void Update()
    {
        peer.Service();
    }
    void SendMessage()
    {
        //peer.OpCustom();
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        print(Enum.Parse(typeof(DebugLevel),level.ToString()).ToString()+message);
        throw new System.NotImplementedException();
    }

    public void OnEvent(EventData eventData)
    {
        print(eventData.ToString());
        throw new System.NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                print("连接成功！");
                break;
            case StatusCode.Disconnect:
                print("断开连接！");
                break;
            case StatusCode.Exception:
                print("异常！");
                break;
            case StatusCode.ExceptionOnConnect:
                break;
            case StatusCode.SecurityExceptionOnConnect:
                break;
            case StatusCode.QueueOutgoingReliableWarning:
                break;
            case StatusCode.QueueOutgoingUnreliableWarning:
                break;
            case StatusCode.SendError:
                break;
            case StatusCode.QueueOutgoingAcksWarning:
                break;
            case StatusCode.QueueIncomingReliableWarning:
                break;
            case StatusCode.QueueIncomingUnreliableWarning:
                break;
            case StatusCode.QueueSentWarning:
                break;
            case StatusCode.ExceptionOnReceive:
                break;
            case StatusCode.TimeoutDisconnect:
                break;
            case StatusCode.DisconnectByServer:
                break;
            case StatusCode.DisconnectByServerUserLimit:
                break;
            case StatusCode.DisconnectByServerLogic:
                break;
            case StatusCode.EncryptionEstablished:
                break;
            case StatusCode.EncryptionFailedToEstablish:
                break;
            default:
                break;
        }
    }
    
}
