using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestDeath : BaseMsg
{
    public string anemyID;
    public string playerID;
    public string deathType;

    public RequestDeath() : base(MsgID.Request_Death)
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
