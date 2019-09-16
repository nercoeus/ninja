using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastResurgence : BaseMsg
{
    public string playerID;
    public float x;
    public float y;

    public BroadcastResurgence() : base(MsgID.Broadcast_Resurgence)
    {

    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastMeleeDamage jsonData = JsonUtility.FromJson<BroadcastMeleeDamage>(jsonString);
        this.playerID = jsonData.playerID;
        this.x = jsonData.x;
        this.y = jsonData.y;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}

