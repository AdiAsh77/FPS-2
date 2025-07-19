using Unity.Netcode;
using UnityEngine;

public class DeactivateObjectNetwork : NetworkBehaviour
{
    private void Update()
    {
        // Check if the host or client presses the key (e.g., 'D' for deactivate)
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Check if the current player is the owner or a client
            if (IsOwner || IsClient)
            {
                // Request deactivation across the network
                DeactivateGameObjectServerRpc();
            }
        }
    }


    public void DG()
    {
        if (IsOwner || IsClient)
        {
            // Request deactivation across the network
            DeactivateGameObjectServerRpc();
        }
    }


    // ServerRpc to request the server to deactivate the GameObject
    [ServerRpc(RequireOwnership = false)]
    private void DeactivateGameObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        // Call the client Rpc to deactivate the GameObject on all clients
        DeactivateGameObjectClientRpc();
    }

    // ClientRpc to deactivate the GameObject on all clients
    [ClientRpc]
    private void DeactivateGameObjectClientRpc(ClientRpcParams rpcParams = default)
    {
        gameObject.SetActive(false);
    }
}
