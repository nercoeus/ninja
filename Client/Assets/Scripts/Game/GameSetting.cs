using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    void initIntegrationPannel(Text[] texts)
    {
        texts[0] = GameObject.Find("Canvas/Panel/nameText").GetComponent<Text>();
        texts[1] = GameObject.Find("Canvas/Panel/integrationText").GetComponent<Text>();
        texts[2] = GameObject.Find("Canvas/Panel/RewardText").GetComponent<Text>();
        texts[3] = GameObject.Find("Canvas/Panel/nameText1").GetComponent<Text>();
        texts[4] = GameObject.Find("Canvas/Panel/nameText2").GetComponent<Text>();
        texts[5] = GameObject.Find("Canvas/Panel/nameText3").GetComponent<Text>();
        texts[6] = GameObject.Find("Canvas/Panel/nameText4").GetComponent<Text>();
        texts[7] = GameObject.Find("Canvas/Panel/integrationText1").GetComponent<Text>();
        texts[8] = GameObject.Find("Canvas/Panel/integrationText2").GetComponent<Text>();
        texts[9] = GameObject.Find("Canvas/Panel/integrationText3").GetComponent<Text>();
        texts[10] = GameObject.Find("Canvas/Panel/integrationText4").GetComponent<Text>();
        texts[11] = GameObject.Find("Canvas/Panel/RewardText1").GetComponent<Text>();
        texts[12] = GameObject.Find("Canvas/Panel/RewardText2").GetComponent<Text>();
        texts[13] = GameObject.Find("Canvas/Panel/RewardText3").GetComponent<Text>();
        texts[14] = GameObject.Find("Canvas/Panel/RewardText4").GetComponent<Text>();
    }
}
