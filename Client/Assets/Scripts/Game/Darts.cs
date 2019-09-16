using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darts : MonoBehaviour
{
    Vector3 fwd;
    public float dartsSpeed;
    public string user;
    public float saveTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "arena")
        {

            Debug.Log("nothing");
            Destroy(gameObject);
        }
        if (other.gameObject.transform.tag == "Anemy" )
        {
            //Debug.Log(this.GetComponent<Darts>().user);
        }
        else if(other.gameObject.transform.tag == "Player" && user != other.transform.GetComponent<Player>().ID && (other.gameObject.transform.GetComponent<Player>().state == (int)State.play || other.gameObject.transform.GetComponent<Player>().state == (int)State.normal))
        {
            
            Debug.Log(this.GetComponent<Darts>().user);
            string[] data = new string[3];
            data[0] = this.GetComponent<Darts>().user;
            data[1] = other.GetComponent<Player>().ID;
            data[2] = SkillType.DartAttack.ToString();
            Debug.Log("Darts death: " + data[0] + data[1] + data[2]);
            NotificationCenter.Instance.PushEvent(NotificationType.Network_OnResponseDeath, data);
        }
    }

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, saveTime);
        //transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        //transform.localEulerAngles = new Vector3(transform.eulerAngles.x, GameObject.FindGameObjectWithTag("Player").transform.eulerAngles.y, transform.eulerAngles.z);
        //fwd = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().AddForce(transform.right * dartsSpeed);//给物体一个向前的力
        transform.Translate(Vector3.right * dartsSpeed * Time.deltaTime);
        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, dartsSpeed);
    }
}
