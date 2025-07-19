using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKill : NetworkBehaviour
{
    [SerializeField] private GameObject killImage;
    //static ulong s = 500;

    public void ShowKillImage()
    {
        if (IsLocalPlayer)
        {

            StartCoroutine(ShowKillImageCoroutine());
            
        }

        //s = 500;
    }

    private IEnumerator ShowKillImageCoroutine()
    {
        killImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        killImage.SetActive(false);
    }

    //[ServerRpc(RequireOwnership = false)]
    //public void ghServerRPC()
    //{

    //}
    public void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.M))
        {
            ShowKillImage();
        }
        if (IsOwner && Input.GetKeyDown(KeyCode.N))
        {
            killImage.SetActive(false);
        }

        //if (s == OwnerClientId)
        //{
        //    Debug.Log("I killed Him");
        //    ShowKillImage();
        //}

    }
}
