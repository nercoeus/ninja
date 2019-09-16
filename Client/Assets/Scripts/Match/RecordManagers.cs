
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RecordManagers : MonoBehaviour
{
    public Animator hands;
    public GameObject ninja;
    public Data data;
    private void Start()
    {
        Debug.Log(data.name);


        ninja = GameObject.FindGameObjectWithTag("Anemy");
        hands = ninja.GetComponent<Animator>();
        hands.Play("Run");
        var child = ninja.transform.GetChild(0).gameObject;
        child.transform.GetChild(0).GetComponent<Text>().text = data.winner;
        child.transform.localEulerAngles = new Vector3(0.0f, -transform.eulerAngles.y, 0.0f);


        GameObject canvas = GameObject.FindGameObjectWithTag("Finish");
        LoadData(canvas);
        if (canvas == null)
        {
            Debug.Log("error");
        }

        Button button1 = canvas.transform.GetChild(1).GetComponent<Button>();
        if (button1 == null)
        {
            Debug.Log("error2");
        }

        Button button2 = canvas.transform.GetChild(2).GetComponent<Button>();
        if (button1 == null)
        {
            Debug.Log("error3");
        }

        button1.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MatchScene");
        });

        button2.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    void OnClick()
    {

    }
    

    void LoadData(GameObject canvas)
    {

        List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>(data.record);
        myList.Sort(delegate (KeyValuePair<string, int> s1, KeyValuePair<string, int> s2) {

            return s2.Value.CompareTo(s1.Value);

        });

        data.record.Clear();

        foreach (KeyValuePair<string, int> pair in myList)
        {

            data.record.Add(pair.Key, pair.Value);

        }
        Debug.Log(data.record.Count);
        int i = 0;
        foreach(string key in data.record.Keys)
        {
            string str = key.Substring(2);
            canvas.transform.GetChild(9 + i).GetComponent<Text>().text = str;
            canvas.transform.GetChild(15 + i).GetComponent<Text>().text = data.record[key].ToString();
            i++;
        }

        data.record.Clear();
    }

}
