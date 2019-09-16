using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private Dictionary<string, int> plane = new Dictionary<string, int>();           //保存id和所属的text字段
    private Dictionary<string, int> integrationRecord = new Dictionary<string, int>();
    private Dictionary<string, int> rewardRecord = new Dictionary<string, int>();


    public GameObject playerPrefab;
    public GameObject ninjaPrefab;
    public int resurgenceTime;
    public Text messageAborcast;
    public Text updateText;
    public Text stateAborcast;
    public GameObject dartButton;
    public GameObject shenglongButton;
    public GameObject unkwonButton;
   
    public Data data2;

    //public Animator hands;
    private GameObject hero;

    public Text[] texts;
   

    //private int frameCount = 0;
    //private bool downQ = false;

    public int BeginCount;

    private int countJoin;  //为了计算加入了多少个人
    public int nowPlayers;

    void Start()
    {
        RegisterEvents();
        NetworkManager.Instance.Connect();
        resurgenceTime = 4;
        nowPlayers = 0;
        //hands = hero.GetComponent<Animator>();
        countJoin = 6;
    }

    private void Update()
    {
        if(hero!=null)
            stateAborcast.text ="state: " + hero.GetComponent<Player>().state.ToString();
    }
    void RegisterEvents()
    {
        NotificationCenter nc = NotificationCenter.Instance;

        nc.AddEventListener(NotificationType.Network_OnResponseJoin, OnResponseJoin);
        nc.AddEventListener(NotificationType.Network_OnBroadcastMove, OnBroadcastMove);
        nc.AddEventListener(NotificationType.Network_OnBroadcastJoin, OnBroadcastJoin);
        nc.AddEventListener(NotificationType.Network_OnBroadcastLeave, OnBroadcastLeave);
        nc.AddEventListener(NotificationType.Network_OnConnected, OnConnected);
        nc.AddEventListener(NotificationType.Network_OnDisconnected, OnDisconnected);
        nc.AddEventListener(NotificationType.Operate_MapPosition, OnTouchMap);
        nc.AddEventListener(NotificationType.Request_Hide,OnRequestDodgeHide);
        nc.AddEventListener(NotificationType.Network_OnBroadcastHide, OnBroadcastHide);

        nc.AddEventListener(NotificationType.Request_Darts,OnRequestSendDarts);
        nc.AddEventListener(NotificationType.Network_OnBroadcastDarts, OnBroadcastDarts);

        nc.AddEventListener(NotificationType.Request_MeleeDamage, OnRequestMeleeDamage);
        nc.AddEventListener(NotificationType.Network_OnBroadcastMeleeDamage, OnBroadcastMeleeDamage);
        nc.AddEventListener(NotificationType.Request_Blink, OnRequestBlink);
        nc.AddEventListener(NotificationType.Network_OnBroadcastBlink, OnBroadcastBlink);
        nc.AddEventListener(NotificationType.Request_Wait, OnRequestWait);
        nc.AddEventListener(NotificationType.Network_OnBroadcastWait, OnBroadcastWait);

        nc.AddEventListener(NotificationType.Network_OnResponseDeath, OnResponseDeath);
        nc.AddEventListener(NotificationType.Network_OnBroadcastDeath, OnBroadcastDeath);

        nc.AddEventListener(NotificationType.Network_OnRequestResurgence, OnRequestResurgence);
        nc.AddEventListener(NotificationType.Network_OnBroadcastResurgence, OnBroadcastResurgence);


        nc.AddEventListener(NotificationType.Request_ShengLong, OnRequestShengLong);
        nc.AddEventListener(NotificationType.Network_OnBroadcastShengLong, OnBroadcastShengLong);

        nc.AddEventListener(NotificationType.Request_CaoZhui, OnRequestCaoZhui);
        nc.AddEventListener(NotificationType.Networt_OnBroadcastCaoZhui, OnBroadcastCaoZhui);


        nc.AddEventListener(NotificationType.Networt_OnBroadcastCountDown, CountDown);
        nc.AddEventListener(NotificationType.Networt_OnRequestWinner, RequestEndGame);
        nc.AddEventListener(NotificationType.Networt_OnBroadcastWinner, EndGame);

        nc.AddEventListener(NotificationType.Networt_OnBroadcastWaitBegin, WaitGameBegin);

    }

    void WaitGameBegin(NotificationArg arg)
    {
        BroadcastWaitBegin data = arg.GetValue<BroadcastWaitBegin>();
        if(data.type == "OK")
        {
            Debug.Log(data.type + "name : " + data.playerID);
            hero.GetComponent<Player>().BeginGame();
            StartCoroutine("Begin");
        }
        else
        {
            hero.GetComponent<Player>().BeginGame();
            Debug.Log(data.type+ "name : "+ data.playerID);
            messageAborcast.text = "玩家: " + data.playerID +" 加入游戏！";
        }
    }

    IEnumerator Begin()
    {
        BeginCount = 3;
        while (BeginCount >= -1)
        {

            messageAborcast.text = BeginCount.ToString();
            if (BeginCount == -1)
            {
                messageAborcast.text = null;
                hero.GetComponent<Player>().BeginGame();
            }
            yield return new WaitForSeconds(1);
            BeginCount--;
        }

    }

    void CountDown(NotificationArg arg)
    {
        BroadcastCountDown data = arg.GetValue<BroadcastCountDown>();
        if(data.anemyID == hero.GetComponent<Player>().ID)
            hero.GetComponent<Player>().BCountDown();
        else
        {
            if (players.ContainsKey(data.anemyID))
            {
                //获胜英雄开始倒计时
                
                var p = players[data.anemyID];

                p.GetComponent<Anemy>().BCountDown();
                EnemyLookAt.Instance.AppearById(data.anemyID);
            }
        }
    }

    void RequestEndGame(NotificationArg arg)
    {
        NetworkManager.Instance.SendWinner(hero.GetComponent<Player>().ID);
    }

    void EndGame(NotificationArg arg)
    {
        BroadcastWinner data = arg.GetValue<BroadcastWinner>();
        Debug.Log(data.enemyID);
        Debug.Log(data.list);
        data2.winner = data.enemyID;
        data2.record.Clear();
        Debug.Log("end game1" + data.list.Count);
        for (int i = 0; i<data.list.Count; i++)
        {
            data2.record.Add(i.ToString() + "_" + data.list[i].name, data.list[i].kills);
        }
        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnDisconnected, null);
        SceneManager.LoadScene("RecordScene");
    }


    void OnRequestMeleeDamage(NotificationArg arg)
    {
        Vector3 targetPos = arg.GetValue<Vector3>();
        NetworkManager.Instance.SendMeleeDamage(targetPos.x, targetPos.z, hero.GetComponent<Player>().ID);
        hero.GetComponent<Player>().MeleeDamage();   
    }

    void OnBroadcastMeleeDamage(NotificationArg arg)
    {    
        BroadcastMeleeDamage data = arg.GetValue<BroadcastMeleeDamage>();
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            Vector3 myPosition = hero.GetComponent<Player>().getPlayerPosition();
            
            bool isDie = p.GetComponent<Anemy>().MeleeDamage(data, myPosition.x, myPosition.z, hero.GetComponent<Player>().ID, hero.GetComponent<Player>().state);
        }

    }

    void ResurgenceRandomSkills()
    {
        System.Random rd = new System.Random();
        int i = rd.Next(0, 3);
        i = 1;
        if (i == 0)
        {
            dartButton.SetActive(true);
            shenglongButton.SetActive(false);
            unkwonButton.SetActive(false);
            hero.GetComponent<RandomSkillMsg>().m_randomSkillType = SkillType.DartAttack;
            Image datrImg = GameObject.Find("Canvas/SkillButtons/DartButton/Image").GetComponent<Image>();
            string dartImgUrl = "Images/dart_1";
            datrImg.sprite = Resources.Load(dartImgUrl, typeof(Sprite)) as Sprite;
            Debug.Log("dart true");
        }
        else if (i == 1)
        {
            dartButton.SetActive(false);
            shenglongButton.SetActive(true);
            unkwonButton.SetActive(false);
            hero.GetComponent<RandomSkillMsg>().m_randomSkillType = SkillType.ShenglongAttack;
            Debug.Log("shenglong true");
        }
        else if (i == 2)
        {
            dartButton.SetActive(false);
            shenglongButton.SetActive(false);
            unkwonButton.SetActive(true);
            hero.GetComponent<RandomSkillMsg>().m_randomSkillType = SkillType.CaoZhuiAttack;
            Image datrImg = GameObject.Find("Canvas/SkillButtons/caozhuiButton/Image").GetComponent<Image>();
            string dartImgUrl = "Images/sword_1";
            datrImg.sprite = Resources.Load(dartImgUrl, typeof(Sprite)) as Sprite;
            Debug.Log("unkwon true");
        }
        else
        {
            Debug.Log("random skill error");
        }
        hero.GetComponent<RandomSkillMsg>().m_randomSkillLevel = 1;
        hero.GetComponent<RandomSkillMsg>().m_randomSkillKillCount = 0;
    }
    //发送复活请求
    void OnRequestResurgence(NotificationArg arg)
    {
        float[] arr = arg.GetValue<float[]>();

        hero.SetActive(true);
        hero.GetComponent<Player>().Resurgence(arr[0], arr[1]);                    //处理本地复活

        NetworkManager.Instance.SendResurgence(arr[0], arr[1], hero.GetComponent<Player>().ID);
      
        ResurgenceRandomSkills();
    }
    void OnBroadcastResurgence(NotificationArg arg)
    {
        BroadcastResurgence data = arg.GetValue<BroadcastResurgence>();
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            p.GetComponent<Anemy>().Resurgence(data.x, data.y);
        }
    }

    //发送闪避请求
    void OnRequestDodgeHide(NotificationArg arg)
    {
        NetworkManager.Instance.SendID(hero.GetComponent<Player>().ID);
        hero.GetComponent<Player>().DodgeTo();
    }

    //处理其他客户端的闪避表现
    //使用闪避的时候广播人物隐藏，无法被攻击
    void OnBroadcastHide(NotificationArg arg)
    {

        BroadcastDodgeHide data = arg.GetValue<BroadcastDodgeHide>();
        //Debug.Log("OnBroadcastHide player'id  is" + data.playerID);
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            // Debug.Log("OnBroadcastHide player'id  is" + data.playerID);
            p.GetComponent<Anemy>().DodgeTo();     
        }
    }

    //处理飞镖发射的逻辑
    void OnRequestSendDarts(NotificationArg arg)
    {
        Debug.Log("处理发射飞镖");
        ReqDartMsg msg = arg.GetValue<ReqDartMsg>();
        Vector3 targetPos = msg.position;
        int level = msg.level;
        hero.GetComponent<Player>().SendDarts(targetPos, level);
       
        NetworkManager.Instance.SendDarts(hero.transform.position.x, hero.transform.position.z, hero.GetComponent<Player>().angle_360(targetPos), hero.GetComponent<Player>().ID, level);
        
    }

    void OnBroadcastDarts(NotificationArg arg)
    {
        BroadcastDarts data = arg.GetValue<BroadcastDarts>();
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            p.GetComponent<Anemy>().SendDarts(data.x, data.y, data.w, data.skilllevel);
        }
        Debug.Log("dart level is " + data.skilllevel);
    }

    //请求死亡
    public void OnResponseDeath(NotificationArg arg)
    {
        CameraFollow.Instance.Shake();
        StopCoroutine("DeathAndResurgence");
        StartCoroutine("DeathAndResurgence", arg);
    }

    private IEnumerator DeathAndResurgence(NotificationArg arg)
    {
        string[] data = arg.GetValue<string[]>();
        Debug.Log(data[0] + data[1] + data[2]);
        hero.GetComponent<Player>().Death();
        NetworkManager.Instance.SendDeath(data[0], data[1], data[2]);
        string name = hero.GetComponent<Player>().playerName;
        messageAborcast.text = name + " use " + data[2] + " kill " + data[1];
        updateText.text = "";
        yield return new WaitForSeconds(resurgenceTime);                            //等待 2s
        clearMessageOnScreen();
        hero.SetActive(false);                                         //尸体消失，人物不可见
        float[] randomPosition = new float[2];
        Vector2 pos =  Player.getRandomPosition(99);
        randomPosition[0] = pos.x;
        randomPosition[1] = pos.y;


        NotificationCenter.Instance.PushEvent(NotificationType.Network_OnRequestResurgence, randomPosition);     //请求复活
    }

    private void clearMessageOnScreen()
    {
        if(messageAborcast != null)
            messageAborcast.text = "";
    }


    void changeButtonImage(SkillType  skill, int level)
    {
        Image datrImg;
        datrImg = GameObject.Find("Canvas/SkillButtons/DartButton/Image").GetComponent<Image>();
        string dartImgUrl = "";
        if (skill == SkillType.DartAttack)
        {
            datrImg = GameObject.Find("Canvas/SkillButtons/DartButton/Image").GetComponent<Image>();
            if (level == 1)
            {
                dartImgUrl = "Images/dart_1";
            }
            else if (level == 2)
            {
                dartImgUrl = "Images/dart_2";
            }else if(level == 3)
            {
                dartImgUrl = "Images/dart_3";
            } 
        }
        else if(skill == SkillType.ShenglongAttack)
        {
            datrImg = GameObject.Find("Canvas/SkillButtons/shenglongButton/Image").GetComponent<Image>();
            if (level == 1)
            {
                dartImgUrl = "Images/knife_1";
            }
            else if (level == 2)
            {
                dartImgUrl = "Images/knife_2";
            }
            else if (level == 3)
            {
                dartImgUrl = "Images/knife_3";
            }

        }
        else if(skill == SkillType.CaoZhuiAttack)
        {
            datrImg = GameObject.Find("Canvas/SkillButtons/caozhuiButton/Image").GetComponent<Image>();
            if (level == 1)
            {
                dartImgUrl = "Images/sword_1";
            }
            else if (level == 2)
            {
                dartImgUrl = "Images/sword_2";
            }
            else if (level == 3)
            {
                dartImgUrl = "Images/sword_3";
            }
        }
        if (dartImgUrl != "")
        {
            datrImg.sprite = Resources.Load(dartImgUrl, typeof(Sprite)) as Sprite;
        }
    }


    //广播死亡
    void OnBroadcastDeath(NotificationArg arg)
    {
        BroadcastDeath data = arg.GetValue<BroadcastDeath>();
        //死者不是我
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            p.GetComponent<Anemy>().Death(data.type);
          
            if (players.ContainsKey(data.anemyID))
            {
                var q = players[data.anemyID];
                messageAborcast.text = q.GetComponent<Anemy>().Name + " use " + data.type + " kill " + p.GetComponent<Anemy>().Name;
                Invoke("clearMessageOnScreen", 4f);
            }
            else
            {
                //就是我杀的
                CameraFollow.Instance.Shake();
                hero.GetComponent<RandomSkillMsg>().skillUpgrade(data.anemyID, hero.GetComponent<Player>().ID, data.type, hero.GetComponent<Player>().state,updateText);
                messageAborcast.text = hero.GetComponent<Player>().Name + " use " + data.type + " kill " + p.GetComponent<Anemy>().Name;
                changeButtonImage(hero.GetComponent<RandomSkillMsg>().m_randomSkillType, hero.GetComponent<RandomSkillMsg>().m_randomSkillLevel);
                Invoke("clearMessageOnScreen", 4f);
            }          
        }
        //死者是我
        if (players.ContainsKey(data.anemyID))
        {
            var q = players[data.anemyID];
            Debug.Log("enemyName is " + q.GetComponent<Anemy>().name);
            messageAborcast.text = q.GetComponent<Anemy>().Name + " use " + data.type + " kill " + hero.GetComponent<Player>().Name;
            Invoke("clearMessageOnScreen", 4f);
        }

        updatePanel(data.anemyID, data.playerID, data.numbers);
    }
    void OnRequestBlink(NotificationArg arg)
    {
        Vector4 data = arg.GetValue<Vector4>();
        Vector4 data2 = hero.GetComponent<Player>().Blink(data);
        NetworkManager.Instance.SendBlink(data2.x, data2.z, data2.w, hero.GetComponent<Player>().ID);
    }

    void OnBroadcastBlink(NotificationArg arg)
    {
        BroadcastBlink data = arg.GetValue<BroadcastBlink>();
        //Debug.Log("enamy blink : " + data.x + " " + data.y +" "+ data.playerID);

        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];

            p.GetComponent<Anemy>().Blink(data.x, data.y, data.w);
        }
    }
    void OnRequestShengLong(NotificationArg arg)
    {
        ReqDartMsg msg = arg.GetValue<ReqDartMsg>();
        Vector4 data = msg.data;
        int level = msg.level;
        if (level == 1)
        {
            NetworkManager.Instance.SendShengLong(data.x, data.z, data.w, hero.GetComponent<Player>().ID, level);
            hero.GetComponent<Player>().ShengLong(data, 1);
        }
        else if(level == 2 || level == 3)
        {
            NetworkManager.Instance.SendShengLong(data.x, data.z, data.w, hero.GetComponent<Player>().ID, level);
            hero.GetComponent<Player>().ShengLong(data, 2);
        }
    }
    void OnBroadcastShengLong(NotificationArg arg)
    {
        BroadcastShengLong data = arg.GetValue<BroadcastShengLong>();
        Debug.Log("ShengLong: " + data.x + " " + data.y + " " + data.playerID);

        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            Vector3 myPosition = hero.GetComponent<Player>().getPlayerPosition();
            p.GetComponent<Anemy>().ShengLong(data, myPosition.x, myPosition.z, hero.GetComponent<Player>().ID, hero.GetComponent<Player>().state, data.skilllevel);
        }
    }

    void OnRequestCaoZhui(NotificationArg arg)
    {
        ReqDartMsg msg = arg.GetValue<ReqDartMsg>();
        Vector4 data = msg.data;
        int level = msg.level;
        //Debug.Log("level:::");
        //Debug.Log(level);
       
        hero.GetComponent<Player>().CaoZhui(data, level);
        NetworkManager.Instance.SendCaoZhui(data.x, data.z, data.w, hero.GetComponent<Player>().ID, level);
      
    }
    void OnBroadcastCaoZhui(NotificationArg arg)
    {
        BroadcastCaoZhui data = arg.GetValue<BroadcastCaoZhui>();
        Debug.Log("CaoZhui: " + data.x + " " + data.y + " " + data.playerID);

        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            //Vector3 myPosition = hero.GetComponent<Player>().getPlayerPosition();

            p.GetComponent<Anemy>().CaoZhui(data, hero.GetComponent<Player>().ID, hero.GetComponent<Player>().state, data.skilllevel);
        }
    }
    void OnRequestWait(NotificationArg arg)
    {
        NetworkManager.Instance.SendWait(hero.GetComponent<Player>().ID);
    }

    //处理其他客户端的闪避表现
    //使用闪避的时候广播人物隐藏，无法被攻击
    void OnBroadcastWait(NotificationArg arg)
    {
        BroadcastWait data = arg.GetValue<BroadcastWait>();
        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            p.GetComponent<Anemy>().Idle();
        }
    }

    void OnResponseJoin(NotificationArg arg)
    {
        ResponseJoin data = arg.GetValue<ResponseJoin>();
        PlayerData self = data.self;
        
        //Debug.Log("number; " + data.list.Count);
        //Vector2 pos = Player.getRandomPosition(data.list.Count);
        //self.x = pos.x;
        //self.y = pos.y;
        hero = CreateMyself(self.x, self.y, self.playerID, data.roomNumber);
        ResurgenceRandomSkills();
        initIntegrationPannel(texts);
        //initOneLine(3, texts, hero.GetComponent<Player>().name);

        
        foreach (PlayerData pdata in data.list)
        {
            var p = CreatePlayer(pdata.x, pdata.y, pdata.playerID, pdata.name);
            EnemyLookAt.Instance.Add( p.transform , nowPlayers, pdata.playerID);
            nowPlayers++;
            players.Add(pdata.playerID, p);
        }
    }

    void OnBroadcastMove(NotificationArg arg)
    {
        BroadcastMove data = arg.GetValue<BroadcastMove>();
        //Debug.Log(data.x + " " + data.y +" "+ data.playerID);
        
        if (players.ContainsKey(data.playerID.ToString()))
        {
            var p = players[data.playerID.ToString()];
           
            p.GetComponent<Anemy>().MoveTo(data.x, data.y, data.w);
        }
        
    }



    void OnBroadcastJoin(NotificationArg arg)
    {
        BroadcastJoin data = arg.GetValue<BroadcastJoin>();
        var p = CreatePlayer(data.x, data.y, data.playerID, data.name);
        players.Add(data.playerID, p);
        //initOneLine(countJoin, texts, p.GetComponent<Anemy>().Name);
       // Debug.Log(p.GetComponent<Anemy>().Name);
        //countJoin += 3;


        EnemyLookAt.Instance.Add(p.transform, nowPlayers, data.playerID);
        nowPlayers++;
    }

    void OnBroadcastLeave(NotificationArg arg)
    {
        BroadcastLeave data = arg.GetValue<BroadcastLeave>();

        if (players.ContainsKey(data.playerID))
        {
            var p = players[data.playerID];
            Destroy(p);
        }
    }

    void OnTouchMap(NotificationArg arg)
    {
        Vector4 targetPos = arg.GetValue<Vector4>();
        //hero.GetComponent<Player>().MoveTo(targetPos.x, targetPos.z);
        //Debug.Log("on touch map" + targetPos.x + " " + targetPos.y +" " + targetPos.z+ " " + targetPos.w);
        NetworkManager.Instance.SendMove(targetPos.x, targetPos.z, targetPos.w,  hero.GetComponent<Player>().ID);
    }

    public GameObject CreateMyself(float x, float y, string playerID, int roomnumber)
    {

        Debug.Log("init player"+ x +  y);
        Vector3 position = new Vector3(x, 0.5f, y);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //GameObject player = Instantiate(ninjaPrefab);
        player.transform.position = position;
        player.GetComponent<Player>().SetName(data2.name);
        //player.GetComponent<Player>().SetType(type);
        player.GetComponent<Player>().ID = playerID;
        player.GetComponent<Player>().roomnumber = roomnumber;

        initOneLine(3, texts, data2.name);
        plane.Add(playerID, 3);
        integrationRecord.Add(playerID, 0);
        rewardRecord.Add(playerID, 1);
   

        return player;
    }

    public GameObject CreatePlayer(float x, float y, string playerID, string enemyName)
    {
        Vector3 position = new Vector3(x, 0.5f, y);
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = position;
        player.GetComponent<Anemy>().SetName(enemyName);
        //player.GetComponent<Anemy>().SetType(type);
        player.GetComponent<Anemy>().ID = playerID;
        player.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = enemyName;


        initOneLine(countJoin, texts, enemyName);
         Debug.Log(player.GetComponent<Anemy>().Name);
        plane.Add(playerID, countJoin);
        integrationRecord.Add(playerID, 0);
        rewardRecord.Add(playerID, 1);

        countJoin += 3;

        return player;
    }


    void OnConnected(NotificationArg arg)
    {
            NetworkManager.Instance.SendJoin(data2.name);
       
    }

    void OnDisconnected(NotificationArg arg)
    {
        Debug.Log("Desconnect");
        NetworkManager.Instance.Destory();
    }

    public Vector3 EnemyPosition(string ID)
    {
        if (players.ContainsKey(ID))
        {
            var p = players[ID];
            return p.transform.position;
        }
        Debug.Log("没有这个角色");
        return transform.position;
    }

    void initIntegrationPannel(Text[] texts)
    {
        
        texts[0].text = "名字";
        texts[1].text = "积分";
        texts[2].text = "悬赏值";
    }

    void initOneLine(int i, Text[] texts, string name)
    {
        texts[i].text = name;
        texts[i + 1].text = "0";
        texts[i + 2].text = "1";
    }

    void updatePanel(string winnerID, string loserID, int numbers)
    {

        int intergation = numbers / 1000;
        int reward = numbers % 1000;

        Debug.Log("intergation is " + intergation);
        Debug.Log("reward is " + reward);

        //更新赢者的积分和悬赏
        if (integrationRecord.ContainsKey(winnerID))
        {
            integrationRecord[winnerID] = intergation;
        }
        if (rewardRecord.ContainsKey(winnerID))
        {
            rewardRecord[winnerID] = reward;
        }
        //更新死者的悬赏
        if (rewardRecord.ContainsKey(loserID))
        {
            rewardRecord[loserID] = 1;
        }
        if (plane.ContainsKey(winnerID))
        {
            texts[plane[winnerID] + 1].text = integrationRecord[winnerID].ToString();
            texts[plane[winnerID] + 2].text = rewardRecord[winnerID].ToString();
        }
        if (plane.ContainsKey(loserID))
        {
            texts[plane[loserID] + 2].text = rewardRecord[loserID].ToString();
        }
    }

}
