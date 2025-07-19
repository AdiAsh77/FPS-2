using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.Netcode;
using TMPro;

public class PlayerLook : NetworkBehaviour
{
    public TextMeshProUGUI sens;
    public Transform aim;
    //public GameObject playr;
    public float range = 100f;
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void Update()
    {
        //float d = sens.text.Con;
        xSensitivity = float.Parse(sens.text);
        ySensitivity = float.Parse(sens.text);
        //MobLook();
    }
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            aim.position = Vector3.Lerp(aim.position, hit.point, 20 * Time.deltaTime);
            //aim.LookAt = Vector3.Lerp(aim.position, hit.point, 20 * Time.deltaTime);
            //aim.position = new Vector3(0, hit.point.y, 0);
        }
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //playr.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    //public void MobLook()
    //{
    //    if (!IsOwner) return;

    //    float mousex = 0;
    //    float mouseY = 0;

    //    if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
    //    {
    //        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
    //        {
    //            Debug.Log("UI TOUCH");
    //            return;
    //        }
    //        mousex = Touchscreen.current.touches[0].delta.ReadValue().x;
    //        mouseY = Touchscreen.current.touches[0].delta.ReadValue().y;
    //    }

    //    RaycastHit hit;
    //    if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
    //    {
    //        aim.position = Vector3.Lerp(aim.position, hit.point, 20 * Time.deltaTime);
    //        //aim.LookAt = Vector3.Lerp(aim.position, hit.point, 20 * Time.deltaTime);
    //        //aim.position = new Vector3(0, hit.point.y, 0);
    //    }
    //    xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
    //    xRotation = Mathf.Clamp(xRotation, -80f, 80f);
    //    cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    //    //playr.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    //    transform.Rotate(Vector3.up * mousex * Time.deltaTime * xSensitivity);
    //}

}
