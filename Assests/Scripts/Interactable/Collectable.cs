using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Collectable : NetworkBehaviour
{
    public string promptMessage;
    public void BaseCollect()
    {
        Collact();
    }

    // Update is called once per frame
    protected virtual void Collact()
    { 
        
    }
}
