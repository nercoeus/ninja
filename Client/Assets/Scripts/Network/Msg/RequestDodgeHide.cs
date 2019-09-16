
using UnityEngine;

[System.Serializable]

public class RequestDodgeHide : BaseMsg
{
    public string playerID;
    public RequestDodgeHide() : base(MsgID.Request_Hide)
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
