using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraControler : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject camCon;
    public Vector3 offset;
    private void Start()
    {
        if (IsLocalPlayer)
        {
            camCon.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        camCon.transform.position = transform.position + offset;
    }
}
