using UnityEngine;

[System.Serializable]

public class RequesResurgence : BaseMsg
{
    public string playerID;
    public float x;
    public float y;

    public RequesResurgence() : base(MsgID.Request_Resurgence)
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