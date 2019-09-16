using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonRound : MonoBehaviour
{
    GameObject player;
    GameObject elementGo;

    public int skillType;
    Button BtnRound;
    CN.CountDownTimer CD;

    Image RoundSkill;

    Image SkillImage;

    public float InitTime = 3;
    bool flag = true; //初始未冷却

    IEnumerator WaitCircly()
    {
        Debug.Log("WaitCircly");
        yield return new WaitForSeconds(1);
        elementGo.gameObject.SetActive(false);
    }


    public float roundTime = 0.5f;

    IEnumerator Wait(float waitTime)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        lock (p.GetComponent<Player>().thisLock)
        {
            p.GetComponent<Player>().state = (int)State.play;
        }
        yield return new WaitForSeconds(waitTime);
        lock (p.GetComponent<Player>().thisLock)
        {
            if (p.GetComponent<Player>().state == (int)State.play)
                p.GetComponent<Player>().state = (int)State.normal;
        }
        //print("WaitAndPrint " + Time.time);

    }

    public void Start()
    {




        elementGo = Instantiate(Resources.Load("Effect/Prefabs/Hero_skillarea/quan_hero")) as GameObject;

        elementGo.transform.localScale = new Vector3(2, 2, 2);
        skillType = 1;


        CD = new CN.CountDownTimer(InitTime);

        SkillImage = GameObject.Find("Canvas").transform.GetChild(3).GetComponent<Image>();
        SkillImage.raycastTarget = false;

        BtnRound = GameObject.Find("Canvas/RoundImage").transform.GetChild(0).GetComponent<Button>();
        BtnRound.onClick.AddListener(delegate () {
            this.BtnClick();
        });

    }


    public void setRandom()
    {
        System.Random rd = new System.Random();
        skillType = rd.Next(0, 2);

    }
    public void BtnClick()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state != (int)State.normal)
            return;
        Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state);



        if (!flag && !CD.IsTimeUp)
        {
            return;
        }
        else
        {
            StartCoroutine(Wait(roundTime));
            player = GameObject.FindGameObjectWithTag("Player");

            if (skillType == 0)
            {
                //图标也是要变的啊
                NotificationCenter.Instance.PushEvent(NotificationType.Request_Hide, null);

            }
            else if (skillType == 1)
            {
                NotificationCenter.Instance.PushEvent(NotificationType.Request_MeleeDamage, player.transform.position);
            }

            elementGo.transform.position = player.transform.position;
            elementGo.gameObject.SetActive(true);
            CD.Start();
            flag = false;
            StartCoroutine(WaitCircly());

        }


    }
    public void Update()
    {
        if (!flag)
            UpdateImage();
    }
    public void UpdateImage()
    {
        float TimePercent = CD.GetPercent();

        SkillImage.fillAmount = TimePercent;
        if (SkillImage.fillAmount != 0)
        {
            SkillImage.raycastTarget = true;
        }
        else
        {
            SkillImage.raycastTarget = false;
            SkillImage.fillAmount = 1;
        }


    }

}
