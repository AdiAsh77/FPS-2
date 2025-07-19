using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class bullet : NetworkBehaviour
{
    [SerializeField]
    private float speed = 20f;
    // Start is called before the first frame update

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("hhhhhhhhhhhhhhhhhhhhhhhhhhh");
    //    Debug.Log("____________________________" + collision.gameObject.tag);
    //    if (!IsOwner) return;


    //    Debug.Log("in here");
        
    //    //Destroy(gameObject, 10f);
    //    if (collision.gameObject.tag == "IsPlayer")
    //    {
    //        Debug.Log("Player collided with an enemy!");
    //        //Destroy(gameObject, 5f);
    //        Inputs inpg = collision.gameObject.GetComponent<Inputs>();
    //        inpg.TakeDamage(30);
    //        Debug.Log("Finalyy");
    //        //collision.gameObject.GetComponent<CapsuleCollider>();

    //        if (inpg != null)
    //        {
    //            inpg.TakeDamage(5);
    //        }

    //        // Logic when the player collides with an enemy
    //    }
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
        //Destroy(gameobject, 10f);
    }

    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
        
    //}
}
