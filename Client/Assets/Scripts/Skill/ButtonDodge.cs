using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonDodge : MonoBehaviour
{
    CN.CountDownTimer CD;
    Image SkillImage;
    Button BtnDodge;

    public float InitTime = 3;
    bool flag = true; //初始未冷却

    public float dodgeTime = 0.5f;

    public void Start()
    {
        CD = new CN.CountDownTimer(InitTime);

        SkillImage = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<Image>();
        SkillImage.raycastTarget = false;
        BtnDodge = GameObject.Find("Canvas/DodgeImage").transform.GetChild(0).GetComponent<Button>();
        BtnDodge.onClick.AddListener(delegate () {
            this.BtnClick();
        });
        //if (SkillImage)
        //    Debug.Log("11111111111");


    }

    IEnumerator Wait(float waitTime)
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        int temp = p.GetComponent<Player>().state;
        lock (p.GetComponent<Player>().thisLock)
        {
            p.GetComponent<Player>().state = (int)State.play;
        }
        yield return new WaitForSeconds(waitTime);
        lock (p.GetComponent<Player>().thisLock)
        {
            p.GetComponent<Player>().state = temp;
        }
        //print("WaitAndPrint " + Time.time);

    }
    public void BtnClick()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state != (int)State.normal)
            return;

        if (!flag && !CD.IsTimeUp)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NotificationCenter.Instance.PushEvent(NotificationType.Request_Hide, null);
        CD.Start();
        flag = false;
        //StartCoroutine(Wait(dodgeTime));
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