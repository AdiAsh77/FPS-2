using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class Inputs : NetworkBehaviour
{
    private InputManager inputManager;
    public InputManager.OnFootActions onFoot;
    [SerializeField] private float PosRange = 5f;
    private PlayerMotor motor;
    private PlayerLook look;
    public int MaxHealth = 200;
    public int CurrentHealth;
    public Health_Bar health;
    [HideInInspector]
    public NetworkVariable<int> HealthPoint = new NetworkVariable<int>();
    public CapsuleCollider firstCollider;
    private ulong LastAttackerID;
    public PlayerKill Attack;
    public GameObject player;
    public Rig rig;
    [SerializeField] private GameObject killState;


    // Start is called before the first frame update
    private void Start()
    {
        CurrentHealth = MaxHealth;
        health.SetMaxHealth(MaxHealth);
    }
    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();
        UpdatePosServerRPC();
        HealthPoint.Value = 200;
    }
    //public override void OnNetworkSpawn()
    //{
        
    //}

    void Awake()
    {   
        inputManager = new InputManager();
        onFoot = inputManager.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.jump();
        
    }
    public void OnParticleUpdateJobScheduled()
    {
        
    }
    public void Restore(int points)
    {
        if ((MaxHealth - CurrentHealth) <= points)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth += points;
        }
        
        health.SetHealth(CurrentHealth);
    }
    // Update is called once per frame
    public void TakeDamage(int damage, ulong AttackerID)
    {
        LastAttackerID = AttackerID;
        
        CurrentHealth -= damage;
        health.SetHealth(CurrentHealth);
        Debug.Log("I am Called");

        if (CurrentHealth <= 0)
        {

            //DespawnSpawn(LastAttackerID);
            player.GetComponent<Animator>().SetLayerWeight(1, 0);
            rig.weight = 0;
            player.GetComponent<Animator>().SetBool("death", true);
            Debug.Log("lets go");
            dead();
        }

    }

    public void dead()
    {
        player.GetComponent<Animator>().SetLayerWeight(1, 0);
        rig.weight = 0;
        player.GetComponent<Animator>().SetBool("death", true);
        Debug.Log("lets go");
        if (IsLocalPlayer)
        {
            StartCoroutine(ShowKillImageCoroutine());
        }
    }

    private IEnumerator ShowKillImageCoroutine()
    {
        yield return new WaitForSeconds(5f);
        killState.SetActive(true);
        Time.timeScale = 0;
        //killState.SetActive(false);
    }

    public void alive()
    {
        killState.SetActive(false);
        Time.timeScale = 1;
        health.SetMaxHealth(MaxHealth);
        CurrentHealth = MaxHealth;
        rig.weight = 1;
        player.GetComponent<Animator>().SetBool("death", false);
        player.GetComponent<Animator>().SetLayerWeight(1, 1);
    }

    
    //public void Takamage(int damage)
    //{
    //    //LastAttackerID = AttackerID;
        
    //    CurrentHealth -= damage;
    //    health.SetHealth(CurrentHealth);
    //    Debug.Log("I am Called");

    //    if (CurrentHealth <= 0)
    //    {
    //        Debug.Log("Dead");
    //    }

    //}

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePosServerRPC()
    {
        transform.position = new Vector3(Random.Range(-70, 0), 13.95013f, Random.Range(-70, 0));
    }


    void Update()
    {
        if (IsOwner && Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            motor.jump();
        }

        //if (IsOwner && Input.GetKeyDown(KeyCode.J))
        //{
        //    DespawnSpawn();
        //}

        if (IsOwner && Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(50, OwnerClientId);
        }
        
        //if (IsOwner && Input.GetKeyDown(KeyCode.V))
        //{
        //    alive();
        //}
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        if (!IsOwner) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }


    public void DespawnSpawn(ulong KillId)
    {
        if (!IsOwner) return;
        AbServerRPC(KillId);
    }
    [ServerRpc(RequireOwnership = false)]
    private void AbServerRPC(ulong SomeId)
    {
        //Attack.ShowKillImageClientRpc(SomeId);
        NetworkObject.Despawn();
    }



}
