using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestDarts : BaseMsg
{
    public float x;
    public float y;
    public int skilllevel;
    public float w;
    public string playerID;
    //public int skilllevel;

    public RequestDarts() : base(MsgID.Request_Darts)
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
