using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Vector2 positionA; // First position
    public Vector2 positionB; // Second position
    private bool isAtPositionA = true;

    private SpriteRenderer spriteRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Renderer platformRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformRenderer = GetComponent<Renderer>();
        originalColor = spriteRenderer.color;

        // Set initial position
        transform.position = positionA;
    }

    public void MovePlatform()
    {
        // Only move the platform if it is visible to the camera
        if (!IsVisibleToCamera())
        {
            Debug.Log("Platform is not visible. Cannot be moved.");
            return;
        }

        // Toggle platform position
        Vector2 targetPosition = isAtPositionA ? positionB : positionA;
        isAtPositionA = !isAtPositionA;
        StartCoroutine(SmoothMove(targetPosition));
    }

    private System.Collections.IEnumerator SmoothMove(Vector2 targetPosition)
    {
        float time = 0f;
        Vector2 startPosition = transform.position;
        while (time < 1f)
        {
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(startPosition, targetPosition, time);
            yield return null;
        }
        transform.position = targetPosition; // Ensure exact target position
    }

    public void Highlight(bool enable)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = enable ? highlightColor : originalColor;
        }
    }

    private bool IsVisibleToCamera()
    {
        return platformRenderer.isVisible;
    }
}
