using System.ComponentModel;
using UnityEngine;

public class Gemtest : MonoBehaviour
{
    public bool isAnimating = false;
    private float T;
    [SerializeField] private float vectorArc = 10f; // Adjusted for UI units
    [SerializeField] private float duration = 2f;
    private Vector2 dir;
    private RectTransform rectTransform;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform testTarget;
    private void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating) return;
        T += Time.deltaTime;
        float t01 =Mathf.Clamp01(T / duration);
        Vector2 arcOffset = Mathf.Sin(t01 * Mathf.PI) * vectorArc * dir;
        Vector2 newPos = Vector2.Lerp(rectTransform.anchoredPosition , Vector2.zero , t01);
        // Calculate perpendicular direction
        Vector2 direction = ( Vector2.zero - rectTransform.anchoredPosition ).normalized;
        Vector2 perpendicular = new Vector2(-direction.y , direction.x); // 90 degree rotation

        // Apply arc offset
        Vector2 finalPos = newPos + perpendicular * arcOffset;

        rectTransform.anchoredPosition = finalPos;

        if (t01>=1)
        {
            Debug.Log("completed!");
            isAnimating = false;
            T = 0f; // Reset the timer
        }
    }

    [ContextMenu("Collect Gem")]
    public void CollectGem ()
    {
        if (isAnimating) return;
        dir = GetDirection();
        rectTransform.SetParent(testTarget);
        isAnimating = true;
    }

    [ContextMenu("Reset Gem")]
    public void ResetGem ()
    {
        isAnimating = false;
        RectTransform parent = CommandCenter.Instance.mainMenuController_.GetSafeArea();
        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = Vector2.zero; // Reset position
    }

    public Vector2 GetDirection ()
    {
        Vector2 containerPos = container.anchoredPosition;
        Vector2 gemPos = rectTransform.anchoredPosition;

        return ( gemPos.x < containerPos.x ) ? Vector2.right : Vector2.left;
    }

}
