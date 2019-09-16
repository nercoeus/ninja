using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// 怪物位置指示器
/// </summary>
public class EnemyLookAt : MonoBehaviour
{
    //public List<Transform> monsterList;

    public GameObject imagePerfab;

    public static EnemyLookAt Instance = null;

    public Transform player;
    public int disFromPlayer = 200;//指示箭头与玩家距离,以像素算
    public Camera uiCam;
    public RectTransform _parent;
    
    Dictionary<string, IndicatorMonster> outScreenMonsters = new Dictionary<string, IndicatorMonster>();//在屏幕外的怪物列表
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        uiCam = GameObject.Find("Camera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        /*
        for (int i = 0, size = transform.childCount; i < size; i++)
        {
            Transform child = transform.GetChild(i);
            indicatorList.Add(child);
            child.name = "-1";
        }
        */
    }

    void LateUpdate()
    {
        foreach (KeyValuePair<string, IndicatorMonster> kv in outScreenMonsters)
        {
            if(kv.Value.monst!=null)
                kv.Value.Update();
            else
            {
                Disappear(kv.Value.Image1.gameObject);
            }
        }
    }



    
    public void Add(Transform monst, int number, string ID)
    {

        IndicatorMonster monstInfo = new IndicatorMonster();
        monstInfo.monst = monst;

        
        monstInfo.Image1 = GameObject.Find("Canvas2/Image"+number.ToString()).GetComponent<RectTransform>(); 
        monstInfo.player = player;
        monstInfo.uiCam = uiCam;
        monstInfo.parent = _parent;
        Debug.Log(monstInfo.parent.name);
        monstInfo.disFromPlayer = disFromPlayer;

        outScreenMonsters.Add(ID , monstInfo);
        Disappear(monstInfo.Image1.gameObject);
    }

    public void Disappear(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    public void Appear(GameObject enemy)
    {
        enemy.SetActive(true);
    }

    public void DisappearById(string enemyID)
    {
        Disappear(outScreenMonsters[enemyID].Image1.gameObject);
    }

    public void AppearById(string enemyID)
    {
        Appear(outScreenMonsters[enemyID].Image1.gameObject);
    }


}

internal class IndicatorMonster
{
    public RectTransform Image1;
    public Transform monst;
    public Transform player;
    public Camera uiCam;
    public RectTransform parent;
    public int disFromPlayer;

    public void Update()
    {
        //Debug.Log("monster pos: " + monst.position + " player pos: " + player.position);
        Vector3 indicatorPoint = (monst.position - player.position).normalized * 5 + new Vector3(player.position.x, player.position.y + 1.0f, player.position.z);
        //Debug.Log(monst.position - player.position);
        //Debug.Log("indicatorPoint pos: " + indicatorPoint);
        Vector3 indicatorScnPoint = Camera.main.WorldToScreenPoint(indicatorPoint);
        //Debug.Log("indicatorScnPoint pos: " + indicatorScnPoint);
        Vector3 playerPos = Camera.main.WorldToScreenPoint(new Vector3(player.position.x, indicatorPoint.y, player.position.z));
        //Debug.Log("playerPos pos: " +playerPos);
        //Debug.Log("cam pos : " + uiCam.transform.position);
        Vector2 indicatorUIPoint;
        Vector2 playerUIPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, indicatorScnPoint, uiCam, out indicatorUIPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, playerPos, uiCam, out playerUIPos);
        //Debug.Log("monsterUI pos: " + indicatorUIPoint + " playerUI pos: " + playerUIPos);
        Vector2 indicatorPos = playerUIPos + (indicatorUIPoint - playerUIPos).normalized * 200;
        //indicator.localPosition = new Vector3(indicatorPos.x, indicatorPos.y, 0);
        Image1.localPosition = new Vector3(indicatorPos.x, indicatorPos.y, 0);
        UILookAt(Image1, indicatorUIPoint - playerUIPos, Vector3.up);
    }
    /// <summary>
    /// UI的LookAt
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="dir"></param>
    /// <param name="lookAxis"></param>
    public void UILookAt(Transform transform, Vector3 dir, Vector3 lookAxis)
    {
        Quaternion q = Quaternion.identity;
        q.SetFromToRotation(lookAxis, dir);
        transform.rotation = q;
    }

    public void Dispose()
    {
        /*
        indicator.gameObject.SetActive(false);
        indicator.gameObject.name = "-1";
        indicator = null;
        */
        monst = null;
        player = null;
        uiCam = null;
        parent = null;
    }
}
