using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Anemy : MonoBehaviour
{
    private GameObject xuying;
    private GameObject pugong;
    //获胜倒计时
    public int countDown = 0;

    public int winTime = 20;

    private bool isWinTime = false;

    public string Name { get; set; }

    public float move_speed = 10.0f;

    public float ShengLong_Speed = 15f;
    public string ID { get; set; }

    public bool isDeath;
    public bool isIdle;
    public bool isPLay;
    public Animator hands;
    struct SkillShengLong
    {

        public BroadcastShengLong Da;
        public float x;
        public float z;
        public string PlayerID;
        public int State;
        public int Level;
    };
    string[] DeathWay;

    //飞镖预设体
    public GameObject dartsPrefab;
    public GameObject darts2Prefab;

    
    public int dartFirSize;
    public int dartSecSize;
    //子弹发射位置
    public Transform dartsPosition;
    //旋转攻击范围
    private double radius = 2;

    public GameObject SwordPrefab;

    private Rigidbody anemyRigidbody;

    public float afterBlinkWith = 0.3f;
    public float afterShengLong = 0.5f;
    public float afterCaoZhui = 1.3f;

    bool FlagShengLong = false;
    bool FlagShengLongV = false;
    bool FlagShengLongD = false;
    bool FlagShengLongC = false;

    Vector3 InShengLong;
    Vector3 EndShengLong;
    SkillShengLong DataShengLong;


    GameObject CirclyCaoZhui;
    public void SetName(string name)
    {
       // print("set name");
        Name = name;
    }


    public void SetType(int type)
    {
        //Color[] colors = { Color.blue, Color.cyan, Color.yellow, Color.red, Color.green };
        //GetComponent<MeshRenderer>().material.color = colors[type];
    }

    private void Start()
    {
        xuying = transform.GetChild(2).gameObject;
        xuying.GetComponent<ParticleSystem>().Stop();
        pugong = transform.GetChild(3).gameObject;
        pugong.GetComponent<ParticleSystem>().Stop();

        isDeath = false;
        isIdle = true;
        isPLay = false;

        anemyRigidbody = gameObject.GetComponent<Rigidbody>();

        CirclyCaoZhui = Instantiate(Resources.Load("Effect/Prefabs/Hero_skillarea/quan_hero")) as GameObject;

        CirclyCaoZhui.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        CirclyCaoZhui.gameObject.SetActive(false);
        DeathWay = new string[3] { "Dead_1", "Dead_2", "Dead_3" };

        hands = transform.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {

        var child = transform.GetChild(0).gameObject;
        child.transform.localEulerAngles = new Vector3(0.0f, -transform.eulerAngles.y, 0.0f);
        //transform.GetChild(10).LookAt(Camera.main.transform);


        if (FlagShengLong)
        {
            float step;
            if (DataShengLong.Level == 1)
            {
                step = ShengLong_Speed * Time.deltaTime * 1f;
            }
            else
            {
                step = ShengLong_Speed * Time.deltaTime * 2f;
            }
           

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, InShengLong, step);

            if (transform.localPosition.x < InShengLong.x + 0.1f && transform.localPosition.x > InShengLong.x - 0.1f
                && transform.localPosition.y < InShengLong.y + 0.1f && transform.localPosition.y > InShengLong.y - 0.1f
                && transform.localPosition.z < InShengLong.z + 0.1f && transform.localPosition.z > InShengLong.z - 0.1f)
            {
                FlagShengLongV = true;
                FlagShengLong = false;

            }

        }
        if (FlagShengLongV)
        {
            //Debug.Log("FlagN");
            float step;
            if (DataShengLong.Level == 1)
            {
                step = ShengLong_Speed * Time.deltaTime * 1f;
            }
            else
            {
                step = ShengLong_Speed * Time.deltaTime * 2f;
            }
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, EndShengLong, step);
            if (transform.localPosition.x < EndShengLong.x + 0.1f && transform.localPosition.x > EndShengLong.x - 0.1f
                && transform.localPosition.y < EndShengLong.y + 0.1f && transform.localPosition.y > EndShengLong.y - 0.1f
                && transform.localPosition.z < EndShengLong.z + 0.1f && transform.localPosition.z > EndShengLong.z - 0.1f)
            {
                FlagShengLongV = false;
                FlagShengLongD = true;
            }
        }
        if (FlagShengLongD && FlagShengLongC)
        {
            // StartCoroutine(WaitShengLong(DataShengLong.Da, DataShengLong.x, DataShengLong.z,  DataShengLong.PlayerID));
            AnimatorStateInfo info = hands.GetCurrentAnimatorStateInfo(0);
            // 判断动画是否播放完成
            if (info.normalizedTime >= 0.5f)
            {
                FlagShengLongD = false;
                FlagShengLongC = false;
                Player player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
                if (player.state == (int)State.normal || player.state == (int)State.play)
                {
                    if (DataShengLong.Level == 1)
                    {

                        if (IsInCircleAttackArea(player.transform.position.x, player.transform.position.z, transform.position.x, transform.position.z))
                        {
                            //这里应该播放我的死亡的动画和我的生命减1
                            Debug.Log("死 ");
                            string[] rotationMsg = new string[3];
                            rotationMsg[0] = DataShengLong.Da.playerID;
                            rotationMsg[1] = DataShengLong.PlayerID;
                            rotationMsg[2] = SkillType.ShenglongAttack.ToString();
                            Debug.Log("Darts death: " + rotationMsg[0] + rotationMsg[1] + rotationMsg[2]);
                            NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, rotationMsg);

                        }
                        else 
                        {
                            Debug.Log("活");

                        }
                    }
                    else if (DataShengLong.Level == 2 || DataShengLong.Level == 3)
                    {
                        if (IsInCircleAttackArea(player.transform.position.x, player.transform.position.z, transform.position.x, transform.position.z, 3f))
                        {
                            //这里应该播放我的死亡的动画和我的生命减1
                            Debug.Log("死 ");
                            string[] rotationMsg = new string[3];
                            rotationMsg[0] = DataShengLong.Da.playerID;
                            rotationMsg[1] = DataShengLong.PlayerID;
                            rotationMsg[2] = SkillType.ShenglongAttack.ToString();
                            Debug.Log("Darts death: " + rotationMsg[0] + rotationMsg[1] + rotationMsg[2]);
                            NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, rotationMsg);

                        }
                        else
                        {
                            Debug.Log("活");

                        }
                    }

                }
                isPLay = false;
            }

        }
        child = transform.GetChild(10).gameObject;
        
        child.transform.localEulerAngles = new Vector3(0.0f, -transform.eulerAngles.y, 0.0f);

    }

    public void Resurgence(float x, float z)
    {

        isDeath = false;

        Debug.Log("anemys Resurgence");
        transform.position = new Vector3(x, 0.5f, z);
        hands.Play("Idle");
        if(isWinTime == true)
        {
            StartCoroutine("CountDown");
        }
    }


    public void MoveTo(float x, float y, float w)
    {
        StopCoroutine("Move");
        Vector4 targetPos = new Vector4(x, 0.5f, y, w);
        StartCoroutine("Move", targetPos);
        //hands.Play("Idle");
    }

    public void Idle()
    {
        isIdle = true;
        if (hands != null)
            hands.Play("Idle");
    }

    //移动函数
    //获取移动的目标地址
    private IEnumerator Move(Vector4 target_pos)
    {
        isPLay = false;
        if (Vector3.Distance(transform.position, target_pos) < 0.1f)
        {
            yield break;
        }

        isIdle = false;

        Vector3 start_pos = transform.position;
        
        float move_distance = 0;
        float move_ratio = 0;
        float total_distance = Vector3.Distance(start_pos, target_pos);
        transform.LookAt(target_pos);

        if (Vector3.Distance(transform.position, target_pos) < 0.1f)
        {
            anemyRigidbody.MovePosition(new Vector3(target_pos.x, transform.position.y, target_pos.z));
            transform.localEulerAngles = new Vector3(transform.rotation.x, target_pos.w, transform.rotation.z);

            if (hands != null)
                hands.Play("Run");
        }
        else
        {
            while (true)
            {
                
                if (isDeath == true) {
                    if (hands != null)
                    {
                        hands.Play("Dead_3");
                    }
                    yield break;
                } 
                if (isPLay == true)
                {
                    if (hands != null)
                    {
                        hands.Play("Idle");
                    }
                    yield break;
                }
                if (Vector3.Distance(transform.position, target_pos) < 0.1f)
                {
                    if (isIdle == true)
                    {
                        if (hands != null)
                        {
                            hands.Play("Idle");
                        }
                    }
                    yield break;
                }
                move_distance += move_speed * Time.deltaTime;
                move_ratio = move_distance / total_distance;
                Vector3 position = Vector3.Lerp(start_pos, target_pos, move_ratio);
                transform.position = position;
                transform.localEulerAngles = new Vector3(transform.rotation.x, target_pos.w, transform.rotation.z);
                if (hands != null)
                {
                    hands.Play("Run");
                }
                yield return null;

            }
            /*
            anemyRigidbody.MovePosition(new Vector3(target_pos.x, transform.position.y, target_pos.z));
            transform.localEulerAngles = new Vector3(transform.rotation.x, target_pos.w, transform.rotation.z);


            if(hands != null)
                hands.Play("Run");
                */
        }
    }

    public void DodgeTo()
    {
        StopCoroutine("Dodge");
        //Debug.Log("enemy DodgeTo");
        StartCoroutine("Dodge");
    }

    private IEnumerator Dodge()
    {
        //Debug.Log("now dodge hide");
        hands.Play("Dodge");
        xuying.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(10);
        //Debug.Log("now dodge appear");
    }

    private bool IsInCircleAttackArea(float myX, float myY, float otherX, float otherY)
    {
        float x = System.Math.Abs(myX - otherX);
        float y = System.Math.Abs(myY - otherY);
        double tmp = (double)x * x + y * y;
        double distance = System.Math.Sqrt(tmp);

        if (distance <= radius)
        {
            return true;
        }
        return false;
    }
    private bool IsInCircleAttackArea(float myX, float myY, float otherX, float otherY, float range)
    {
        float x = System.Math.Abs(myX - otherX);
        float y = System.Math.Abs(myY - otherY);
        double tmp = (double)x * x + y * y;
        double distance = System.Math.Sqrt(tmp);

        if (distance <= range)
        {
            return true;
        }
        return false;
    }

    public bool MeleeDamage(BroadcastMeleeDamage data, float x, float z, string playerId, int state)
    {
        //播放动画
        //发动近程攻击以后自己能看到效果，这里就是放一个动画，动画有延迟，事件触发以后，先放动画，动画放到一半的时候，再计算计算伤害。

        Debug.Log("Anemy OnMeleeDamage");
        hands.Play("Hard_Attk_5");
        pugong.GetComponent<ParticleSystem>().Play();
        if (state != (int)State.dodge && state != (int)State.death)
        {
            if (IsInCircleAttackArea(x, z, data.x, data.y))
            {
                //这里应该播放我的死亡的动画和我的生命减1
                Debug.Log("阿 我死了！");
                string[] rotationMsg = new string[3];
                rotationMsg[0] = data.playerID;
                rotationMsg[1] = playerId;
                rotationMsg[2] = SkillType.MeleeAttack.ToString();
                Debug.Log("Darts death: " + rotationMsg[0] + rotationMsg[1] + rotationMsg[2]);
                NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, rotationMsg);
                return true;
            }
            else
            {
                Debug.Log("距离太远 死不了！");
                return false;
            }
        }
        else
        {
            Debug.Log("闪避状态 死不了！");
            return false;
        }
    }




    public void SendDarts(float x, float z, float w, int level)
    {
        if (level == 1)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, w, transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartFirSize, dartFirSize, dartFirSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, w - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level1");
        }
        else if (level == 2)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, w, transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartSecSize, dartSecSize, dartSecSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, w - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level2");
        }
        else if (level == 3)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, w, transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartSecSize, dartSecSize, dartSecSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, w - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level3");
        }
    }


    public void Death(string data)
    {
        Debug.Log("enemy Death");
        Debug.Log(data);
        //设置死亡状态
        isDeath = true;

        if (isWinTime == true)
        {
            StopCoroutine("CountDown");
        }
        hands.Play(DeathWay[Random.Range(0, 3)]);
    }

    IEnumerator Wait(float waitTime, Vector4 data)
    {
        yield return new WaitForSeconds(waitTime);
        transform.SetPositionAndRotation(data, transform.rotation);
        transform.localEulerAngles = new Vector3(transform.rotation.x, data.w, transform.rotation.z);
    }

    public void Blink(float x, float z, float w)
    {
        Vector4 data = new Vector4(x, transform.position.y, z, w);

        hands.Play("Floor_Hard_With_Sword");
        StartCoroutine(Wait(afterBlinkWith, data));
    }


    public void ShengLong(BroadcastShengLong Da, float x, float z, string PlayerID, int state, int level)
    {
        isPLay = true;
        transform.localEulerAngles = new Vector3(transform.rotation.x, Da.w, transform.rotation.z);
        Vector4 data = new Vector4(Da.x, transform.position.y, Da.y, Da.w);
        //Debug.Log("放升龙斩 ");
        // hands.Play("Jump_Forward");
        if(level == 1)
            hands.Play("Jump_Forward_Inplace");
        else
            hands.Play("Jump_Forward_Inplace2");
        EndShengLong.x = data.x;
        EndShengLong.y = 0.5f;
        EndShengLong.z = data.z;

        InShengLong.x = (transform.localPosition.x + EndShengLong.x) / 2;
        InShengLong.z = (transform.localPosition.z + EndShengLong.z) / 2;
        float S1 = System.Math.Abs(EndShengLong.x - transform.localPosition.x);
        float S2 = System.Math.Abs(EndShengLong.z - transform.localPosition.z);
        //Debug.Log(S1);
        //Debug.Log(S2);
        float MaxS1S2 = S1 > S2 ? S1 : S2;
        InShengLong.y = MaxS1S2 - 1.5f;
        if (MaxS1S2 < 1)
            InShengLong.y = 1f;

        FlagShengLong = true;
        FlagShengLongV = false;
        DataShengLong.Da = Da;
        DataShengLong.x = x;
        DataShengLong.z = z;
        DataShengLong.PlayerID = PlayerID;
        DataShengLong.State = state;
        DataShengLong.Level = level;
        FlagShengLongC = true;

    }


    IEnumerator WaitCaoZhui(float waitTime, Vector4 data, BroadcastCaoZhui Da, string PlayerID, int state, int level)
    {
        Transform tPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (level == 1)
        {
            CirclyCaoZhui.transform.localScale = new Vector3(4f, 4f, 4f);
            CirclyCaoZhui.transform.SetPositionAndRotation(data, transform.rotation);
            CirclyCaoZhui.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            GameObject temp = Instantiate(SwordPrefab) as GameObject;
            temp.transform.localScale = new Vector3(2, 2, 2);
            temp.transform.position = new Vector3(data.x, 8, data.z);
            CirclyCaoZhui.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.51f);
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player.state == (int)State.normal || player.state == (int)State.play)
            {
                float x = tPlayer.position.x;
                float z = tPlayer.position.z;
                if (IsInCircleAttackArea(x, z, Da.x, Da.y))
                {
                    Debug.Log("死 ");
                    string[] rotationMsg = new string[3];
                    rotationMsg[0] = Da.playerID;
                    rotationMsg[1] = PlayerID;
                    rotationMsg[2] = SkillType.CaoZhuiAttack.ToString();
                    Debug.Log("Darts death: " + rotationMsg[0] + rotationMsg[1] + rotationMsg[2]);
                    NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, rotationMsg);
                }
                else
                {
                    Debug.Log("活");

                }
            }
        }
        else if (level == 2 || level == 3)
        {
            CirclyCaoZhui.transform.localScale = new Vector3(6f, 6f, 6f);
            CirclyCaoZhui.transform.SetPositionAndRotation(data, transform.rotation);
            CirclyCaoZhui.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            GameObject temp = Instantiate(SwordPrefab) as GameObject;
            temp.transform.localScale = new Vector3(3, 3, 3);
            temp.transform.position = new Vector3(data.x, 8, data.z);
            CirclyCaoZhui.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.51f);
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player.state == (int)State.normal || player.state == (int)State.play)
            {
                float x = tPlayer.position.x;
                float z = tPlayer.position.z;
                if (IsInCircleAttackArea(x, z, Da.x, Da.y, 3f))
                {
                    Debug.Log("死 ");
                    string[] rotationMsg = new string[3];
                    rotationMsg[0] = Da.playerID;
                    rotationMsg[1] = PlayerID;
                    rotationMsg[2] = SkillType.CaoZhuiAttack.ToString();
                    Debug.Log("Darts death: " + rotationMsg[0] + rotationMsg[1] + rotationMsg[2]);
                    NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, rotationMsg);
                }
                else
                {
                    Debug.Log("活");

                }
            }
        }
    }
    public void CaoZhui(BroadcastCaoZhui Da, string PlayerID, int state, int level)
    {
        Debug.Log("处理广播草锥剑 level1");
        transform.localEulerAngles = new Vector3(transform.rotation.x, Da.w, transform.rotation.z);
        Vector4 data = new Vector4(Da.x, transform.position.y, Da.y, Da.w);
        Debug.Log("放草锥剑 ");
        hands.Play("caozhui");
        StartCoroutine(WaitCaoZhui(afterCaoZhui, data, Da, PlayerID, state, level));
        //if (level == 1)
        //{

        //    Debug.Log("处理广播草锥剑 level1");
        //    transform.localEulerAngles = new Vector3(transform.rotation.x, Da.w, transform.rotation.z);
        //    Vector4 data = new Vector4(Da.x, transform.position.y, Da.y, Da.w);
        //    Debug.Log("放草锥剑 ");
        //    hands.Play("Floor_Hard_With_Sword");
        //    StartCoroutine(WaitCaoZhui(afterCaoZhui, data, Da, x, z, PlayerID, state, level));
        //}else if(level == 2)
        //{
        //    Debug.Log("处理广播草锥剑 level 2");

        //}
        //else if(level == 3)
        //{
        //    Debug.Log("处理广播草锥剑 level3");
        //}
        //else
        //{
        //    Debug.Log("处理广播草锥剑 level error");
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("碰撞！！！！Anemy");


    }
    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.transform.tag == "Player" )
        {
            return;
        }

        Vector3 temp = transform.position;
        temp.y = 0.5f;
        transform.position = temp;
        FlagShengLongV = false;
        FlagShengLong = false;
        FlagShengLongD = true;

    }
    private void OnCollisionExit(Collision collision)
    {
        FlagShengLongD = false;

    }
    public void BCountDown()
    {
        if (isWinTime == false)
        {
            isWinTime = true;
            StartCoroutine("CountDown");
        }
        
    }
    IEnumerator CountDown()
    {
        countDown = winTime;
        while (countDown >= 0)
        {
            Debug.Log("enemy; " + countDown);
            transform.GetChild(0).GetChild(1).GetComponent<Text>().text = countDown.ToString();
            yield return new WaitForSeconds(1);
            countDown--;
        }
    }
}
