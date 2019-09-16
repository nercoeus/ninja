using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BroadcastWinner : BaseMsg
{

    public List<PlayerData> list;

    public string enemyID;

    public BroadcastWinner() : base(MsgID.Broadcast_Winner)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastWinner jsonData = JsonUtility.FromJson<BroadcastWinner>(jsonString);
        this.enemyID = jsonData.enemyID;
        this.list = jsonData.list;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}
