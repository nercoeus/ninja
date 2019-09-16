using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastDeath : BaseMsg
{

    public string anemyID;
    public string playerID;
    public string type;
    public int numbers;

    public BroadcastDeath() : base(MsgID.Broadcast_Death)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastDeath jsonData = JsonUtility.FromJson<BroadcastDeath>(jsonString);
        this.anemyID = jsonData.anemyID;
        this.playerID = jsonData.playerID;
        this.type = jsonData.type;
        this.numbers = jsonData.numbers;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }

}
