using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastCaoZhui : BaseMsg
{
    public string playerID;
    public float x;
    public float y;
    public float w;
    public int skilllevel;

    public BroadcastCaoZhui() : base(MsgID.Broadcast_CaoZhui)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastCaoZhui jsonData = JsonUtility.FromJson<BroadcastCaoZhui>(jsonString);
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
