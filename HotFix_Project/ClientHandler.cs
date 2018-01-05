using UnityEngine;
using System;
using System.Net.Sockets;

public class ClientHandler : MonoBehaviour
{
    const int portNo = 500;
    private TcpClient _client;
    byte[] data;

    public string nickName = "";
    public string message = "";
    public string sendMsg = "";

    void OnGUI()
    {
        nickName = GUI.TextField(new Rect(10, 10, 100, 20), nickName);
        message = GUI.TextArea(new Rect(10, 40, 300, 200), message);
        sendMsg = GUI.TextField(new Rect(10, 250, 210, 20), sendMsg);

        if (GUI.Button(new Rect(120, 10, 80, 20), "Connect"))
        {
            //Debug.Log("hello");

            this._client = new TcpClient();
            this._client.Connect("127.0.0.1", portNo);

            data = new byte[this._client.ReceiveBufferSize];

            //SendMessage(txtNick.Text);
            SendMessage(nickName);

            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        };

        if (GUI.Button(new Rect(230, 250, 80, 20), "Send"))
        {
            SendMessage(sendMsg);
            sendMsg = "";
        };
    }

    public void SendMessage(string message)
    {
        try
        {
            NetworkStream ns = this._client.GetStream();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            ns.Write(data, 0, data.Length);
            ns.Flush();
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.ToString());
        }
    }

    public void ReceiveMessage(IAsyncResult ar)
    {
        try
        {
            int bytesRead;

            bytesRead = this._client.GetStream().EndRead(ar);

            if (bytesRead < 1)
            {
                return;
            }
            else
            {

                Debug.Log(System.Text.Encoding.ASCII.GetString(data, 0, bytesRead));

                message += System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
            }

            this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);


        }
        catch (Exception ex)
        {

        }
    }
}
