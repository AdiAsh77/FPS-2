using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private Material[] availableMaterials; // Assign in the inspector
    private SkinnedMeshRenderer playerRenderer;

    // A NetworkVariable to store the material index
    private NetworkVariable<int> materialIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        // Subscribe to the value change event
        //if (IsOwner)
        //{
        //    //Debug.Log("This player is the owner of the object.");
        //    // Optionally request material change here or in response to some input
        //    ChangeMaterial(1); // Default material
        //}

        materialIndex.OnValueChanged += OnMaterialIndexChanged;
        UpdateMaterial(materialIndex.Value);
    }

    private void OnDestroy()
    {
        materialIndex.OnValueChanged -= OnMaterialIndexChanged;
    }

    private void OnMaterialIndexChanged(int oldValue, int newValue)
    {
        UpdateMaterial(newValue);
    }

    private void UpdateMaterial(int index)
    {
        if (index >= 0 && index < availableMaterials.Length)
        {
            playerRenderer.material = availableMaterials[index];
        }
    }

    // Client-side function to request a material change
    public void ChangeMaterial(int newMaterialIndex)
    {
        if (IsOwner)
        {
            //Debug.Log("Called");
            ChangeMaterialServerRpc(newMaterialIndex);
        }
    }

    // Server RPC to change the material
    [ServerRpc]
    private void ChangeMaterialServerRpc(int newMaterialIndex)
    {
        materialIndex.Value = newMaterialIndex;
    }

    void Update()
    {
        if (IsOwner && Input.GetKeyDown(KeyCode.U))
        {
            ChangeMaterial(0); // Change to material at index 1
        }
        
        if (IsOwner && Input.GetKeyDown(KeyCode.I))
        {
            ChangeMaterial(1); // Change to material at index 1
        }
        
        if (IsOwner && Input.GetKeyDown(KeyCode.O))
        {
            ChangeMaterial(2); // Change to material at index 1
        }
        
        if (IsOwner && Input.GetKeyDown(KeyCode.P))
        {
            ChangeMaterial(3); // Change to material at index 1
        }
    }
}
