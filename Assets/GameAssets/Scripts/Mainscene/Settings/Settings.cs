using DG.Tweening;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private RectTransform settingsMenu;
    [SerializeField] private bool isSettingsMenuActive = false;
    [SerializeField] private bool isAnimating = false;

    private readonly Vector2 hiddenPosition = new Vector2(0 , -110f);
    private readonly Vector2 visiblePosition = Vector2.zero;
    private const float animationDuration = 0.25f;

    private void Start ()
    {
        settingsMenu.anchoredPosition = hiddenPosition;
        settings.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
    }

    private void ShowSettings ()
    {
        if (isAnimating) return; // Prevent multiple calls
        isAnimating = true;

        isSettingsMenuActive = true;
        settings.SetActive(true);
        settingsMenu.gameObject.SetActive(true);

        settingsMenu.DOAnchorPos(visiblePosition , animationDuration).OnComplete(() =>
        {
            isAnimating = false;
        });
    }

    private void HideSettings ()
    {
        if (isAnimating) return; // Prevent multiple calls
        isAnimating = true;

        settingsMenu.DOAnchorPos(hiddenPosition , animationDuration).OnComplete(() =>
        {
            isSettingsMenuActive = false;
            settingsMenu.gameObject.SetActive(false);
            settings.SetActive(false);
            isAnimating = false;
        });
    }

    public void ToggleSettings ()
    {
        if (isAnimating) return; // Prevent spam-clicking
        if (isSettingsMenuActive)
        {
            HideSettings();
        }
        else
        {
            ShowSettings();
        }
    }
}
