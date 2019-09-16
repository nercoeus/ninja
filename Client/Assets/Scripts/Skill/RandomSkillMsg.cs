using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkillMsg : MonoBehaviour
{

    //技能升二、三级的条件分别是杀了2个人，4个人
    public int upGradeTwo;
    public int upGradeThere;

    //当前随机技能层级
    public int m_randomSkillLevel { get; set; }
    //当前随机技能类型
    public SkillType m_randomSkillType { get; set; }
    public int m_randomSkillKillCount { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        upGradeTwo = 1;
        upGradeThere = 2;
    }

   


    private IEnumerator textNotification(Text updateText)
    {
        Debug.Log("now we need printf");
        updateText.text = "升 级 ！";
        yield return new WaitForSeconds(2);
        updateText.text = "";
        Debug.Log("now we ");
    }

  

    public void skillUpgrade(string ememyID, string myID, string killSkillType, int state, Text text)
    {
        if (state != (int)State.death)
        {
            if (ememyID == myID)
            {
                if (killSkillType == m_randomSkillType.ToString())
                {
                    m_randomSkillKillCount++;
                    if (m_randomSkillKillCount >= upGradeTwo && m_randomSkillKillCount < upGradeThere && m_randomSkillLevel == 1)
                    {
                        m_randomSkillLevel = 2;
                        Debug.Log("技能升级2！");
                        StopCoroutine("textNotification");
                        StartCoroutine("textNotification", text);
                    }
                    else if (m_randomSkillKillCount > upGradeThere && m_randomSkillLevel == 2)
                    {
                        m_randomSkillLevel = 3;
                        Debug.Log("技能升级3！");
                        StopCoroutine("textNotification");
                        StartCoroutine("textNotification", text);
                    }
                }
                else
                {
                    Debug.Log("杀人方式 " + killSkillType + "我的杀人方式 " + m_randomSkillType.ToString());
                    Debug.Log("不是我用飞镖杀的 ");
                }
            }
            else
            {
                Debug.Log("不是我杀的 ");
            }
        }
    }
       

    public int returnSkillLevel(SkillType i)
    {
        int j = System.Convert.ToInt32(i);
        if(i == m_randomSkillType)
        {
            return m_randomSkillLevel;
        }
        else
        {
            return 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
