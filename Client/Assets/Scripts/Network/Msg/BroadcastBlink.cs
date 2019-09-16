using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastBlink : BaseMsg
{
    public string playerID;
    public float x;
    public float y;
    public float w;

    public BroadcastBlink() : base(MsgID.Broadcast_Blink)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastBlink jsonData = JsonUtility.FromJson<BroadcastBlink>(jsonString);
        this.playerID = jsonData.playerID;
        this.x = jsonData.x;
        this.y = jsonData.y;
        this.w = jsonData.w;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }

}
