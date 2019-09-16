using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestShengLong : BaseMsg
{
    public float x;
    public float y;
    public int skilllevel;
    public float w;
    public string playerID;

    public RequestShengLong() : base(MsgID.Request_ShengLong)
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
