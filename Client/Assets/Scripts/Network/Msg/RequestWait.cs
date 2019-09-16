using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestWait : BaseMsg
{
    public string playerID;
    public RequestWait() : base(MsgID.Request_Wait)
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
