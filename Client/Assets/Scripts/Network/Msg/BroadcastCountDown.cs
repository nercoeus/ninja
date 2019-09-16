using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastCountDown : BaseMsg
{
    public string anemyID;

    public BroadcastCountDown() : base(MsgID.Broadcast_CountDown)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastDeath jsonData = JsonUtility.FromJson<BroadcastDeath>(jsonString);
        this.anemyID = jsonData.anemyID;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}
