using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SkillAreaType
{
    OuterCircle = 0,
    OuterCircle_InnerCube = 1,
    OuterCircle_InnerSector = 2,
    OuterCircle_InnerCircle = 3,
}

public class SkillArea : MonoBehaviour {

   // public GameObject dartsPrefab;
    public Transform positions;
    public float dartsTime = 0.5f;
    public float blinkTime = 0.5f;
    public float shenglongTime = 1.0f;

    enum SKillAreaElement
    {
        OuterCircle,    // 外圆
        InnerCircle,    // 内圆
        Cube,           // 矩形 
        Sector60,        // 扇形
        Sector120,        // 扇形
    }

    SkillJoystick joystick;

    public GameObject player;

    public Transform dartsPosition;

    public SkillAreaType areaType;      // 设置指示器类型

    Vector3 deltaVec;

    float outerRadius = 6;      // 外圆半径
    float innerRadius = 2f;     // 内圆半径
    float cubeWidth = 2f;       // 矩形宽度 （矩形长度使用的外圆半径）
    int angle = 60;             // 扇形角度

    bool isPressed = false;
    string att = "Light_Attk_1";

    string path = "Effect/Prefabs/Hero_skillarea/";  // 路径
    string circle = "quan_hero";    // 圆形
    string cube = "chang_hero";     // 矩形
    string sector60 = "shan_hero_60";    // 扇形60度
    string sector120 = "shan_hero_120";    // 扇形120度

    Dictionary<SKillAreaElement, string> allElementPath;
    Dictionary<SKillAreaElement, Transform> allElementTrans;


    public float angle_360(Vector3 from)
    {
        float angle = Mathf.Atan2(from.x, from.z) * Mathf.Rad2Deg;
        return angle;

    }

    // Use this for initialization
    void Start()
    {

        
        joystick = GetComponent<SkillJoystick>();

        joystick.onJoystickDownEvent += OnJoystickDownEvent;
        joystick.onJoystickMoveEvent += OnJoystickMoveEvent;
        joystick.onJoystickUpEvent += OnJoystickUpEvent;

        InitSkillAreaType();
    }

    void OnDestroy()
    {
        joystick.onJoystickDownEvent -= OnJoystickDownEvent;
        joystick.onJoystickMoveEvent -= OnJoystickMoveEvent;
        joystick.onJoystickUpEvent -= OnJoystickUpEvent;
    }

    void InitSkillAreaType()
    {
        allElementPath = new Dictionary<SKillAreaElement, string>();
        allElementPath.Add(SKillAreaElement.OuterCircle, circle);
        allElementPath.Add(SKillAreaElement.InnerCircle, circle);
        allElementPath.Add(SKillAreaElement.Cube, cube);
        allElementPath.Add(SKillAreaElement.Sector60, sector60);
        allElementPath.Add(SKillAreaElement.Sector120, sector120);

        allElementTrans = new Dictionary<SKillAreaElement, Transform>();
        allElementTrans.Add(SKillAreaElement.OuterCircle, null);
        allElementTrans.Add(SKillAreaElement.InnerCircle, null);
        allElementTrans.Add(SKillAreaElement.Cube, null);
        allElementTrans.Add(SKillAreaElement.Sector60, null);
        allElementTrans.Add(SKillAreaElement.Sector120, null);
    }


    void OnJoystickDownEvent(Vector2 deltaVec)
    {
        if (GetComponent<SkillJoystick>().skill == 4 || GetComponent<SkillJoystick>().skill == 5)
        {
            if (player.GetComponent<RandomSkillMsg>().m_randomSkillLevel == 2 || player.GetComponent<RandomSkillMsg>().m_randomSkillLevel == 3)
            {
                outerRadius = 8f;
                innerRadius = 3f;
            }
            else
            {
                outerRadius = 6f;
                innerRadius = 2f;
            }
        }
        else
        {
            outerRadius = 6f;
            innerRadius = 2f;
        }
        isPressed = true;
        this.deltaVec = new Vector3(deltaVec.x, 0, deltaVec.y);
        CreateSkillArea();
    }

    IEnumerator Wait(float waitTime)
    {
        player.GetComponent<Player>().state = (int)State.play;
        yield return new WaitForSeconds(waitTime);
        if(player.GetComponent<Player>().state == (int)State.play)
            player.GetComponent<Player>().state = (int)State.normal;
        //print("WaitAndPrint " + Time.time);

    }

    void OnJoystickUpEvent()
    {

        //修复抬起遥杆死亡时会持续动作
        if (player.GetComponent<Player>().state == (int)State.normal)
        {
            isPressed = false;
            HideElements();
            //Debug.Log("释放技能" + (int)areaType);
            switch (areaType)
            {
                //case SkillAreaType.OuterCircle:   //外圆技能释放（周身AOE）暂无

                case SkillAreaType.OuterCircle_InnerCube:    //飞镖释放
                    player.GetComponent<Animator>().Play(att);
                    int level = player.GetComponent<RandomSkillMsg>().returnSkillLevel(SkillType.DartAttack);
                    if (level != 0)                      //i == 0 表示当前按下的技能不是现在随机的技能，这种情况正常状况下不可能出现
                    {
                        ReqDartMsg rdmsg = new ReqDartMsg();
                        rdmsg.position = deltaVec;
                        rdmsg.level = level;
                        //Debug.Log("发射飞镖 飞镖level is " + rdmsg.level);
                        NotificationCenter.Instance.PushEvent(NotificationType.Request_Darts, rdmsg);
                        StartCoroutine(Wait(dartsTime));
                    }

                    return;
                case SkillAreaType.OuterCircle_InnerSector:   //范围斩击释放
                    player.GetComponent<Animator>().Play(att);
                    player.transform.LookAt(GetCubeSectorLookAt());
                    Debug.Log("斩击");
                    return;
                case SkillAreaType.OuterCircle_InnerCircle:   //闪现
                    {
                        int sk = GetComponent<SkillJoystick>().skill;
                        if (sk == 3)
                        {
                            Debug.Log("闪现");
                            Vector4 data;
                            Vector3 temp = GetCirclePosition(outerRadius);
                            data.x = temp.x;
                            data.y = temp.y;
                            data.z = temp.z;
                            data.w = angle_360(deltaVec);
                            NotificationCenter.Instance.PushEvent(NotificationType.Request_Blink, data);
                            StartCoroutine(Wait(blinkTime));
                        }
                        else if (sk == 4)
                        {
                            int level2 = player.GetComponent<RandomSkillMsg>().returnSkillLevel(SkillType.ShenglongAttack);
                            if (level2 != 0)
                            {
                                Debug.Log("升龙斩");
                                Vector4 data;
                                Vector3 temp = GetCirclePosition(outerRadius);
                                data.x = temp.x;
                                data.y = temp.y;
                                data.z = temp.z;
                                data.w = angle_360(deltaVec);
                                player.transform.LookAt(GetCubeSectorLookAt());
                                ReqDartMsg rdmsg2 = new ReqDartMsg();
                                rdmsg2.data = data;
                                rdmsg2.level = level2;
                                NotificationCenter.Instance.PushEvent(NotificationType.Request_ShengLong, rdmsg2);
                                StartCoroutine(Wait(shenglongTime));
                            }
                        }
                        else if (sk == 5)
                        {
                            int level2 = player.GetComponent<RandomSkillMsg>().returnSkillLevel(SkillType.CaoZhuiAttack);
                            if (level2 != 0)
                            {
                                Debug.Log("草锥剑");
                                Vector4 data;
                                Vector3 temp = GetCirclePosition(outerRadius);
                                data.x = temp.x;
                                data.y = temp.y;
                                data.z = temp.z;
                                data.w = angle_360(deltaVec);
                                player.transform.LookAt(GetCubeSectorLookAt());
                                ReqDartMsg rdmsg2 = new ReqDartMsg();
                                rdmsg2.data = data;
                                rdmsg2.level = level2;
                                NotificationCenter.Instance.PushEvent(NotificationType.Request_CaoZhui, rdmsg2);
                            }

                        }
                        return;
                    }
                default:
                    return;
            }
        }
        else
        {
            isPressed = false;
            HideElements();
            return;
        }
    }

    void OnJoystickMoveEvent(Vector2 deltaVec)
    {
        this.deltaVec = new Vector3(deltaVec.x, 0, deltaVec.y);
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if(isPressed)
            UpdateElement();
    }

    /// <summary>
    /// 创建技能区域展示
    /// </summary>
    void CreateSkillArea()
    {
        switch (areaType)
        {
            case SkillAreaType.OuterCircle:
                CreateElement(SKillAreaElement.OuterCircle);
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                CreateElement(SKillAreaElement.OuterCircle);
                CreateElement(SKillAreaElement.Cube);
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                CreateElement(SKillAreaElement.OuterCircle);
                switch (angle)
                {
                    case 60:
                        CreateElement(SKillAreaElement.Sector60);
                        break;
                    case 120:
                        CreateElement(SKillAreaElement.Sector120);
                        break;
                    default:
                        break;
                }
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                CreateElement(SKillAreaElement.OuterCircle);
                CreateElement(SKillAreaElement.InnerCircle);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 创建技能区域展示元素
    /// </summary>
    /// <param name="element"></param>
	void CreateElement(SKillAreaElement element)
    {
        Transform elementTrans = GetElement(element);
        if (elementTrans == null) return;
        allElementTrans[element] = elementTrans;
        switch (element)
        {
            case SKillAreaElement.OuterCircle:
                elementTrans.localScale = new Vector3(outerRadius * 2, 1, outerRadius * 2) / player.transform.localScale.x;
                elementTrans.gameObject.SetActive(true);
                break;
            case SKillAreaElement.InnerCircle:
                elementTrans.localScale = new Vector3(innerRadius * 2, 1, innerRadius * 2) / player.transform.localScale.x;
                break;
            case SKillAreaElement.Cube:
                elementTrans.localScale = new Vector3(cubeWidth, 1, outerRadius) / player.transform.localScale.x;
                break;
            case SKillAreaElement.Sector60:
            case SKillAreaElement.Sector120:
                elementTrans.localScale = new Vector3(outerRadius, 1, outerRadius) / player.transform.localScale.x;
                break;
            default:
                break;
        }
    }

    Transform elementParent;
    /// <summary>
    /// 获取元素的父对象
    /// </summary>
    /// <returns></returns>
    Transform GetParent()
    {
        if (elementParent == null)
        {
            elementParent = player.transform.Find("SkillArea");
        }
        if (elementParent == null)
        {
            elementParent = new GameObject("SkillArea").transform;
            elementParent.parent = player.transform;
            elementParent.localEulerAngles = Vector3.zero;
            elementParent.localPosition = Vector3.zero;
            elementParent.localScale = Vector3.one;
        }
        return elementParent;
    }

    /// <summary>
    /// 获取元素物体
    /// </summary>
    Transform GetElement(SKillAreaElement element)
    {
        if (player == null) return null;
        string name = element.ToString();
        Transform parent = GetParent();
        Transform elementTrans = parent.Find(name);
        if (elementTrans == null)
        {
            GameObject elementGo = Instantiate(Resources.Load(path + allElementPath[element])) as GameObject;
            elementGo.transform.parent = parent;
            elementGo.gameObject.SetActive(false);
            elementGo.name = name;
            elementTrans = elementGo.transform;
        }
        elementTrans.localEulerAngles = Vector3.zero;
        elementTrans.localPosition = Vector3.zero;
        elementTrans.localScale = Vector3.one;
        return elementTrans;
    }

    /// <summary>
    /// 隐藏所有元素
    /// </summary>
    void HideElements()
    {
        if (player == null) return;
        Transform parent = GetParent();
        for (int i = 0, length = parent.childCount; i < length; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 隐藏指定元素
    /// </summary>
    /// <param name="element"></param>
    void HideElement(SKillAreaElement element)
    {
        if (player == null) return;
        Transform parent = GetParent();
        Transform elementTrans = parent.Find(element.ToString());
        if (elementTrans != null)
            elementTrans.gameObject.SetActive(false);
    }

    /// <summary>
    /// 每帧更新元素
    /// </summary>
    void UpdateElement()
    {
        switch (areaType)
        {
            case SkillAreaType.OuterCircle:
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                UpdateElementPosition(SKillAreaElement.Cube);
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                switch (angle)
                {
                    case 60:
                        UpdateElementPosition(SKillAreaElement.Sector60);
                        break;
                    case 120:
                        UpdateElementPosition(SKillAreaElement.Sector120);
                        break;
                    default:
                        break;
                }
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                UpdateElementPosition(SKillAreaElement.InnerCircle);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 每帧更新元素位置
    /// </summary>
    /// <param name="element"></param>
    void UpdateElementPosition(SKillAreaElement element)
    {
        if (allElementTrans[element] == null)
            return;
        switch (element)
        {
            case SKillAreaElement.OuterCircle:
                break;
            case SKillAreaElement.InnerCircle:
                allElementTrans[element].transform.position = GetCirclePosition(outerRadius);
                break;
            case SKillAreaElement.Cube:
            case SKillAreaElement.Sector60:
            case SKillAreaElement.Sector120:
                allElementTrans[element].transform.LookAt(GetCubeSectorLookAt());
                //allElementTrans[element].transform.LookAt(player.transform.position + deltaVec);
                break;
            default:
                break;
        }
        if (!allElementTrans[element].gameObject.activeSelf)
            allElementTrans[element].gameObject.SetActive(true);
    }

    /// <summary>
    /// 获取InnerCircle元素位置
    /// </summary>
    /// <returns></returns>
    Vector3 GetCirclePosition(float dist)
    {
        if (player == null) return Vector3.zero;

        Vector3 targetDir = deltaVec * dist;

        //float y = Camera.main.transform.rotation.eulerAngles.y;
        //targetDir = Quaternion.Euler(0, y, 0) * targetDir;

        return targetDir + player.transform.position;
    }

    /// <summary>
    /// 获取Cube、Sector元素朝向
    /// </summary>
    /// <returns></returns>
    Vector3 GetCubeSectorLookAt()
    {
        if (player == null) return Vector3.zero;
        
        Vector3 targetDir = deltaVec;

        //float y = Camera.main.transform.rotation.eulerAngles.y;
        //targetDir = Quaternion.Euler(0, y, 0) * targetDir;
        return targetDir + player.transform.position;
    }
}
