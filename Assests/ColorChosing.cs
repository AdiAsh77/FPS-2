using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorChosing : MonoBehaviour
{
    public PlayerSettings settings;
    //public GameObject color;
    //public GameObject login;


    public void Button()
    {
        settings.ChangeMaterial(0);
        
        //color.SetActive(false);
        //login.SetActive(true);
    }
    public void Button1()
    {
        settings.ChangeMaterial(1);
        //color.SetActive(false);
        //login.SetActive(true);
    }
    public void Button2()
    {
        settings.ChangeMaterial(2);
        //color.SetActive(false);
        //login.SetActive(true);
    }
    public void Button3()
    {
        settings.ChangeMaterial(3);
        //color.SetActive(false);
        //login.SetActive(true);
    }


    //public void CreateButton()
    //{
    //    rel.CreateRelay();
    //    //Debug.Log(d);

    //    screen.SetActive(false);

    //}
}
