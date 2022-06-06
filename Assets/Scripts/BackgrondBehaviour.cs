using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrondBehaviour : MonoBehaviour
{
    public bool solstice = false;
    bool stateLight = false;

    public float solsticeTimer;
    public float timerCounter;
    public bool startTimer;

    GameObject back_01;
    GameObject eclipse_02;
    GameObject eclipse_03;
    GameObject eclipse_04;
    GameObject sun_05;
    GameObject sun_06;

    Vector3 backPos;
    Vector3 eclipse02Pos;
    Vector3 eclipse03Pos;
    Vector3 eclipse04Pos;
    Vector3 sun05Pos;
    Vector3 sun06Pos;

    int transitionStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        back_01 = transform.GetChild(0).gameObject;
        eclipse_02 = transform.GetChild(1).gameObject;
        eclipse_03 = transform.GetChild(2).gameObject;
        eclipse_04 = transform.GetChild(3).gameObject;
        sun_05 = transform.GetChild(4).gameObject;
        sun_06 = transform.GetChild(5).gameObject;

        backPos = back_01.transform.localPosition;
        eclipse02Pos = eclipse_02.transform.localPosition;
        eclipse03Pos = eclipse_03.transform.localPosition;
        eclipse04Pos = eclipse_04.transform.localPosition;
        sun05Pos = sun_05.transform.localPosition;
        sun06Pos = sun_06.transform.localPosition;

        timerCounter = solsticeTimer;
    }

    void FixedUpdate()
    {
        if (startTimer)
        {
            timerCounter -= Time.deltaTime;
        }

        if (stateLight)
        {
            if (timerCounter <= 0)
            {
                solstice = true;
                stateLight = true;
                startTimer = false;
                timerCounter = solsticeTimer;
            }
        }

        // Get bright
        if (solstice && !stateLight)
        {
            switch(transitionStep)
            {
                case 0:
                    SolsticeLight(back_01, backPos);
                break;
                case 1:
                    SolsticeLight(eclipse_02, eclipse02Pos);
                break;
                case 2:
                    SolsticeLight(eclipse_03, eclipse03Pos);
                break;
                case 3:
                    SolsticeLight(eclipse_04, eclipse04Pos);
                break;
                case 4:
                    SolsticeLight(sun_05, sun05Pos);
                break;
                case 5:
                    SolsticeLight(sun_06, sun06Pos);
                break;
                case 6:
                    solstice = false;
                    stateLight = true;
                    transitionStep = 5;
                break;
            }
        }
        // Get dark
        else if (solstice && stateLight)
        {
            switch(transitionStep)
            {
                case 5:
                    SolsticeDark(sun_06, sun06Pos);
                break;
                case 4:
                    SolsticeDark(sun_05, sun05Pos);
                break;
                case 3:
                    SolsticeDark(eclipse_04, eclipse04Pos);
                break;
                case 2:
                    SolsticeDark(eclipse_03, eclipse03Pos);
                break;
                case 1:
                    SolsticeDark(eclipse_02, eclipse02Pos);
                break;
                case 0:
                    SolsticeDark(back_01, backPos);
                break;
                case -1:
                    solstice = false;
                    stateLight = false;
                    transitionStep = 0;
                break;
            }
        }
    }

    

    void SolsticeLight(GameObject obj, Vector3 initialPosition)
    {
        if (obj.transform.localPosition.y > initialPosition.y - 4.8f)
        {
            obj.GetComponent<Rigidbody2D>().AddForce(-transform.up * .005f);
        }
        else
        {
            obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            transitionStep++;
        }
    }

    void SolsticeDark(GameObject obj, Vector3 initialPosition)
    {
        if (obj.transform.localPosition.y < initialPosition.y)
        {
            obj.GetComponent<Rigidbody2D>().AddForce(transform.up * .005f);
        }
        else
        {
            obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            transitionStep--;
        }
    }
}
