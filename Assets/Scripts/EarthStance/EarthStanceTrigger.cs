using UnityEngine;

public class EarthStanceTrigger : MonoBehaviour
{
    private SpriteRenderer triggerRenderer;  // The SpriteRenderer of the trigger object
    private Color originalColor;  // Original color of the trigger

    // Reference to the player's EarthStance script (set in the Inspector)
    public EarthStance playerEarthStance;

    // The collider to use for the proximity check (e.g., the Feet Collider)
    public Collider2D playerFeetCollider;

    void Start()
    {
        // Get the SpriteRenderer of the object to change its color
        triggerRenderer = GetComponent<SpriteRenderer>();
        originalColor = triggerRenderer.material.color;  // Store the original color
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player’s feet collider has entered the trigger area
        if (other == playerFeetCollider)
        {
            // Change the color of the trigger object to green when player is near
            triggerRenderer.material.color = Color.green;
            // Enable Earth Stance for the player
            playerEarthStance.CanEnterEarthStance(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player’s feet collider has exited the trigger area
        if (other == playerFeetCollider)
        {
            // Reset the color back to the original when player leaves the trigger
            triggerRenderer.material.color = originalColor;
            // Disable Earth Stance for the player
            playerEarthStance.CanEnterEarthStance(false);
        }
    }
}
