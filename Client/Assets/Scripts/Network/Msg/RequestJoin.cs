using UnityEngine;

[System.Serializable]
public class RequestJoin : BaseMsg
{

    public string name;
    public float x;
    public float y;

    public RequestJoin() : base(MsgID.Request_Join)
    {

    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        RequestJoin jsonData = JsonUtility.FromJson<RequestJoin>(jsonString);
        this.name = jsonData.name;

    }

    public override byte[] ToData()
    {
        var str = JsonUtility.ToJson(this);
        return System.Text.Encoding.ASCII.GetBytes(str);
    }
}
