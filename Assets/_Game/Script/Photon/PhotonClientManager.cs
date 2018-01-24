using UnityEngine;  
using System.Collections;  
using ExitGames.Client.Photon;  
using ExitGames.Client.Photon.Chat;
using System;
using System.Collections.Generic;
using Common;
/// <summary>
/// 通讯监听类的实例
/// </summary>
public class PhotonClientManager : MonoBehaviour, IPhotonPeerListener
{

    #region 定义photon客户端设置需要的变量  
    
    private Dictionary<OperatedCode, Request> RequestDict = new Dictionary<OperatedCode, Request>();//所有请求的一个集合

    public static PhotonClientManager Instance;                                                       // PhotonClientManager的单例模式 
 
    public static PhotonPeer peer;                                                                    // 连接PHTON服务器用的 连接工具 

    private ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp;                           // 连接工具PhotonPeer 使用的协议 
  
    private string SeverIpAdress = "127.0.0.1:5055";                                                  // 连接工具PhotonPeer 要连接的IP地址和端口号 

    private string SeverName = "MyServer";                                                            // 连接工具PhotonPeer 要连接的服务器名字
    

    #endregion
  
    #region MonoBehaviour脚本原生方法  
    void Awake()
    {
        ///创建单例模式  
        //如果单例模式还没有设置，则让这个脚本成为单例模式，如果存在，则删除这个脚本以及物体  
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.transform);
        }
        //设置脚本属性,换场景不删除、后台运行等  
        DontDestroyOnLoad(this.transform);
        Application.runInBackground = true;

        //建立PhotonPeer实例(第一个参数为带有IPhotonPeerListener的脚本（通讯监听类的实例），第2个为选用的通讯协议);  
        peer = new PhotonPeer(this, connectionProtocol);
        //连接器 将要连接的服务器IP端口加上服务器名字  
        peer.Connect(SeverIpAdress, SeverName);
        //连接器 开始发送队列中拥有的 通讯信息，所有固话请求以及通讯请求都以队列的方式存储在连接器中，当执行peer.Service()方法时才对服务器发送他们  
    }

    void Start()
    {

    }

    void Update()
    {
        //每帧检查peer中是否有命令需要发送，当peer有命令时，开始发送请求  
        if (peer != null) peer.Service();
    }
    //当关闭客户端时断开与服务器的连接
    void OnDestroy()
    {
        if (peer != null && (peer.PeerState == PeerStateValue.Connected)) // 当peer不等于空 而且 正在运行的时候  
        {
            peer.Disconnect();//断开连接  
        }
    }

    #endregion

    /// <summary>
    /// 向服务器发送请求
    /// </summary>
    /// <param name="reqEnum">请求枚举</param>
    /// <param name="message">要发的信息</param>
    /// <param name="isArrial">是否发送成功</param>
    public static void SendRequest(OperatedCode reqEnum, Dictionary<byte, object> message, bool isArrial)
    {
        peer.OpCustom((byte)reqEnum, message, isArrial);//photon构建通讯信息的方法，他发送的内容我们叫做请求参数，这个参数必须是一个字典
    }


    #region photon接口方法  

    //当photonclient要输出debug时，会调用此方法  
    public void DebugReturn(DebugLevel level, string message)
    {

    }


    /// <summary>
    /// 处理服务器广播的事件 响应SendEvent方法
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                Debug.Log("客户端：收到服务器端发送过来的事件");
                break;
        }
    }

    //当客户端状态改变时会调用此方法  
    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(statusCode);
    }

    /// <summary>
    /// 客户端向服务器发起一个请求以后服务器处理完以后 就会给客户端一个响应
    /// 响应服务端的SendOperationResponse方法
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {

        Dictionary<byte, object> dict = operationResponse.Parameters;
        object v = null;
        dict.TryGetValue(1, out v);
        //Console.WriteLine("Get value from server " + v.ToString());
        switch (operationResponse.OperationCode)//处理服务器端作出的响应
        {
            case 1:
                Dictionary<byte, object> data = operationResponse.Parameters;

                object intValue;
                object stringValue;
                data.TryGetValue(1, out intValue);
                data.TryGetValue(2, out stringValue);

                Debug.Log(string.Format("客户端：接收服务器端发送的数据{0},{1} ", intValue, stringValue));

                break;

            case 2:
                Debug.Log(222);
                break;
            default:
                break;
        }

        //把服务器返回的请求分发给对应的子类去处理
        OperatedCode opCode = (OperatedCode)operationResponse.OperationCode;
        Debug.Log("opCode:" + opCode);
        Request request = null;
        bool temp = RequestDict.TryGetValue(opCode, out request);

        if (temp)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.Log("没找到对应的响应处理对象");
        }
    }
    /// <summary>
    /// 添加请求
    /// </summary>
    /// <param name="request"></param>
    public void AddRequest(Request request)
    {
        RequestDict.Add(request.OpCode, request);
    }
    /// <summary>
    /// 移除请求
    /// </summary>
    /// <param name="request"></param>
    public void RemoveRequest(Request request)
    {
        RequestDict.Remove(request.OpCode);
    }
    #endregion
}

