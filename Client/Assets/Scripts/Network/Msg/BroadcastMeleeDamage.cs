using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastMeleeDamage : BaseMsg
{
    public string playerID;
    public float x;
    public float y;

    public BroadcastMeleeDamage() : base(MsgID.Broadcast_MeleeDamege)
    {

    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastMeleeDamage jsonData = JsonUtility.FromJson<BroadcastMeleeDamage>(jsonString);
        this.playerID = jsonData.playerID;
        this.x = jsonData.x;
        this.y = jsonData.y;
        Debug.Log("class BroadcastMeleeDamage playerID is " + this.playerID + " x is " + this.x + "y is " + this.y);

    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}


