using UnityEngine;

public class Spike : MonoBehaviour
{
    // Reference to the player's Transform for teleportation
    public Transform playerTransform;

    // List of all checkpoints in the scene
    public Checkpoint[] checkpoints;

    // Method to handle collision with the spikes
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayerToNearestCheckpoint();
        }
    }

    // Teleports the player to the nearest checkpoint
    void TeleportPlayerToNearestCheckpoint()
    {
        if (checkpoints.Length == 0)
        {
            Debug.LogError("No checkpoints found!");
            return;
        }

        // Find the nearest checkpoint
        Checkpoint nearestCheckpoint = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            float distance = Vector2.Distance(playerTransform.position, checkpoint.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCheckpoint = checkpoint;
            }
        }

        // Teleport the player to the nearest checkpoint
        if (nearestCheckpoint != null)
        {
            playerTransform.position = nearestCheckpoint.transform.position;
            Debug.Log("Player teleported to nearest checkpoint.");
        }
    }
}
