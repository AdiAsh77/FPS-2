using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject setin;
    //public GameObject joystick;
    //public Vector3 offset;
    public void seting()
    {
        setin.SetActive(false);
        //joystick.SetActive(true);
        Time.timeScale = 1;

    }
}
