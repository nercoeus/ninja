using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance = null;

    public string host = "127.0.0.1";
    public int port = 18787;

    public Queue<Message> m_msgQueue = new Queue<Message>();
    // for queue
    private object thisLock = new object();

    private GameSocket m_gameSocket;

    //判断是否处于连接状态
    private bool isConnect = true;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
        m_gameSocket = new GameSocket(OnConnected, OnDisconnect, OnMessage);
    }

    void Update()
    {
        lock (thisLock)
        {
            if (m_msgQueue.Count > 0)
            {
                Message msg = m_msgQueue.Dequeue();
                HandleMessage(msg);
            }
        }
    }

    private void HandleMessage(Message msg)
    {
        var msgID = (MsgID)msg.MessageID;
        if (isConnect)
        {
            switch (msgID)
            {
                case MsgID.Response_Join:
                    {
                        ResponseJoin data = new ResponseJoin();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseJoin, data);
                        break;
                    }


                case MsgID.Broadcast_Move:
                    {
                        BroadcastMove data = new BroadcastMove();
                        data.FromMessage(msg);
                        //print("move: " + data.x + " " + data.y + " " + data.w);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastMove, data);
                        break;
                    }

                case MsgID.Broadcast_Join:
                    {
                        BroadcastJoin data = new BroadcastJoin();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastJoin, data);
                        break;
                    }

                case MsgID.Broadcast_Leave:
                    {
                        BroadcastLeave data = new BroadcastLeave();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastLeave, data);
                        break;
                    }
                case MsgID.Broadcast_Hide:
                    {
                        BroadcastDodgeHide data = new BroadcastDodgeHide();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastHide, data);
                        //Debug.Log("Broadcast_Hide  revice msg from service" + data.playerID);
                        break;
                    }
                case MsgID.Broadcast_MeleeDamege:
                    {
                        BroadcastMeleeDamage data = new BroadcastMeleeDamage();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastMeleeDamage, data);
                        //Debug.Log("Broadcast_MeleeDamege  revice msg from service" + data.playerID);
                        break;
                    }
                case MsgID.Broadcast_Darts:
                    {
                        BroadcastDarts data = new BroadcastDarts();
                        data.FromMessage(msg);
                        Debug.Log("飞镖层级是 " + data.skilllevel);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastDarts, data);

                        break;
                    }
                case MsgID.Broadcast_Death:
                    {
                        BroadcastDeath data = new BroadcastDeath();
                        data.FromMessage(msg);
                        //Debug.Log(data.anemyID + " " + data.playerID + " " + data.type);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastDeath, data);
                        break;
                    }
                case MsgID.Broadcast_Blink:
                    {
                        BroadcastBlink data = new BroadcastBlink();
                        data.FromMessage(msg);
                        //print("blink: " + data.x + " " + data.y + " " + data.w);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastBlink, data);
                        break;
                    }
                case MsgID.Broadcast_Wait:
                    {
                        BroadcastWait data = new BroadcastWait();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastWait, data);
                        //Debug.Log("Broadcast_Wait revice msg from service:" + data.playerID);
                        break;
                    }
                case MsgID.Broadcast_Resurgence:
                    {
                        BroadcastResurgence data = new BroadcastResurgence();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastResurgence, data);
                        break;

                    }
                case MsgID.Broadcast_ShengLong:
                    {
                        BroadcastShengLong data = new BroadcastShengLong();
                        data.FromMessage(msg);
                        Debug.Log("ShengLong" + data.x + " " + data.y + " " + data.w);
                        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnBroadcastShengLong, data);

                        break;
                    }
                case MsgID.Broadcast_CaoZhui:
                    {
                        BroadcastCaoZhui data = new BroadcastCaoZhui();
                        data.FromMessage(msg);
                        Debug.Log("CaoZhui" + data.x + " " + data.y + " " + data.w);
                        NotificationCenter.Instance.PushEvent(NotificationType.Networt_OnBroadcastCaoZhui, data);

                        break;
                    }
                case MsgID.Broadcast_CountDown:
                    {
                        BroadcastCountDown data = new BroadcastCountDown();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Networt_OnBroadcastCountDown, data);

                        break;
                    }
                case MsgID.Broadcast_Winner:
                    {
                        BroadcastWinner data = new BroadcastWinner();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Networt_OnBroadcastWinner, data);

                        break;
                    }
                case MsgID.Broadcast_WaitBegin:
                    {
                        BroadcastWaitBegin data = new BroadcastWaitBegin();
                        data.FromMessage(msg);
                        NotificationCenter.Instance.PushEvent(NotificationType.Networt_OnBroadcastWaitBegin, data);

                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Connect to server.
    /// </summary>
    public void Connect()
    {
        isConnect = true;
        TimeSpan timeout = new TimeSpan(0, 0, 5);
        m_gameSocket.Connect(host, port, timeout);
    }

    /// <summary>
    /// Sends message.
    /// </summary>
    /// <param name="msg">Message.</param>
    public void SendMessage(Message msg)
    {
        if (m_gameSocket != null)
        {
            m_gameSocket.PushMessage(msg);
        }
    }

    public void SendJoin(string playName)
    {
        var msg = new RequestJoin();
        Vector2 pos = new Vector2(1.0f, 1.0f);
        pos = Player.getRandomPosition(99);
        msg.x = pos.x;
        msg.y = pos.y;
        msg.name = playName;
        SendMessage(msg.ToMessage());
    }

    public void SendWinner(string ID)
    {
        var msg = new RequestWinner();
        msg.playerID = ID;
        SendMessage(msg.ToMessage());
    }

    public void SendDarts(float x, float y, float w, string playerID, int level)
    {
        var msg = new RequestDarts();
        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;
        msg.w = w;
        msg.skilllevel = level;

        SendMessage(msg.ToMessage());
    }

    public void SendDeath(string enamy, string player, string type)
    {
        var msg = new RequestDeath();
        msg.anemyID = enamy;
        msg.playerID = player;
        msg.deathType = type;

        SendMessage(msg.ToMessage());
    }

    public void SendMove(float x, float y, float w, string playerID)
    {
        var msg = new RequestMove();
        msg.playerID = Convert.ToInt32(playerID);
        msg.x = x;
        msg.y = y;
        msg.w = w;

        SendMessage(msg.ToMessage());
    }

    public void SendResurgence(float x, float y, string playerID)
    {
        var msg = new RequesResurgence();
        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;

        SendMessage(msg.ToMessage());
    }
    public void SendMeleeDamage(float x, float y, string playerID)
    {
        Debug.Log("sendMeleeDamage");
        var msg = new RequestMeleeDamage();
        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;

        SendMessage(msg.ToMessage());
    }
    public void SendID(string playerID)
    {
        var msg = new RequestDodgeHide();
        msg.playerID = playerID;
        SendMessage(msg.ToMessage());
        Debug.Log("SendID" + msg.playerID);
    }

    public void SendWait(string playerID)
    {
        var msg = new RequestWait();
        msg.playerID = playerID;
        SendMessage(msg.ToMessage());
    }

    public void SendBlink(float x, float y, float w, string playerID)
    {
        var msg = new RequestBlink();
        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;
        msg.w = w;

        SendMessage(msg.ToMessage());
    }
    public void SendShengLong(float x, float y, float w, string playerID, int level)
    {
        var msg = new RequestShengLong();

        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;
        msg.w = w;
        msg.skilllevel = level;

        SendMessage(msg.ToMessage());
    }
    public void SendCaoZhui(float x, float y, float w, string playerID, int level)
    {
        var msg = new RequestCaoZhui();

        msg.playerID = playerID;
        msg.x = x;
        msg.y = y;
        msg.w = w;
        msg.skilllevel = level;

        SendMessage(msg.ToMessage());
    }
    public void OnConnected(Message msg)
    {
        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnConnected, null);
    }

    public void OnDisconnect(Message msg)
    {
        isConnect = false;
        m_gameSocket.Reset();
        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnDisconnected, null);
    }

    public void OnMessage(Message msg)
    {
        lock (thisLock)
        {
            m_msgQueue.Enqueue(msg);
        }
    }

    public void Destory()
    {
        m_gameSocket.Disconnect();
    }

    private void OnDestroy()
    {
        m_gameSocket.Reset();
    }
}
