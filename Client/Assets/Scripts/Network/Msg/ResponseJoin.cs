using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string playerID;
    public string name;
    public float x;
    public float y;
    public int kills;
}

[Serializable]
public class ResponseJoin : BaseMsg
{
    public List<PlayerData> list;
    public PlayerData self;
    public int roomNumber;
    public string name;

    public ResponseJoin() : base(MsgID.Response_Join)
    {
    }

    public override void FromData(byte[] data)
    {
        var jsonString = System.Text.Encoding.Default.GetString(data);

        ResponseJoin jsonData = JsonUtility.FromJson<ResponseJoin>(jsonString);
        this.list = jsonData.list;
        this.self = jsonData.self;
        this.roomNumber = jsonData.roomNumber;
        this.name = jsonData.name;
    }

    public override byte[] ToData()
    {
        return new byte[0];
    }
}

