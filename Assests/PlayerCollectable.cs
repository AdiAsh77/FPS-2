using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlayerCollectable : NetworkBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    [SerializeField]
    private float distance = 7f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private Inputs inputManager;

    // private InputManager inputManager;
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<Inputs>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.Updatetext(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;


        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {

            if (hitInfo.collider.GetComponent<Collectable>() != null)
            {
                Collectable collectable  = hitInfo.collider.GetComponent<Collectable>();
                playerUI.Updatetext(collectable.promptMessage);
                if (inputManager.onFoot.Interact.triggered)
                {
                    inputManager.Restore(20);
                    collectable.BaseCollect();
                }

            }
        }
    }
}
