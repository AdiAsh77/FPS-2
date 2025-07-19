using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//using UnityEngine.UI;

public class PlayerShot : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float distance = 7f;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private LayerMask mask2;
    //private PlayerUI playerUI;
    private Inputs inputManager;
    [SerializeField]
    private PlayerKill kill;
    static bool s = false;
    static int heal = 200;
    static ulong DieId = 500;
    //public Button btn;



    public int MaxHealth = 200;
    private NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    public Health_Bar health;

    public GameObject player;
    public Rig rig;
    [SerializeField] private GameObject killState;


    // private InputManager inputManager;
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        //playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<Inputs>();
        CurrentHealth.Value = MaxHealth;
        health.SetMaxHealth(MaxHealth);
    }


    // Update is called once per frame
    void Update()
    {
        //playerUI.Updatetext(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            var plInfo = hitInfo.collider.GetComponentInParent<NetworkObject>();
            //playerUI.Updatetext(collectable.promptMessage);

            //if(btn != null)
            //{
            //    btn.onClick.AddListener(killcall);
            //}
            //killcall(plInfo.OwnerClientId);
            if (plInfo != null && inputManager.onFoot.Shoot.triggered)
            {
                //inputManager.Restore(20);
                //collectable.BaseCollect();

                UpdateHealthServerRpc(40, plInfo.OwnerClientId);

            }

            //}
        }

        if (Physics.Raycast(ray, out hitInfo, distance, mask2))
        {
            var plInfo = hitInfo.collider.GetComponentInParent<NetworkObject>();
            //playerUI.Updatetext(collectable.promptMessage);


            if (plInfo != null && inputManager.onFoot.Shoot.triggered)
            {
                //inputManager.Restore(20);
                //collectable.BaseCollect();

                UpdateHealth2ServerRpc(40, plInfo.OwnerClientId);
                kill.ShowKillImage();
            }

            //}
        }

        if (s == true && OwnerClientId == DieId)
        {
            Debug.Log("The ID ISSSSSISSIIIIIIIIIIISSSSSS:::   " + OwnerClientId);
            changeHeal(heal);
            s = false;
        }

        if (IsOwner && Input.GetKeyDown(KeyCode.V))
        {
            //Debug.Log("abcdse"+ CurrentHealth.Value);
            //GGServerRpc();
            //Debug.Log("abc..........dse"+ CurrentHealth.Value);
            //health.SetHealth(200);
            alive();
        }

    }

    public void alive()
    {
        GGServerRpc();
        //Debug.Log("abc..........dse"+ CurrentHealth.Value);
        health.SetHealth(200);


        killState.SetActive(false);
        Time.timeScale = 1;
        rig.weight = 1;
        player.GetComponent<Animator>().SetBool("death", false);
        player.GetComponent<Animator>().SetLayerWeight(1, 1);

    }

    [ServerRpc(RequireOwnership =false)]
    public void GGServerRpc()
    {
        var clientDam = NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<PlayerShot>();
        clientDam.CurrentHealth.Value = 200;
        //Debug.Log("running:---------:------  "+ clientDam.CurrentHealth.Value);
        clientDam.health.SetHealth(clientDam.CurrentHealth.Value);
        //CurrentHealth.Value += 40;
        //health.SetHealth();
    }
    
    public void body()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            var plInfo = hitInfo.collider.GetComponentInParent<NetworkObject>();
            //playerUI.Updatetext(collectable.promptMessage);

            //if(btn != null)
            //{
            //    btn.onClick.AddListener(killcall);
            //}
            //killcall(plInfo.OwnerClientId);
            if (plInfo != null)
            {
                //inputManager.Restore(20);
                //collectable.BaseCollect();

                UpdateHealthServerRpc(40, plInfo.OwnerClientId);

            }

            //}
        }
    }
    
    public void head()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, mask2))
        {
            var plInfo = hitInfo.collider.GetComponentInParent<NetworkObject>();
            //playerUI.Updatetext(collectable.promptMessage);


            if (plInfo != null)
            {
                //inputManager.Restore(20);
                //collectable.BaseCollect();

                UpdateHealth2ServerRpc(40, plInfo.OwnerClientId);
                kill.ShowKillImage();
            }

            //}
        }
    }



    public void killcall()
    {
        head();
        body();

    }

    //public void Headcall(ulong kId)
    //{
    //    if (kId != null && (inputManager.onFoot.Shoot.triggered))
    //    {
    //        //inputManager.Restore(20);
    //        //collectable.BaseCollect();

    //        UpdateHealthServerRpc(40, kId);

    //    }

    //}

    //public void calling()
    //{
    //    UpdateHealth2ServerRpc(dam, kId);
    //}


    [ServerRpc]
    public void UpdateHealthServerRpc(int dam, ulong clientId)
    {
        var clientDam = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerShot>();

        if(clientDam != null && clientDam.CurrentHealth.Value > 0)
        {
            clientDam.CurrentHealth.Value -= dam;
            clientDam.health.SetHealth(clientDam.CurrentHealth.Value);
            if (clientDam.CurrentHealth.Value <= 0)
            {
                //clientDam.kill.ShowKillImage();
                //clientDam.inputManager.DespawnSpawn(clientId);
                clientDam.inputManager.dead();
            }
            Debug.Log("YOU GOT HIT. REMAINING HEALTH:  " + clientDam.CurrentHealth.Value + "........" +  clientDam.NetworkObject.OwnerClientId);
            
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
    
    [ServerRpc]
    public void UpdateHealth2ServerRpc(int dam, ulong clientId)
    {
        var clientDam = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerShot>();

        if(clientDam != null && clientDam.CurrentHealth.Value > 0)
        {
            clientDam.CurrentHealth.Value = 0;
            clientDam.health.SetHealth(clientDam.CurrentHealth.Value);
            if (clientDam.CurrentHealth.Value <= 0)
            {
                //clientDam.kill.ShowKillImage();
                //clientDam.inputManager.DespawnSpawn(clientId);
                clientDam.inputManager.dead();
            }
            //Debug.Log("YOU GOT HIT. REMAINING HEALTH:  " + clientDam.CurrentHealth.Value);
            
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
            //kill.ShowKillImage();
            //inputManager.DespawnSpawn(OwnerClientId);
            inputManager.dead();
        }
        Debug.Log("Still does not work I gUESS");
    }



    //[ServerRpc(RequireOwnership = false)]
    //public void changeHellServerRpc(ulong id, int hell)
    //{
    //    var ch = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerCollectable>();
    //    ch.health.SetHealth(hell);
    //    Debug.Log("doesnot work");
    //}

}
