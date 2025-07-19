using System.Reflection;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : NetworkBehaviour
{
    public GameObject player;
    //public GameObject bullet;
    public Transform aim;
    public Transform barrel;
    public float damage = 10f;
    public float range = 10f;
    public float FireRate = 0.5f;
    public float impactForce = 30f;
    public float bulvel = 30f;
    public Camera fpscam;
    public ParticleSystem muzflash;
    public GameObject impactEffect;
    private float NextTimeToFire = 0f;
    
    private InputManager inputManager;
    public InputManager.OnFootActions onFoot;
    

    // Start is called before the first frame update
    void Awake()
    {
        inputManager = new InputManager();
        onFoot = inputManager.OnFoot;

        
        // onFoot.Shoot.performed += ctx => Shoot();
        
    }
    void Update()
    {
        if (!IsOwner) return;
        if(inputManager.OnFoot.Shoot.triggered && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + FireRate;
            // onFoot.Shoot.performed += ctx => Shoot();
            //SpawnBullServerRPC();
            Shoot();
            //

        }
        else
        {
            player.GetComponent<Animator>().SetBool("shoot", false);
        }
        
    }

    public void Shoot()
    {
        if (!IsOwner) return;
        muzflash.Play();
        Debug.Log("ff");
        //Instantiate(muzflash);
        barrel.LookAt(aim.position);
        player.GetComponent<Animator>().SetBool("shoot", true);

        //GameObject current_bullet = Instantiate(bullet, barrel.position, barrel.rotation);
        //current_bullet.GetComponent<NetworkObject>().Spawn();

        //Rigidbody rb = current_bullet.GetComponent<Rigidbody>();
        //rb.AddForce(barrel.forward * bulvel, ForceMode.Impulse);
        //SpawnBullServerRPC();
        
        RaycastHit hit;

        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
        {
           
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject ImpactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGO, 1f);

        }
    }

    //[ServerRpc]
    //private void SpawnBullServerRPC(ServerRpcParams serverRpcParams = default)
    //{
    //    barrel.LookAt(aim.position);
    //    GameObject current_bullet = Instantiate(bullet, barrel.position, barrel.rotation);
    //    current_bullet.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    //    Destroy(current_bullet, 12f);

    //}

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
