using DG.Tweening;
using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    MainMenuController mainMenuController;
    public bool isCollected = false;
    public bool isAnimating = false;
    private float T;
    [SerializeField] private float vectorArc = 10f; // Adjusted for UI units
    [SerializeField] private float duration = 2f;
    private Vector2 dir;
    private Vector2 startPos;
    private RectTransform rectTransform;
    [SerializeField] private RectTransform testTarget;
    public Action OnComplete;
    private void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start ()
    {
        mainMenuController = CommandCenter.Instance.mainMenuController_;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isAnimating) return;
        T += Time.deltaTime;
        float t01 = Mathf.Clamp01(T / duration);
        Vector2 arcOffset = Mathf.Sin(t01 * Mathf.PI) * vectorArc * dir;
        Vector2 newPos = Vector2.Lerp(rectTransform.anchoredPosition , Vector2.zero , t01);
        // Calculate perpendicular direction
        Vector2 direction = ( Vector2.zero - rectTransform.anchoredPosition ).normalized;
        Vector2 perpendicular = new Vector2(-direction.y , direction.x); // 90 degree rotation

        // Apply arc offset
        Vector2 finalPos = newPos + perpendicular * arcOffset;

        rectTransform.anchoredPosition = finalPos;

        if (t01 >= 1)
        {
            isAnimating = false;
            T = 0f; // Reset the timer
            OnCompleted();
        }
    }
    [ContextMenu("Collect Gem")]
    public void collectGemtest ()
    {
        //CollectGem(testTarget);
    }

    public void CollectGem (RectTransform target = null,RectTransform container = null)
    {
        if (isCollected) return;
        isCollected =true;

        dir = GetDirection(container);
        startPos = rectTransform.anchoredPosition;
        rectTransform.SetParent(target);
        isAnimating = true;
    }


    public Vector2 GetDirection ( RectTransform container )
    {
        Vector2 containerPos = container.anchoredPosition;
        Vector2 gemPos = rectTransform.anchoredPosition;

        return ( gemPos.x < containerPos.x ) ? Vector2.right : Vector2.left;
    }

    public void OnCompleted ()
    {
        Debug.Log("Gem collection completed!");
        OnComplete?.Invoke();
    }

    [ContextMenu("Reset Gem")]
    public void ResetGem ()
    {
        isCollected = false;
        isAnimating = false;
        RectTransform parent = CommandCenter.Instance.mainMenuController_.GetSafeArea();
        rectTransform.SetParent(parent);
        rectTransform.anchoredPosition = Vector2.zero; // Reset position
    }
}
