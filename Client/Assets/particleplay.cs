using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleplay : MonoBehaviour

{
    private GameObject glow;
    //private GameObject xuying;
    private GameObject zhangangshan;
    private GameObject yan;
    // Start is called before the first frame update
    void Start()
    {
        //glow = GameObject.Find("glow");
        glow = transform.GetChild(1).gameObject;
        //xuying = transform.GetChild(2).gameObject;
        zhangangshan = transform.GetChild(3).gameObject;
        yan = transform.GetChild(4).gameObject;
        glow.GetComponent<ParticleSystem>().Stop();
        //xuying.GetComponent<ParticleSystem>().Stop();
        zhangangshan.GetComponent<ParticleSystem>().Stop();
        yan.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tiaozhantexiao()
    {
        glow.GetComponent<ParticleSystem>().Play();

    }
    public void shanbi()
    {
        //xuying.GetComponent<ParticleSystem>().Play();
    }
    public void pugongtexiao()
    {
        zhangangshan.GetComponent<ParticleSystem>().Play();
    }
    public void blink()
    {
        yan.GetComponent<ParticleSystem>().Play();
    }
}

