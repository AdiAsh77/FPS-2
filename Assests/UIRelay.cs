using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRelay : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI output;
    public TMP_InputField code;
    public TestRelay rel;
    public GameObject screen;
    public GameObject login;
    static string d;

   
    public void Button()
    {
        rel.JoinRelay(code.text);
        screen.SetActive(false);
        //login.SetActive(true);
    }
    
    public void Sary(string s)
    {
        d = s;
        login.SetActive(true);
        output.text ="JOINING CODE: " + s;
        Debug.Log(s);
    }
    
    public void CreateButton()
    {
        rel.CreateRelay();
        //Debug.Log(d);

        screen.SetActive(false);
        
    }


}
