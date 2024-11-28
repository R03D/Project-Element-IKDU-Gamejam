using UnityEngine;
using System.Collections.Generic;

public class EarthStance : MonoBehaviour
{
    public Camera mainCamera;
    private List<PlatformController> visiblePlatforms = new List<PlatformController>();
    private int selectedIndex = 0;
    private bool inEarthStance = false;
    private bool canEnterEarthStance = false;
    private bool permanentEarthStance = false;

    // Reference to the Earth Stance indicator prefab
    public GameObject earthStanceIndicatorPrefab;

    // The instance of the Earth Stance indicator
    private GameObject earthStanceIndicatorInstance;

    public Transform playerTransform; // Reference to the player's transform for positioning the indicator

    void Update()
    {
        if ((canEnterEarthStance || permanentEarthStance) && Input.GetKeyDown(KeyCode.P)) // Enter/Exit Earth Stance
        {
            inEarthStance = !inEarthStance;
            if (inEarthStance)
            {
                EnterEarthStance();
            }
            else
            {
                ExitEarthStance();
            }
        }

        if (inEarthStance)
        {
            // Continuously check for new visible platforms
            UpdateVisiblePlatforms();

            if (Input.GetKeyDown(KeyCode.RightArrow)) SelectNextPlatform();
            if (Input.GetKeyDown(KeyCode.LeftArrow)) SelectPreviousPlatform();
            if (Input.GetKeyDown(KeyCode.L)) ToggleSelectedPlatform();
        }
    }

    public void CanEnterEarthStance(bool canEnter)
    {
        canEnterEarthStance = canEnter;
        if (!canEnterEarthStance && inEarthStance)
        {
            // Automatically exit Earth Stance if not near the trigger
            ExitEarthStance();
        }
    }

    public void EnablePermanentEarthStance(bool enable)
    {
        permanentEarthStance = enable;
    }

    void EnterEarthStance()
    {
        UpdateVisiblePlatforms();

        // Spawn the Earth Stance indicator
        if (earthStanceIndicatorPrefab != null && playerTransform != null)
        {
            earthStanceIndicatorInstance = Instantiate(earthStanceIndicatorPrefab, playerTransform.position + Vector3.up * 3.5f, Quaternion.identity);
            earthStanceIndicatorInstance.transform.parent = playerTransform; // Attach to the player
        }
    }

    void ExitEarthStance()
    {
        inEarthStance = false;

        // Remove highlight from all platforms
        foreach (var platform in visiblePlatforms)
        {
            platform.Highlight(false);
        }

        visiblePlatforms.Clear();

        // Destroy the Earth Stance indicator
        if (earthStanceIndicatorInstance != null)
        {
            Destroy(earthStanceIndicatorInstance);
        }
    }

    void UpdateVisiblePlatforms()
    {
        PlatformController[] allPlatforms = FindObjectsOfType<PlatformController>();

        foreach (PlatformController platform in allPlatforms)
        {
            if (IsPlatformVisible(platform) && !visiblePlatforms.Contains(platform))
            {
                visiblePlatforms.Add(platform);
            }
        }

        visiblePlatforms.RemoveAll(platform => !IsPlatformVisible(platform));

        if (visiblePlatforms.Count > 0 && selectedIndex >= visiblePlatforms.Count)
        {
            selectedIndex = 0;
        }

        HighlightCurrentPlatform();
    }

    bool IsPlatformVisible(PlatformController platform)
    {
        return platform.GetComponent<Renderer>().isVisible;
    }

    void HighlightCurrentPlatform()
    {
        for (int i = 0; i < visiblePlatforms.Count; i++)
        {
            visiblePlatforms[i].Highlight(i == selectedIndex);
        }
    }

    void SelectNextPlatform()
    {
        if (visiblePlatforms.Count == 0) return;
        selectedIndex = (selectedIndex + 1) % visiblePlatforms.Count;
        HighlightCurrentPlatform();
    }

    void SelectPreviousPlatform()
    {
        if (visiblePlatforms.Count == 0) return;
        selectedIndex = (selectedIndex - 1 + visiblePlatforms.Count) % visiblePlatforms.Count;
        HighlightCurrentPlatform();
    }

    void ToggleSelectedPlatform()
    {
        if (visiblePlatforms.Count == 0) return;
        visiblePlatforms[selectedIndex].MovePlatform();
    }
}
