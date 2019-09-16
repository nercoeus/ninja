using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastWait : BaseMsg
{
    public string playerID;

    public BroadcastWait() : base(MsgID.Broadcast_Wait)
    {

    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastWait jsonData = JsonUtility.FromJson<BroadcastWait>(jsonString);
        this.playerID = jsonData.playerID;

    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}
