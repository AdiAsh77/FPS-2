using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Keypad : Interactable
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    void Start()
    {

    }

    // Update is called once per framed
    void Update()
    {

    }
    protected override void Interact()
    {
        dooropServerRPC();
    }
    [ServerRpc(RequireOwnership = false)]
    public void dooropServerRPC()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}