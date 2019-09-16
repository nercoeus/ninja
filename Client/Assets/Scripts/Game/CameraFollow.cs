//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class CameraFollow : MonoBehaviour
//{

//    [SerializeField]
//    private GameObject player;
//    private float cameraX;
//    private float cameraZ;

//    public float y;
//    public float z;



//    private void LateUpdate()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        if (player)
//        {
//            cameraX = player.transform.position.x;
//            cameraZ = player.transform.position.z;
//            this.transform.position = new Vector3(cameraX, y, cameraZ + z);
//        }
//    }

//}
//*/
//public class CameraFollow : MonoBehaviour
//{ 
//    public Transform target = null;     // 目标玩家
//    [SerializeField]
//    [Range(0, 360)]
//    float horizontalAngle = 270f;      // 水平角度
//    [SerializeField]
//    [Range(0, 20)]
//    float initialHeight = 2f;    // 人物在视野内屏幕中的位置设置

//    [SerializeField]
//    [Range(10, 90)]
//    float initialAngle = 40f;   // 初始俯视角度
//    [SerializeField]
//    [Range(10, 90)]
//    float maxAngle = 50f;     // 最高俯视角度
//    [SerializeField]
//    [Range(10, 90)]
//    float minAngle = 35f;     // 最低俯视角度

//    float initialDistance;    // 初始化相机与玩家的距离 根据角度计算
//    [SerializeField]
//    [Range(1, 100)]
//    float maxDistance = 20f;        // 相机距离玩家最大距离
//    [SerializeField]
//    [Range(1, 100)]
//    float minDistance = 5f;        // 相机距离玩家最小距离

//    [SerializeField]
//    [Range(1, 100)]
//    float zoomSpeed = 50;       // 缩放速度

//    [SerializeField]
//    [Range(1f, 200)]
//    float swipeSpeed = 50;      // 左右滑动速度

//    float scrollWheel;        // 记录滚轮数值
//    float tempAngle;          // 临时存储摄像机的初始角度
//    Vector3 tempVector = new Vector3();

//    void Start()
//    {

//        InitCamera();
//    }

//    void Update()
//    {
//        if (GameObject.FindGameObjectWithTag("Player").transform != null)
//        {
//            target = GameObject.FindGameObjectWithTag("Player").transform;
//        }
//        ZoomCamera();
//        SwipeScreen();
//    }

//    void LateUpdate()
//    {
//        FollowPlayer();
//        RotateCamera();
//    }

//    /// <summary>
//    /// 初始化 相机与玩家距离
//    /// </summary>
//    void InitCamera()
//    {
//        Debug.Log("init camera");
//        tempAngle = initialAngle;

//        initialDistance = Mathf.Sqrt((initialAngle - minAngle) / Calculate()) + minDistance;

//        initialDistance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
//        //Debug.Log(initialDistance);

//    }

//    /// <summary>
//    /// 相机跟随玩家
//    /// </summary>
//    void FollowPlayer()
//    {
//        float upRidus = Mathf.Deg2Rad * initialAngle;
//        float flatRidus = Mathf.Deg2Rad * horizontalAngle;

//        float x = initialDistance * Mathf.Cos(upRidus) * Mathf.Cos(flatRidus);
//        float z = initialDistance * Mathf.Cos(upRidus) * Mathf.Sin(flatRidus);
//        float y = initialDistance * Mathf.Sin(upRidus);
//        //Debug.Log(x + " " + " " + y + " " + z);
//        transform.position = Vector3.zero;
//        tempVector.Set(x, y, z);
//        tempVector = tempVector + target.position;
//        //Debug.Log(tempVector);
//        transform.position = tempVector;
//        tempVector.Set(target.position.x, target.position.y + initialHeight, target.position.z);

//        transform.LookAt(tempVector);
//    }

//    /// <summary>
//    /// 缩放相机与玩家距离
//    /// </summary>
//    void ZoomCamera()
//    {
//        scrollWheel = GetZoomValue();
//        if (scrollWheel != 0)
//        {
//            tempAngle = initialAngle - scrollWheel * 2 * (maxAngle - minAngle);
//            tempAngle = Mathf.Clamp(tempAngle, minAngle, maxAngle);
//        }

//        if (tempAngle != initialAngle)
//        {
//            initialAngle = Mathf.Lerp(initialAngle, tempAngle, Time.deltaTime * 10);

//            initialDistance = Mathf.Sqrt((initialAngle - minAngle) / Calculate()) + minDistance;

//            initialDistance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
//        }
//    }

//    float Calculate()
//    {
//        float dis = maxDistance - minDistance;
//        float ang = maxAngle - minAngle;
//        float line = ang / (dis * dis);
//        return line;
//    }

//    bool isMousePress = false;
//    Vector2 oldMousePos;
//    Vector2 newMousePos;
//    Vector2 mousePosOffset;
//    /// <summary>
//    /// 滑动屏幕 旋转相机和缩放视野
//    /// </summary>
//    public void SwipeScreen()
//    {
//        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
//        {
//            oldMousePos = Vector2.zero;
//            isMousePress = true;
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            mousePosOffset = Vector2.zero;
//            isMousePress = false;
//        }
//        if (!isMousePress)
//            return;

//        newMousePos = Input.mousePosition;
//        if (oldMousePos != Vector2.zero)
//        {
//            mousePosOffset = newMousePos - oldMousePos;
//        }
//        oldMousePos = newMousePos;
//    }

//    /// <summary>
//    /// 获取缩放视野数值  1.鼠标滚轮 2.屏幕上下滑动
//    /// </summary>
//    /// <returns></returns>
//    float GetZoomValue()
//    {
//        float zoomValue = 0;
//        // 使用鼠标滚轮
//        if (Input.GetAxis("Mouse ScrollWheel") != 0)
//        {
//            zoomValue = Input.GetAxis("Mouse ScrollWheel");
//        }
//        else if (mousePosOffset != Vector2.zero)
//        {
//            zoomValue = mousePosOffset.y * Time.deltaTime * zoomSpeed * 0.01f;
//        }

//        return zoomValue;
//    }

//    float xVelocity = 0;
//    /// <summary>
//    /// 旋转相机
//    /// </summary>
//    void RotateCamera()
//    {
//        horizontalAngle = Mathf.SmoothDamp(horizontalAngle, horizontalAngle + mousePosOffset.x * Time.deltaTime * swipeSpeed, ref xVelocity, 0.1f);
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance = null;

    private Camera mainCamera;
    //震动标志位
    public bool isShakeCamera = false;
    //震动幅度
    public float shakeLevel = 3f;
    //震动时间
    public float setShakeTime = 0.1f;
    //震动的FPS
    public float shakeFps = 45f;

    private float fps;
    private float shakeTime = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;

    void Awake()
    {
        //获取Camera组件
        mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }

        shakeTime = setShakeTime;
        fps = shakeFps;
        frameTime = 0.03f;
        shakeDelta = 0.005f;
    }

    void Update()
    {
        if (isShakeCamera)
        {
            if (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                if (shakeTime <= 0)
                {
                    mainCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                    isShakeCamera = false;
                    shakeTime = setShakeTime;
                    fps = shakeFps;
                    frameTime = 0.03f;
                    shakeDelta = 0.005f;
                }
                else
                {
                    frameTime += Time.deltaTime;
                    if (frameTime > 1.0 / fps)
                    {
                        frameTime = 0;
                        mainCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value),
                            shakeDelta * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
                    }
                }
            }
        }
    }

    public void Shake()
    {
        isShakeCamera = true;
    }

}