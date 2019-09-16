using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class Data : ScriptableObject
{
    public Dictionary<string, int> record = new Dictionary<string, int>();
    public string name;
    public string winner;
}