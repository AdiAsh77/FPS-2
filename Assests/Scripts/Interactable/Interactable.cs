using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Interactable : NetworkBehaviour
{
    public string promptMessage;
    public void BaseInteract()
    {
        Interact();
    }

    // Update is called once per frame
    protected virtual void Interact()
    {
        
    }
}
