using UnityEngine;

[System.Serializable]
public class RequestMove : BaseMsg
{
    public float x;
    public float y;
    public float w;
    public int playerID;

    public RequestMove() : base(MsgID.Request_Move)
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
