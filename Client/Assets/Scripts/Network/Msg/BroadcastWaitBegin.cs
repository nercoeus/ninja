using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastWaitBegin : BaseMsg
{
    public string playerID;
    public string type;

    public BroadcastWaitBegin() : base(MsgID.Broadcast_WaitBegin)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastDeath jsonData = JsonUtility.FromJson<BroadcastDeath>(jsonString);
        this.playerID = jsonData.playerID;
        this.type = jsonData.type;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }

}
