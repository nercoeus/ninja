using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestWinner : BaseMsg
{
    public string playerID;

    public RequestWinner() : base(MsgID.Request_Winner)
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
