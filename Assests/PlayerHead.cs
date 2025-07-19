using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHead : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float distance = 7f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private Inputs inputManager;
    static bool s = false;
    static int heal = 200;
    static ulong DieId = 500;



    public int MaxHealth = 200;
    private NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    public Health_Bar health;




    // private InputManager inputManager;
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<Inputs>();
        CurrentHealth.Value = MaxHealth;
        health.SetMaxHealth(MaxHealth);
    }


    // Update is called once per frame
    void Update()
    {
        playerUI.Updatetext(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {

            if (hitInfo.collider.GetComponent<Collectable>() != null)
            {
                Collectable collectable = hitInfo.collider.GetComponent<Collectable>();
                var plInfo = hitInfo.collider.GetComponentInParent<NetworkObject>();
                playerUI.Updatetext(collectable.promptMessage);


                if (plInfo != null && inputManager.onFoot.Shoot.triggered)
                {
                    //inputManager.Restore(20);
                    //collectable.BaseCollect();

                    UpdateHealthServerRpc(40, plInfo.OwnerClientId);

                }

            }
        }

        if (s == true && OwnerClientId == DieId)
        {
            Debug.Log("The ID ISSSSSISSIIIIIIIIIIISSSSSS:::   " + OwnerClientId);
            changeHeal(heal);
            s = false;
        }
    }



    [ServerRpc]
    public void UpdateHealthServerRpc(int dam, ulong clientId)
    {
        var clientDam = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerHead>();

        if(clientDam != null && clientDam.CurrentHealth.Value > 0)
        {
            clientDam.CurrentHealth.Value = 0;
            clientDam.health.SetHealth(clientDam.CurrentHealth.Value);
            if (clientDam.CurrentHealth.Value <= 0)
            {
                clientDam.inputManager.DespawnSpawn(clientId);
            }
            Debug.Log("YOU GOT HIT. REMAINING HEALTH:  " + clientDam.CurrentHealth.Value);
            
        }
        
        int hell = clientDam.CurrentHealth.Value;
        
        NotifyHealthClientRpc(clientId, hell, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        });

    }

    [ClientRpc]
    public void NotifyHealthClientRpc(ulong id, int hell, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner) return;
        Debug.Log("YOU GOT HIT. REMAINING HEALTH:  " + hell);
        //changeHellServerRpc(id, hell);
        //changeHeal(hell);
        
        DieId = id;
        heal = hell;
        s = true;
        
        //health.SetHealth(hell);
    }

    public void changeHeal(int hell)
    {
        health.SetHealth(hell);
        if (hell <= 0)
        {
            inputManager.DespawnSpawn(OwnerClientId);
        }
        Debug.Log("Still does not work I gUESS");
    }


    [ServerRpc(RequireOwnership = false)]
    public void changeHellServerRpc(ulong id, int hell)
    {
        var ch = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerHead>();
        ch.health.SetHealth(hell);
        Debug.Log("doesnot work");
    }

}
