using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private GameObject xuying;
    //出生坐标
    public List<Vector2> bornPosition = new List<Vector2>();

    public int countDown = 0;

    private bool isWinTime = false;

    public int winTime = 15;

    public float dodgeTime;

    public State _state { get; private set; }

    public string Name;

    public string playerName { get; set; }

    public float move_speed = 10.0f;
    public int ShengLong_Speed = 15;

    public string ID { get; set; }

    public Animator hands;

    //是否释放升龙斩
    bool FlagShengLong = false;
  //  protected float Animation;
  
    Vector3 InShengLong;
    Vector3 EndShengLong;
    bool FlagShengLongV = false;
   
    //是否处于
    public int state { get; set; }

    //控制多少帧发送一次move消息
    public int num = 3;

    private int nums = 0;

    public float afterBlinkWith = 0.3f;

    public float afterCaoZhuiWith = 1f;
    Vector3 lastPosition;

    //飞镖预设体
    public GameObject dartsPrefab;
    public GameObject darts2Prefab;
    //子弹发射位置
    public Transform dartsPosition;

    public int dartFirSize;
    public int dartSecSize;

    public GameObject SwordPrefab; 
    GameObject CirclyCaoZhui;
    //房间号
    public int roomnumber;

    string[] DeathWay;

    public object thisLock = new object();

    private void Start()
    {
        xuying = transform.GetChild(2).gameObject;
        xuying.GetComponent<ParticleSystem>().Stop();

        state = (int)State.waitBegin;

        transform.GetChild(0).GetChild(0).GetComponent<Text>().text ="";

        hands = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        lastPosition = transform.position;

       // isDodge = false;
        CirclyCaoZhui = Instantiate(Resources.Load("Effect/Prefabs/Hero_skillarea/quan_hero")) as GameObject;

        //state = (int)State.normal; 
        DeathWay = new string[3] { "Dead_1", "Dead_2", "Dead_3"};

        CirclyCaoZhui.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        CirclyCaoZhui.gameObject.SetActive(false);
       EndShengLong = new Vector3();
    }

    public void FixedUpdate()
    {
        Debug.Log("ID: " + ID);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); 

        var child = transform.GetChild(0).gameObject;
        child.transform.localEulerAngles = new Vector3(0.0f, -transform.eulerAngles.y, 0.0f);

        if (FlagShengLong)
        {
            //n++;
            //Animation += Time.deltaTime * speed * 10;
            //Animation = Animation % 3f;
            //transform.position = MathParabola.Parabola(transform.position, EndShengLong, 1f, Animation / 3f);
            //if (FlagN && n == 5)
            //{
            //    FlagN = false;
            //    hands.Play("Jump_Forward");

            //}
            //if (Animation / 10 < 0.1 )
            //{
            //    FlagShengLong = false;
            //    transform.position = EndShengLong;
            //}
            int level = GetComponent<RandomSkillMsg>().m_randomSkillLevel;
            float step;
            if (level == 1)
            {
                step = ShengLong_Speed * Time.deltaTime * 1f;
            }
            else
            {
                step = ShengLong_Speed * Time.deltaTime * 2f;
            }
            //Vector3 temp = new Vector3();
            //temp.x = (transform.localPosition.x + EndShengLong.x) / 2;
           
            //temp.z = (transform.localPosition.z + EndShengLong.z) / 2;
            //temp.y = 4f;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, InShengLong, step);
           
            if (transform.localPosition.x < InShengLong.x + 0.1f && transform.localPosition.x > InShengLong.x -  0.1f 
                && transform.localPosition.y < InShengLong.y + 0.1f && transform.localPosition.y > InShengLong.y - 0.1f
                && transform.localPosition.z < InShengLong.z + 0.1f && transform.localPosition.z > InShengLong.z - 0.1f)
            {
                //Debug.Log("停止");
                //Debug.Log(FlagShengLong);
                FlagShengLongV = true;
                FlagShengLong = false;
               // hands.Play("Jump_Forward");

            }
           
        }
        if (FlagShengLongV)
        {
            //Debug.Log("FlagN");
            int level = GetComponent<RandomSkillMsg>().m_randomSkillLevel;
            float step;
            if (level == 1)
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
            
            }
        }
     
     }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("碰撞！！！！");
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.transform.name);
        Debug.Log(collision.transform.gameObject.name);
        if (collision.gameObject.transform.tag == "Anemy" )
        {
            return;
        }
        Vector3 temp = transform.position;
        temp.y = 0.5f;
        transform.position = temp;
        FlagShengLongV = false;
        FlagShengLong = false;
    }
    private void OnCollisionExit(Collision collision)
    {

     
    }
    private void PushMoveEvent()
    {
        Vector4 data;
        data.x = transform.position.x;
        data.y = transform.position.y;
        data.z = transform.position.z;
        data.w = transform.eulerAngles.y;
        NotificationCenter.Instance.PushEvent(NotificationType.Operate_MapPosition, data);
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetType(int type)
    {
        //Color[] colors = { Color.blue, Color.cyan, Color.yellow, Color.red, Color.green };
        //GetComponent<MeshRenderer>().material.color = colors[type];
    }

   /* public bool getIsDodge()
    {
        return isDodge;
    }*/



    public void DodgeTo()
    {
        StopCoroutine("Dodge");
        Debug.Log("Player DodgeTo");
        StartCoroutine("Dodge");
    }



    private IEnumerator Dodge()
    {
        //Debug.Log("now dodge hide");
        hands.Play("Dodge");
        xuying.GetComponent<ParticleSystem>().Play();

        lock (thisLock)
        {
            state = (int)State.dodge;
        }
        yield return new WaitForSeconds(dodgeTime);
        if (state == (int)State.dodge)
        {
            lock (thisLock)
            {
                state = (int)State.normal;
            }
        }
    }

    public Vector3 getPlayerPosition()
    {
        return transform.position;
    }


    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }

    public void BeginGame()
    {
        if (state == (int)State.waitBegin)
        {
            state = (int)State.normal;
        }
    }


    void OnJoystickMoveEnd(MovingJoystick move)
    {
        if (move.joystickName == "joy")
        {
            if (state == (int)State.normal || state == (int)State.dodge)
            {
                hands.Play("Idle");
                NotificationCenter.Instance.PushEvent(NotificationType.Request_Wait, null);
                //StartCoroutine(WaitIdle(0.3f));
            }

        }
    }

    IEnumerator WaitIdle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NotificationCenter.Instance.PushEvent(NotificationType.Request_Wait, null);
    }

    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "joy")
        {
            return;
        }
        float joyPositionX = move.joystickAxis.x;
        float joyPositionZ = move.joystickAxis.y;

        if ((joyPositionZ != 0 || joyPositionX != 0) && (state == (int)State.normal || state == (int)State.dodge))
        {

            transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionZ));

            transform.Translate(Vector3.forward * Time.deltaTime * move_speed);

            hands.Play("Run");

            nums++;
            if (nums == num)
            {
                nums = 0;
                PushMoveEvent();
            }
        }

        
    }

    public float angle_360(Vector3 from)
    {
        float angle = Mathf.Atan2(from.x, from.z) * Mathf.Rad2Deg;
        return angle;
        /*
        Vector3 v3 = Vector3.Cross(from_, to_);
        if (v3.z > 0)
            return Vector3.Angle(from_, to_);
        else
            return 360 - Vector3.Angle(from_, to_);
            */
    }
    IEnumerator WaitDarts()
    {
        yield return new WaitForSeconds(1f);
        // FlagShengLong = false;
        //  hands.Play("Light_Attk_3");
        lock (thisLock)
        {
            state = (int)State.normal;
        }
    }
    //己方英雄发射飞镖
    public void SendDarts(Vector3 angles, int level)
    {
        /*
        Vector4 data;
        data.x = transform.position.x;
        data.y = transform.position.y;
        data.z = transform.position.z;
        data.w = angle_360(angles);
        NotificationCenter.Instance.PushEvent(NotificationType.Operate_MapPosition, data);
        */
        lock(thisLock)
        {
            state = (int)State.play;
        }
        if (level == 1)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, angle_360(angles), transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartFirSize, dartFirSize, dartFirSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, angle_360(angles) - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level1");
        }else if(level == 2)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, angle_360(angles), transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartSecSize, dartSecSize, dartSecSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, angle_360(angles) - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level2");
        }
        else if (level == 3)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.x, angle_360(angles), transform.rotation.z);
            GameObject go = Instantiate(dartsPrefab) as GameObject;
            go.transform.localScale = new Vector3(dartSecSize, dartSecSize, dartSecSize);
            go.GetComponent<Darts>().user = this.ID;
            go.transform.position = dartsPosition.position;
            go.transform.localEulerAngles = new Vector3(go.transform.rotation.x, angle_360(angles) - 90.0f, go.transform.rotation.z);
            Debug.Log("发送飞镖 level3");
        }


    }

    public void Death()
    {
        Debug.Log("players Death");
        if (state == (int)State.death)
            return;
        if(isWinTime == true)
        {
            StopCoroutine("CountDown");
        }
        //if (data == SkillType.DartAttack.ToString())
        //{
        //    Debug.Log("dead 1");
        //    hands.Play("Dead_1");
        //}
        //else if (data == SkillType.MeleeAttack.ToString())
        //{
        //    Debug.Log("dead 2");
        //    hands.Play("Dead_2");
        //}else if (data == SkillType.ShenglongAttack.ToString())
        //{
        //    Debug.Log("dead 3");
        //    hands.Play("Dead_3");
        //}
        int Index = Random.Range(0, 3);

        hands.Play(DeathWay[Index]);
        lock (thisLock)
        {
            state = (int)State.death;
        }
    }

    public void Resurgence(float x, float z)
    {
        Debug.Log("players Resurgence");
        transform.position = new Vector3(x, 0.5f, z);
        hands.Play("Idle");
        if (isWinTime == true)
        {
            StartCoroutine("CountDown");
        }
        lock (thisLock)
        {
            state = (int)State.normal;
        }
        //技能切换为4个技能技能之一
    }

    static public Vector2 getRandomPosition(int i)
    {

        //出生坐标
        List<Vector2> bornPosition = new List<Vector2>();

        bornPosition.Add(new Vector2(8.7f, -5.7f));
        bornPosition.Add(new Vector2(8.9f, 24.83f));
        bornPosition.Add(new Vector2(-17f, 25.86f));
        bornPosition.Add(new Vector2(-16f, -5f));

        System.Random rd = new System.Random();
        int p;
        if (i >= 0 && i <= 3)
        {
            p = i;
        }
        else
        {
            p = rd.Next(0, bornPosition.Count );
        }
        float x = bornPosition[p].x;
        //float x1 = (float)(rd.NextDouble() * 4.0 - 2.0);
        float y = bornPosition[p].y;
        //float y1 = (float)(rd.NextDouble() * 4.0 - 2.0);
        

        return new Vector2(x , y );
    }

    public void MeleeDamage()
    {
        hands.Play("Hard_Attk_5");
    }

    IEnumerator Wait(float waitTime, Vector4 data)
    {

        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint " + Time.time);
        transform.SetPositionAndRotation(data, transform.rotation);
        transform.localEulerAngles = new Vector3(transform.rotation.x, data.w, transform.rotation.z);
        lock (thisLock)
        {
            if(state == (int)State.play)
                state = (int)State.normal;
        }
    }

   
    public Vector4 Blink(Vector4 data)
    {
        lock (thisLock)
        {
            state = (int)State.play;
        }
        Vector3 direction = new Vector3(data.x, data.y, data.z)- transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(data, transform.position)))
        {
            Debug.Log("...pengzhuang...");
            data.x = hit.point.x;
            data.z = hit.point.z;
        }
        hands.Play("Floor_Hard_With_Sword");
        StartCoroutine(Wait(afterBlinkWith, data));
        return data;
        ///transform.position = new Vector3(data.x, 0.5f, data.z);
        
    }
    IEnumerator ShengLongSkill(int level)
    {
        if(level == 1)
            yield return new WaitForSeconds(1.5f);
        else
            yield return new WaitForSeconds(2f);
        // FlagShengLong = false;
        //  hands.Play("Light_Attk_3");
        lock (thisLock)
        {
            if(state == (int)State.play)
                state = (int)State.normal;
        }
    }
    public void ShengLong(Vector4 data, int level)
    {
        lock (thisLock)
        {
            state = (int)State.play;
        }
        // hands.Play("Jump_Forward");
        //StartCoroutine(Wait(afterBlinkWith, data));
        if (level == 1)
            hands.Play("Jump_Forward_Inplace");
        else
            hands.Play("Jump_Forward_Inplace2");
        EndShengLong.x = data.x;
        EndShengLong.y = 0.5f;
        EndShengLong.z = data.z;

        InShengLong.x = (transform.localPosition.x + EndShengLong.x) / 2;
        InShengLong.z = (transform.localPosition.z + EndShengLong.z) / 2;
        float S1 = System.Math.Abs( EndShengLong.x - transform.localPosition.x);
        float S2 = System.Math.Abs( EndShengLong.z - transform.localPosition.z);
        //Debug.Log(S1);
        //Debug.Log(S2);
        float MaxS1S2 = S1 > S2 ? S1 : S2;
        InShengLong.y = MaxS1S2 - 1.5f;
        if (MaxS1S2 < 1)
            InShengLong.y = 1f;
       
        FlagShengLong = true;
        FlagShengLongV = false;
  
        //Update();
        //StartCoroutine ("UpdateShengLong");
       StartCoroutine(ShengLongSkill(level));
    }
    IEnumerator WaitCaoZhui(float waitTime, Vector4 data, int level)
    {    
        print("WaitAndPrint " + Time.time);

        if (level == 1)
        {
            CirclyCaoZhui.transform.SetPositionAndRotation(data, transform.rotation);
            CirclyCaoZhui.transform.localScale = new Vector3(4f, 4f, 4f);
            CirclyCaoZhui.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            GameObject temp = Instantiate(SwordPrefab) as GameObject;
            temp.transform.localScale = new Vector3(2, 2, 2);
            temp.transform.position = new Vector3(data.x, 8, data.z);
        }
        else if (level == 2 || level == 3)
        {
            CirclyCaoZhui.transform.SetPositionAndRotation(data, transform.rotation);
            CirclyCaoZhui.transform.localScale = new Vector3(6f, 6f, 6f);
            CirclyCaoZhui.gameObject.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            GameObject temp = Instantiate(SwordPrefab) as GameObject;
            temp.transform.localScale = new Vector3(3, 3, 3);
            temp.transform.position = new Vector3(data.x, 8, data.z);
        }
        CirclyCaoZhui.gameObject.SetActive(false);
        lock (thisLock)
        {
            if(state == (int)State.play)
                state = (int)State.normal;
        }
    }
    public void CaoZhui(Vector4 data, int level)
    {
        lock (thisLock)
        {
            state = (int)State.play;
        }
          
        hands.Play("caozhui");
        StartCoroutine(WaitCaoZhui(afterCaoZhuiWith, data, level));
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
            Debug.Log(countDown);
            yield return new WaitForSeconds(1);
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = countDown.ToString();
            countDown--;
            if(countDown == 0)
            {
                Debug.Log("winnner");
                NotificationCenter.Instance.PushEvent(NotificationType.Networt_OnRequestWinner, null);
            }
        }
        
    }

}
