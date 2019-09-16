using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkillJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    public float outerCircleRadius = 100;

    Transform innerCircleTrans;

    Vector2 outerCircleStartWorldPos = Vector2.zero;

    public Action<Vector2> onJoystickDownEvent;     // 按下事件
    public Action onJoystickUpEvent;     // 抬起事件
    public Action<Vector2> onJoystickMoveEvent;     // 滑动事件
    //摄像头
    public Camera uiCam;

    public RectTransform parent;

    //public static CN.CountDownTimer CDTDarts;
    //public static CN.CountDownTimer CDTDodge;
    //public static CN.CountDownTimer CDTShengLong;
    //CN.CountDownTimer CDTDarts;
    CN.CountDownTimer CDT;

    public int skill;



    //bool flagDarts = true;  //初始未冷却
    //bool flagDodge = true;
    bool flag = true;
    //bool flagShengLong = true;

    //bool TimeUpDarts;
    //bool TimeUpDodge;
    bool TimeUp;
    //bool TimeUpShengLong;

    bool FlagSate = false;

    public float InitTime = 5f;


    //float TimePercentDarts;
    //float TimePercentDodge;
    //float TimePerCentShengLong;
    float TimePercent;

    //Image coolingImageDarts;
    //Image coolingImageDodge;
    Image coolingImage;
    //Image coolingImageShengLong;

    int LevelNum = 2;
    public float Leve1NumOneTime = 5f;
    public float LevelNumTwoTime = 5f;
    CN.CountDownTimer CDLeve1One;

    bool Flaglevel = false;
    bool FlagOne = false;
    bool FlagTwo = false;

    Text ShowLevelNum;

    GameObject elementGo;
    public int skillType;
    void Awake()
    {
        innerCircleTrans = transform.GetChild(0);
    }

    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("Cav").transform as RectTransform;
        outerCircleStartWorldPos = transform.position;
        if (skill == 0 || skill == 4 || skill == 5)
        {

            CDLeve1One = new CN.CountDownTimer(Leve1NumOneTime);

            if (skill == 0)
            {
                ShowLevelNum = GameObject.Find("Canvas/SkillButtons/DartButton/Text").GetComponent<Text>();
                ShowLevelNum.text = "";
            }
            if (skill == 4)
            {
                ShowLevelNum = GameObject.Find("Canvas/SkillButtons/shenglongButton/Text").GetComponent<Text>();
                ShowLevelNum.text = "";
            }
            if (skill == 5)
            {
                ShowLevelNum = GameObject.Find("Canvas/SkillButtons/caozhuiButton/Text").GetComponent<Text>();
                ShowLevelNum.text = "";
            }

        }
        if (skill == 7)
        {

            elementGo = Instantiate(Resources.Load("Effect/Prefabs/Hero_skillarea/quan_hero")) as GameObject;

            elementGo.transform.localScale = new Vector3(2, 2, 2);
            skillType = 1;
            elementGo.gameObject.SetActive(false);
        }
        //switch (skill)
        //{
        //    case 1:
        //        CDTDarts = new CN.CountDownTimer(InitTime);
        //        coolingImageDarts = transform.GetChild(0).GetComponent<Image>();
        //        coolingImageDarts.raycastTarget = false;
        //        return;
        //    case 2:
        //        CDTDodge = new CN.CountDownTimer(InitTime);
        //        coolingImageDodge = transform.GetChild(0).GetComponent<Image>();
        //        coolingImageDodge.raycastTarget = false;
        //        return;
        //    case 3:
        //        CDT = new CN.CountDownTimer(InitTime);
        //        coolingImage = transform.GetChild(0).GetComponent<Image>();
        //        coolingImage.raycastTarget = false;
        //        return;
        //    case 4:
        //        CDTShengLong = new CN.CountDownTimer(InitTime);
        //        coolingImageShengLong = transform.GetChild(0).GetComponent<Image>();
        //        coolingImageShengLong.raycastTarget = false;
        //        return;
        //    case 5:
        //        CDTShengLong = new CN.CountDownTimer(InitTime);
        //        coolingImageShengLong = transform.GetChild(0).GetComponent<Image>();
        //        coolingImageShengLong.raycastTarget = false;
        //        return;

        //}
        CDT = new CN.CountDownTimer(InitTime);
        coolingImage = transform.GetChild(0).GetComponent<Image>();
        coolingImage.raycastTarget = false;
    }

    /// <summary>
    /// 按下
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {

        //switch (skill)
        //{
        //    case 1:
        //        //Debug.Log(skill);
        //        TimeUpDarts = CDTDarts.IsTimeUp;
        //        if (TimeUpDarts || flagDarts)
        //        {
        //            //Debug.Log("按下");
        //            innerCircleTrans.position = eventData.position;
        //            if (onJoystickDownEvent != null)
        //                onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 2:
        //        //  Debug.Log(skill);
        //        TimeUpDodge = CDTDodge.IsTimeUp;
        //        if (TimeUpDodge || flagDodge)
        //        {
        //            //Debug.Log("按下");
        //            innerCircleTrans.position = eventData.position;
        //            if (onJoystickDownEvent != null)
        //                onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 3:
        //        //  Debug.Log(skill);
        //        TimeUp = CDT.IsTimeUp;
        //        if (TimeUp || flag)
        //        {
        //            //Debug.Log("按下");
        //            innerCircleTrans.position = eventData.position;
        //            if (onJoystickDownEvent != null)
        //                onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 4:
        //        //  Debug.Log(skill);
        //        TimeUpShengLong = CDTShengLong.IsTimeUp;
        //        if (TimeUpShengLong || flagShengLong)
        //        {
        //            //Debug.Log("按下");
        //            innerCircleTrans.position = eventData.position;
        //            if (onJoystickDownEvent != null)
        //                onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //}


        TimeUp = CDT.IsTimeUp;
        FlagSate = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state == (int)State.normal;
        //Debug.Log("State");
        //Debug.Log(FlagSate);
        if (skill == 6 || skill == 7)
        {

        }
        else if ((skill == 0 || skill == 4 || skill == 5) && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
        {
            if (LevelNum > 0 && (TimeUp || flag) && FlagSate)
            {
                innerCircleTrans.position = eventData.position;
                if (onJoystickDownEvent != null)
                    onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
            }
        }
        else
        {
            if ((TimeUp || flag) && FlagSate)
            {
                //Debug.Log("按下");
                innerCircleTrans.position = eventData.position;
                if (onJoystickDownEvent != null)
                    onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
                //Debug.Log("State111111");
            }
        }

    }

    /// <summary>
    /// 抬起
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        //switch (skill)
        //{
        //    case 1:
        //        if (TimeUpDarts || flagDarts)
        //        {
        //            //Debug.Log("抬起");
        //            innerCircleTrans.localPosition = Vector3.zero;
        //            if (onJoystickUpEvent != null)
        //                onJoystickUpEvent();
        //            flagDarts = false;
        //            CDTDarts.Start();
        //        }
        //        return;
        //    case 2:
        //        if (TimeUpDodge || flagDodge)
        //        {
        //            //Debug.Log("抬起");
        //            innerCircleTrans.localPosition = Vector3.zero;
        //            if (onJoystickUpEvent != null)
        //                onJoystickUpEvent();
        //            flagDodge = false;
        //            CDTDodge.Start();
        //        }
        //        return;
        //    case 3:
        //        if (TimeUp || flag)
        //        {
        //            //Debug.Log("抬起");
        //            innerCircleTrans.localPosition = Vector3.zero;
        //            if (onJoystickUpEvent != null)
        //                onJoystickUpEvent();
        //            flag = false;
        //            CDT.Start();
        //        }
        //        return;
        //    case 4:
        //        if (TimeUpShengLong || flagShengLong)
        //        {
        //            //Debug.Log("抬起");
        //            innerCircleTrans.localPosition = Vector3.zero;
        //            if (onJoystickUpEvent != null)
        //                onJoystickUpEvent();
        //            flagShengLong = false;
        //            CDTShengLong.Start();
        //        }
        //        return;
        //}
        //Debug.Log("State");
        //Debug.Log(FlagSate);

        if (skill == 6)
        {
            if ((TimeUp || flag) && FlagSate)
            {
                //Debug.Log("抬起");
                BtnClickDodge();
                flag = false;
                CDT.Start();
            }
        }
        else if (skill == 7)
        {
            if ((TimeUp || flag) && FlagSate)
            {

                //Debug.Log("抬起");
                BtnClickRound();
                flag = false;
                CDT.Start();
            }
        }
        else if ((skill == 0 || skill == 4 || skill == 5) && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
        {
            if (LevelNum > 0 && (TimeUp || flag) && FlagSate)
            {
                innerCircleTrans.localPosition = Vector3.zero;
                if (onJoystickUpEvent != null)
                    onJoystickUpEvent();
                if (!Flaglevel)
                    CDLeve1One.Start();
                Flaglevel = true;
                FlagOne = true;
                --LevelNum;

            }
        }
        else
        {
            if ((TimeUp || flag) && FlagSate)
            {
                //Debug.Log("抬起");
                innerCircleTrans.localPosition = Vector3.zero;
                if (onJoystickUpEvent != null)
                    onJoystickUpEvent();
                flag = false;
                CDT.Start();
            }
        }
    }

    /// <summary>
    /// 滑动
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {

        Vector2 touchPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, uiCam, out touchPoint);
        //Debug.Log("event: " + eventData.position + " huadong: " + touchPoint + " out :" + outerCircleStartWorldPos);
        Vector2 touchPos = eventData.position - outerCircleStartWorldPos;
        //switch (skill)
        //{
        //    case 1:
        //        if (TimeUpDarts || flagDarts)
        //        {
        //            if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
        //                innerCircleTrans.localPosition = touchPos;
        //            else
        //                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

        //            if (onJoystickMoveEvent != null)
        //                onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 2:
        //        if (TimeUpDodge || flagDodge)
        //        {
        //            if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
        //                innerCircleTrans.localPosition = touchPos;
        //            else
        //                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

        //            if (onJoystickMoveEvent != null)
        //                onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 3:
        //        if (TimeUp || flag)
        //        {
        //            if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
        //                innerCircleTrans.localPosition = touchPos;
        //            else
        //                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

        //            if (onJoystickMoveEvent != null)
        //                onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //    case 4:
        //        if (TimeUpShengLong || flagShengLong)
        //        {
        //            if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
        //                innerCircleTrans.localPosition = touchPos;
        //            else
        //                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

        //            if (onJoystickMoveEvent != null)
        //                onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
        //        }
        //        return;
        //}
        if (skill == 6 || skill == 7)
        {

        }
        else if ((skill == 0 || skill == 4 || skill == 5) && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
        {
            if (LevelNum > 0 && (TimeUp || flag) && FlagSate)
            {
                if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
                    innerCircleTrans.localPosition = touchPos;
                else
                    innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

                if (onJoystickMoveEvent != null)
                    onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
            }
        }
        else
        {
            if ((TimeUp || flag) && FlagSate)
            {
                if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
                    innerCircleTrans.localPosition = touchPos;
                else
                    innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

                if (onJoystickMoveEvent != null)
                    onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
            }
        }

    }
    private void Update()
    {
        //switch (skill)
        //{
        //    case 1:

        //        if (!flagDarts)
        //            UpdateImageDarts();
        //        return;
        //    case 2:
        //        if (!flagDodge)
        //            UpdateImageDodge();
        //        return;
        //    case 3:
        //        if (!flag)
        //            UpdateImage();
        //        return;
        //    case 4:
        //        if (!flagShengLong)
        //            UpdateImageShengLong();
        //        return;
        //}
        if (Flaglevel && CDLeve1One.IsTimeUp)
        {
            if (LevelNum < 1)
            {
                CDLeve1One.Start();
                LevelNum++;
            }
            else if (LevelNum == 1)
            {
                LevelNum++;
            }
            else if (LevelNum == 2)
            {
                Flaglevel = false;
            }
        }
        if (FlagOne && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
            UpdateImageLevel();

        //Debug.Log("1, pos: " + outerCircleStartWorldPos);

        if (!flag && !FlagOne)
            UpdateImage();
        //if (FlagOne)
        //{
        //    if (CDLeve1One.IsTimeUp)
        //    {
        //        Debug.Log("UpdateLevelOne");
        //        LevelNum += 1;
        //        FlagOne = false;
        //    }
        //}
        //if (FlagTwo)
        //{
        //    if (CDLeve1Two.IsTimeUp)
        //    {
        //         LevelNum += 1;
        //        FlagTwo = false;
        //    }
        //}

        if ((skill == 0 || skill == 4 || skill == 5) && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
        {
            if (LevelNum > 0)
            {
                if (ShowLevelNum != null)
                    ShowLevelNum.text = LevelNum.ToString();
            }
            else
                ShowLevelNum.text = "";

        }
        else if ((skill == 0 || skill == 4 || skill == 5) && GameObject.FindGameObjectWithTag("Player").GetComponent<RandomSkillMsg>().m_randomSkillLevel != 3)
        {
            ShowLevelNum.text = "";
            FlagOne = false;
        }

        //Debug.Log(CDLeve1One.CurrentTime);
    }

    //public void UpdateImageDarts()
    //{
    //    TimePercentDarts = CDTDarts.GetPercent();

    //    coolingImageDarts.fillAmount = 1 - TimePercentDarts;
    //    if (coolingImageDarts.fillAmount != 0)
    //    {
    //        coolingImageDarts.raycastTarget = true;
    //    }
    //    else
    //    {
    //        coolingImageDarts.raycastTarget = false;
    //        coolingImageDarts.fillAmount = 1;

    //    }

    //}
    //public void UpdateImageDodge()
    //{
    //    TimePercentDodge = CDTDodge.GetPercent();
    //    //Debug.Log(TimePercentDodge);
    //    coolingImageDodge.fillAmount = 1 - TimePercentDodge;
    //    if (coolingImageDodge.fillAmount != 0)
    //    {
    //        coolingImageDodge.raycastTarget = true;
    //    }
    //    else
    //    {
    //        coolingImageDodge.raycastTarget = false;
    //        coolingImageDodge.fillAmount = 1;
    //    }

    //}
    public void UpdateImage()
    {
        TimePercent = CDT.GetPercent();

        coolingImage.fillAmount = TimePercent;
        if (coolingImage.fillAmount != 0)
        {
            coolingImage.raycastTarget = true;
        }
        else
        {
            coolingImage.raycastTarget = false;
            coolingImage.fillAmount = 1;
        }

        //}
        //public void UpdateImageShengLong()
        //{
        //    TimePercent = CDTShengLong.GetPercent();

        //    coolingImageShengLong.fillAmount = 1 - TimePercent;
        //    if (coolingImageShengLong.fillAmount != 0)
        //    {
        //        coolingImageShengLong.raycastTarget = true;
        //    }
        //    else
        //    {
        //        coolingImageShengLong.raycastTarget = false;
        //        coolingImageShengLong.fillAmount = 1;
        //    }

        //}
    }
    public void UpdateImageLevel()
    {


        coolingImage.fillAmount = CDLeve1One.GetPercent();
        //Debug.Log(coolingImage.fillAmount);
        if (coolingImage.fillAmount != 0)
        {
            coolingImage.raycastTarget = true;
        }
        else
        {
            coolingImage.raycastTarget = false;
            coolingImage.fillAmount = 1;
        }

        //}
        //public void UpdateImageShengLong()
        //{
        //    TimePercent = CDTShengLong.GetPercent();

        //    coolingImageShengLong.fillAmount = 1 - TimePercent;
        //    if (coolingImageShengLong.fillAmount != 0)
        //    {
        //        coolingImageShengLong.raycastTarget = true;
        //    }
        //    else
        //    {
        //        coolingImageShengLong.raycastTarget = false;
        //        coolingImageShengLong.fillAmount = 1;
        //    }

        //}
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
    public void BtnClickDodge()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state != (int)State.normal)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        NotificationCenter.Instance.PushEvent(NotificationType.Request_Hide, null);

        //StartCoroutine(Wait(dodgeTime));
    }
    IEnumerator WaitCircly()
    {
        Debug.Log("WaitCircly");
        yield return new WaitForSeconds(1);
        elementGo.gameObject.SetActive(false);
    }


    public float roundTime = 0.5f;

    IEnumerator WaitRound(float waitTime)
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
    public void BtnClickRound()
    {

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state != (int)State.normal)
            return;
        Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().state);


        StartCoroutine(WaitRound(roundTime));
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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

        flag = false;
        StartCoroutine(WaitCircly());
    }
}
namespace CN
{
    /// <summary>
    /// 倒计时器。
    /// </summary>
    public sealed class CountDownTimer
    {
        public bool IsAutoCycle { get; private set; }                   // 是否自动循环（小于等于0后重置）
        public bool IsStoped { get; private set; }                      // 是否是否暂停了
        public float CurrentTime { get { return UpdateCurrentTime(); } }// 当前时间
        public bool IsTimeUp { get { return CurrentTime <= 0; } }       // 是否时间到
        public float Duration { get; private set; }                     // 计时时间长度

        private float lastTime;                                         // 上一次更新的时间
        private int lastUpdateFrame;                                    // 上一次更新倒计时的帧数（避免一帧多次更新计时）
        private float currentTime;                                      // 当前计时器剩余时间

        /// <summary>
        /// 构造倒计时器
        /// </summary>
        /// <param name="duration">起始时间</param>
        /// <param name="autocycle">是否自动循环</param>
        public CountDownTimer(float duration, bool autocycle = false, bool autoStart = true)
        {
            IsStoped = true;
            Duration = Mathf.Max(0f, duration);
            IsAutoCycle = autocycle;
            Reset(duration, !autoStart);
        }

        /// <summary>
        /// 更新计时器时间
        /// </summary>
        /// <returns>返回剩余时间</returns>
        private float UpdateCurrentTime()
        {
            if (IsStoped || lastUpdateFrame == Time.frameCount)         // 暂停了或已经这一帧更新过了，直接返回
                return currentTime;
            if (currentTime <= 0)                                       // 小于等于0直接返回，如果循环那就重置时间
            {
                if (IsAutoCycle)
                    Reset(Duration, false);
                return currentTime;
            }
            currentTime -= Time.time - lastTime;
            UpdateLastTimeInfo();
            return currentTime;
        }

        /// <summary>
        /// 更新时间标记信息
        /// </summary>
        private void UpdateLastTimeInfo()
        {
            lastTime = Time.time;
            lastUpdateFrame = Time.frameCount;
        }

        /// <summary>
        /// 开始计时，取消暂停状态
        /// </summary>
        public void Start()
        {
            Reset(Duration, false);
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <param name="duration">持续时间</param>
        /// <param name="isStoped">是否暂停</param>
        public void Reset(float duration, bool isStoped = false)
        {
            UpdateLastTimeInfo();
            Duration = Mathf.Max(0f, duration);
            currentTime = Duration;
            IsStoped = isStoped;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            UpdateCurrentTime();    // 暂停前先更新一遍
            IsStoped = true;
        }

        /// <summary>
        /// 继续（取消暂停）
        /// </summary>
        public void Continue()
        {
            UpdateLastTimeInfo();   // 继续前先更新当前时间信息
            IsStoped = false;
        }

        /// <summary>
        /// 终止，暂停且设置当前值为0
        /// </summary>
        public void End()
        {
            IsStoped = true;
            currentTime = 0f;
        }

        /// <summary>
        /// 获取倒计时完成率（0为没开始计时，1为计时结束）
        /// </summary>
        /// <returns></returns>
        public float GetPercent()
        {
            UpdateCurrentTime();
            if (currentTime <= 0 || Duration <= 0)
                return 1f;
            return 1f - currentTime / Duration;
        }

    }
}