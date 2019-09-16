using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastDodgeHide : BaseMsg
{
    public string playerID;

    public BroadcastDodgeHide() : base(MsgID.Broadcast_Hide)
    {

    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        BroadcastDodgeHide jsonData = JsonUtility.FromJson<BroadcastDodgeHide>(jsonString);
        this.playerID = jsonData.playerID;
        Debug.Log("class ResponseDodgeHide playerID is " + this.playerID);

    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}
