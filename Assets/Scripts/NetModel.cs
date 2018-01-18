using UnityEngine;
using System.Collections;
using ProtoBuf;

[ProtoContract]
//协议数据类
public class NetModel : MonoBehaviour
{
	//数据体定义
	[ProtoMember(1)]
	public string 
		senderIp;

	[ProtoMember(2)]
	public string
		content;

	[ProtoMember(3)]
	public string
		time;

}