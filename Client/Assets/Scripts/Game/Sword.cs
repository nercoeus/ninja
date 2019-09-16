using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Vector3 fwd;
    public float SwordSpeed;
    public string user;


    private void OnCollisionEnter(Collision collision)
    {
       
    }
    private void OnCollisionStay(Collision collision)
    {
        
    }
    private void OnCollisionExit(Collision collision)
    {


    }
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3f);
        //transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        //transform.localEulerAngles = new Vector3(transform.eulerAngles.x, GameObject.FindGameObjectWithTag("Player").transform.eulerAngles.y, transform.eulerAngles.z);
        //fwd = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.down * SwordSpeed);//给物体一个向下的力
    }
}
