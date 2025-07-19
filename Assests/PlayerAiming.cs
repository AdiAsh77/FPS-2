using UnityEngine;
//using UnityEngine.Animations.Rigging;
using Unity.Netcode;

public class PlayerAiming : NetworkBehaviour
{
    public Transform aimTarget;  // This is the target that the player will aim at
    //public MultiAimConstraint aimConstraint;  // The Multi-Aim Constraint
    public float smoothTime = 0.02f;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (IsOwner)
        {
            // Update the aim target position based on player input
            Vector3 newAimPosition = aimTarget.position;
            aimTarget.position = Vector3.SmoothDamp(aimTarget.position, newAimPosition, ref velocity, smoothTime);

            // Sync the aim target position across the network

            UpdateAimTargetClientRpc(aimTarget.position);
            UpdateAimTargetServerRpc(aimTarget.position);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateAimTargetServerRpc(Vector3 targetPosition)
    {
        // Synchronize the aim target position across all clients
        //UpdateAimTargetClientRpc(targetPosition);
        aimTarget.position = targetPosition;

    }

    [ClientRpc]
    void UpdateAimTargetClientRpc(Vector3 targetPosition)
    {
        aimTarget.position = Vector3.SmoothDamp(aimTarget.position, targetPosition, ref velocity, smoothTime);
    }

    Vector3 CalculateAimPosition()
    {
        // Example logic to calculate the aim position
        Camera mainCamera = Camera.main;
        return mainCamera.transform.position + mainCamera.transform.forward* 10f;
    }
}
