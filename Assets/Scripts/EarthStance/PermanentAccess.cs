using UnityEngine;

public class PermanentAccess : MonoBehaviour
{
    // Reference to the EarthStance script
    public EarthStance playerEarthStance;

    // The collider to use for the proximity check (e.g., the Feet Collider)
    public Collider2D playerFeetCollider; // Make sure this is assigned correctly

    // Flag to prevent re-triggering
    private bool hasAccessGranted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player feet collider entered the trigger
        if (!hasAccessGranted && other == playerFeetCollider)
        {
            Debug.Log("Permanent Earth Stance access granted!");

            // Grant permanent access by enabling Earth Stance permanently
            playerEarthStance.EnablePermanentEarthStance(true);
            hasAccessGranted = true;

            // Optionally, change the appearance of the element to indicate it has been used
            // Example: change color
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}
