using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.IO;

//实时消息处理委托
public delegate void NetEventHandler (string msg);

public class NetUtility
{
	//单例
	public static readonly NetUtility Instance = new NetUtility ();
	//消息回调
	private NetEventHandler ReceiveCallback;
	//服务器Tcp
	private TcpListener tcpServer;
	//客户端Tcp
	private TcpClient tcpClient;
	//缓冲区
	private byte[] buffer;
	//缓存数据组
	private List<byte> cache;
	//网络节点
	private IPEndPoint serverIPEndPoint;

	/// <summary>
	/// 设置网络节点
	/// </summary>
	/// <param name="ep">网络节点.</param>
	public void SetIpAddressAndPort (IPEndPoint ep)
	{
		//只写网络节点
		serverIPEndPoint = ep;
	}
	/// <summary>
	/// 设置委托
	/// </summary>
	/// <param name="handler">消息委托.</param>
	public void SetDelegate (NetEventHandler handler)
	{
		//只写赋值回调
		ReceiveCallback = handler;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="NetUtility"/> class.
	/// </summary>
	private NetUtility ()
	{
		//服务器实例
		tcpServer = new TcpListener (IPAddress.Any, 23456);
		//客户端实例
		tcpClient = new TcpClient (AddressFamily.InterNetwork);
		//缓冲区初始化
		buffer = new byte[1024];
		//缓存数据组实例
		cache = new List<byte> ();
		//默认网络节点
		serverIPEndPoint = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 3000);
	}
	#region Server Part:
	/// <summary>
	/// 开启服务器
	/// </summary>
	public void ServerStart ()
	{
		//开启服务器
		tcpServer.Start (10);
		//服务器开启提示
		ReceiveCallback ("Server Has Init!");
		//开始异步接受客户端的连接请求
		tcpServer.BeginAcceptTcpClient (AsyncAccept, null);
	}
	/// <summary>
	/// 异步连接回调
	/// </summary>
	/// <param name="ar">Ar.</param>
	void AsyncAccept (System.IAsyncResult ar)
	{
		//接受到客户端的异步连接请求
		tcpClient = tcpServer.EndAcceptTcpClient (ar);
		//有新的客户端连接提示
		ReceiveCallback ("Accept Client :" + tcpClient.Client.RemoteEndPoint.ToString ());
		//异步接收消息
		tcpClient.Client.BeginReceive (buffer, 0, 1024, SocketFlags.None, AsyncReceive, tcpClient.Client);
		//异步接受客户端请求尾递归
		tcpServer.BeginAcceptTcpClient (AsyncAccept, null);
	}
	/// <summary>
	/// 异步接收消息回调
	/// </summary>
	/// <param name="ar">Ar.</param>
	void AsyncReceive (System.IAsyncResult ar)
	{
		//获取消息套接字
		Socket workingClient = ar.AsyncState as Socket;
		//完成接收
		int msgLength = workingClient.EndReceive (ar);
		//如果接收到了数据
		if (msgLength > 0) {
			//消息接收提示
			ReceiveCallback ("ReceiveData : " + msgLength + "bytes");
			//临时缓冲区
			byte[] tempBuffer = new byte[msgLength];
			//拷贝数据到临时缓冲区
			Buffer.BlockCopy (buffer, 0, tempBuffer, 0, msgLength);
			//数据放到缓存数据组队尾
			cache.AddRange (tempBuffer); 
			//拆包解析
			byte[] result = LengthDecode (ref cache);
			//如果已经接收完全部数据
			if (result != null) {
				//开始反序列化数据
				NetModel resultModel = DeSerialize (result);
				//TODO:Object Processing!
				//数据对象结果提示
				ReceiveCallback ("Object Result IP : " + resultModel.senderIp);
				ReceiveCallback ("Object Result Content : " + resultModel.content);
				ReceiveCallback ("Object Result Time : " + resultModel.time);
			}
			//消息未接收全，继续接收
			tcpClient.Client.BeginReceive (buffer, 0, 1024, SocketFlags.None, AsyncReceive, tcpClient.Client);
		}

	}
	#endregion

	#region Client Part
	/// <summary>
	/// 客户端连接
	/// </summary>
	public void ClientConnnect ()
	{
		//连接到服务器
		tcpClient.Connect (serverIPEndPoint);
		//连接到服务器提示
		ReceiveCallback ("Has Connect To Server : " + serverIPEndPoint.Address.ToString ());
	}
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="model">Model.</param>
	public void SendMsg (NetModel model)
	{
		//将数据对象序列化
		buffer = Serialize (model);
		//将序列化后的数据加字节头
		buffer = LengthEncode (buffer);
		//拆分数据，多次发送
		for (int i = 0; i < buffer.Length/1024 + 1; i++) {
			//满发送，1KB
			int needSendBytes = 1024;
			//最后一次发送，剩余字节
			if (i == buffer.Length / 1024) {
				//计算剩余字节
				needSendBytes = buffer.Length - i * 1024;
			}
			//发送本次数据
			tcpClient.GetStream ().Write (buffer, i * 1024, needSendBytes);
		}
	}
	#endregion

	#region Public Function
	/// <summary>
	/// 数据加字节头操作
	/// </summary>
	/// <returns>数据结果.</returns>
	/// <param name="data">源数据.</param>
	byte[] LengthEncode (byte[] data)
	{
		//内存流实例
		using (MemoryStream ms = new MemoryStream()) {
			//二进制流写操作实例
			using (BinaryWriter bw = new BinaryWriter(ms)) {
				//先写入字节长度
				bw.Write (data.Length);
				//再写入所有数据
				bw.Write (data);
				//临时结果
				byte[] result = new byte[ms.Length];
				//将写好的流数据放入临时结果
				Buffer.BlockCopy (ms.GetBuffer (), 0, result, 0, (int)ms.Length);
				//返回临时结果
				return result;
			}
		}
	}
	/// <summary>
	/// 数据解析，拆解字节头，获取数据.
	/// </summary>
	/// <returns>源数据.</returns>
	/// <param name="cache">缓存数据.</param>
	byte[] LengthDecode (ref List<byte> cache)
	{
		//如果字节数小于4，出现异常
		if (cache.Count < 4)
			return null;
		//内存流实例
		using (MemoryStream ms = new MemoryStream(cache.ToArray())) {
			//二进制流读操作实例
			using (BinaryReader br = new BinaryReader(ms)) {
				//先读取数据长度，一个int值
				int realMsgLength = br.ReadInt32 ();
				//如果未接收全数据，下次继续接收
				if (realMsgLength > ms.Length - ms.Position) {
					return null;
				}
				//接收完，读取所有数据
				byte[] result = br.ReadBytes (realMsgLength);
				//清空缓存
				cache.Clear ();
				//返回结果
				return result;
			}
		}
	}
	/// <summary>
	/// 序列化数据.
	/// </summary>
	/// <param name="mod">数据对象.</param>
	private byte[] Serialize (NetModel mod)
	{
		try {
			//内存流实例
			using (MemoryStream ms = new MemoryStream()) {
				//ProtoBuf协议序列化数据对象
				ProtoBuf.Serializer.Serialize<NetModel> (ms, mod);
				//创建临时结果数组
				byte[] result = new byte[ms.Length];
				//调整游标位置为0
				ms.Position = 0;
				//开始读取，从0到尾
				ms.Read (result, 0, result.Length);
				//返回结果
				return result;
			}
		} catch (Exception ex) {
			
			Debug.Log ("Error:" + ex.ToString ());
			return null;
		}
	}
	
	/// <summary>
	/// 反序列化数据.
	/// </summary>
	/// <returns>数据对象.</returns>
	/// <param name="data">源数据.</param>
	private NetModel DeSerialize (byte[] data)
	{
		try {
			//内存流实例
			using (MemoryStream ms = new MemoryStream(data)) {
				//调整游标位置
				ms.Position = 0;
				//ProtoBuf协议反序列化数据
				NetModel mod = ProtoBuf.Serializer.Deserialize<NetModel> (ms);
				//返回数据对象
				return mod;
				
			}
		} catch (Exception ex) {
			Debug.Log ("Error: " + ex.ToString ());
			return null;
		}
	}
	#endregion

}
