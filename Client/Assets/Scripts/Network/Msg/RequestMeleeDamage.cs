using UnityEngine;

[System.Serializable]

public class RequestMeleeDamage : BaseMsg
{
    public string playerID;
    public float x;
    public float y;

    public RequestMeleeDamage() : base(MsgID.Request_MeleeDamage)
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

