using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastShengLong : BaseMsg
{
    public string playerID;
    public float x;
    public float y;
    public float w;
    public int skilllevel;

    public BroadcastShengLong() : base(MsgID.Broadcast_ShengLong)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastShengLong jsonData = JsonUtility.FromJson<BroadcastShengLong>(jsonString);
        this.playerID = jsonData.playerID;
        this.x = jsonData.x;
        this.y = jsonData.y;
        this.w = jsonData.w;
        this.skilllevel = jsonData.skilllevel;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }

}
