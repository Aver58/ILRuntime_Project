using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//客户端向服务器的请求
public abstract class Request : MonoBehaviour
{
    public OperationCode OpCode;//请求类型
    public abstract void DefaultRequest();//默认的请求

    //服务器端返回的响应
    public abstract void OnOperationResponse(OperationResponse operationResponse);

    public virtual void Start()
    {
        //添加请求到集合
        PhotonClientManager.Instance.AddRequest(this);
    }
    public void OnDestroy()
    {
        //从集合中删除当前请求
        PhotonClientManager.Instance.RemoveRequest(this);
    }
}
