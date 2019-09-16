using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCaoZhui : BaseMsg
{
    public float x;
    public float y;
    public float w;
    public int skilllevel;
    public string playerID;

    public RequestCaoZhui() : base(MsgID.Request_CaoZhui)
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
