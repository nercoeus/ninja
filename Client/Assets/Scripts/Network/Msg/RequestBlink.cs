using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestBlink : BaseMsg
{
    public float x;
    public float y;
    public float w;
    public string playerID;

    public RequestBlink() : base(MsgID.Request_Blink)
    {
    }

    public override byte[] ToData()
    {
        var str = JsonUtility.ToJson(this);
        return System.Text.Encoding.ASCII.GetBytes(str);
    }

    public override void FromData(byte[] data)
    {
    }
}
