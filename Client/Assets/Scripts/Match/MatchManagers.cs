using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MatchManagers : MonoBehaviour
{
    public Data data;
    private void Start()
    {

        string oldName = data.name;

        if(oldName != null)
        {
            GameObject.Find("InputField").GetComponent<InputField>().text = oldName;
        }

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            Debug.Log("error");
        }

        Button button = canvas.GetComponentInChildren<Button>();
        if (button == null)
        {
            Debug.Log("error2");
        }

        button.onClick.AddListener( () =>
        {
            InputField ifAccout = GameObject.Find("InputField").GetComponent<InputField>();
            name = ifAccout.text;
            if (name == "")
            {
                Debug.Log("请输入玩家名称");
            }
            else
            {
                data.name = name;
                SceneManager.LoadScene("SampleScene");
            }
        });
    }
    void OnClick()
    {
        
    }
}
