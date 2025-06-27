using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HintInfo
{
    public TheLanguage language;
    public Sprite [] hint;
    public Sprite OneMoreScatter;
    public Sprite FreeSpinWon;
}

public class HintsManager : MonoBehaviour
{
    public TheLanguage currentLanguage;
    public HintInfo [] hintsInfo;
    public Hint hint;
    [SerializeField] private float duration = 3f;
    public HintWinUI hintWinUI;
    private Coroutine hintsCoroutine;
    Tween tween;
    int hintIndex = 0;
    private void Start ()
    {
        if (LanguageMan.instance)
        {
            currentLanguage = LanguageMan.instance.ActiveLanguage;
            LanguageMan.instance.onLanguageRefresh.AddListener(ChangeTheActiveLanguage);
        }

        StartHintsCoroutine();
    }

    private void Update ()
    {

    }

    [ContextMenu("Show Hints")]
    public void ShowHints ()
    {
        StartHintsCoroutine();
    }

    private IEnumerator HintsSequence ()
    {
        HintInfo currentHintInfo = GetHintInfo(currentLanguage);
        int currentIndex = 0;

        if (currentHintInfo == null || currentHintInfo.hint.Length == 0)
        {
            Debug.LogError("No hints available. Cannot continue.");
            yield break;
        }

        hint.gameObject.SetActive(true);

        while (true)
        {
            currentHintInfo = GetHintInfo(currentLanguage);

            if (currentHintInfo == null || currentHintInfo.hint.Length == 0)
            {
                Debug.LogError("No hints available. Cannot continue.");
                yield break;
            }
            // Set current hint sprite
            hint.hintImage.sprite = currentHintInfo.hint [currentIndex];

            // Reset position instantly before animation
            hint.hintRect.anchoredPosition = new Vector2(0 , hint.hintRect.anchoredPosition.y);

            // Animate to left
            tween = hint.hintRect.DOAnchorPosX(-700 , duration)
                .SetDelay(1f);

            // Wait for animation to complete
            yield return tween.WaitForCompletion();

            // Wait a short pause before resetting (if needed)
            yield return new WaitForSeconds(0.1f); // Optional smoother pacing

            // Next hint
            currentIndex = ( currentIndex + 1 ) % currentHintInfo.hint.Length;
            hintIndex = currentIndex;
        }
    }


    private HintInfo GetHintInfo ( TheLanguage lang )
    {
        return Array.Find(hintsInfo , h => h.language == lang);
    }

    [ContextMenu("Stop Coroutine")]
    public void StopHintsCoroutine ()
    {
        if (hintsCoroutine != null)
        {
            StopCoroutine(hintsCoroutine);
            hintsCoroutine = null;
        }
    }

    public void StartHintsCoroutine ()
    {
        StopHintsCoroutine();
        hintsCoroutine = StartCoroutine(HintsSequence());
    }
    [ContextMenu("Show Extra Hint")]
    public void ShowExtraHint ()
    {
        StopHintsCoroutine();
        hint.Hints.SetActive(false);
        hint.ExtraHints.SetActive(true);
        hint.wintextholder.SetActive(false);
        StartCoroutine(setextraHint());
    }

    public IEnumerator setextraHint( )
    {
        yield return null;
    }
    [ContextMenu("Hide Extra Hint")]
    public void hideExtrahint ()
    {
        hint.ExtraHints.SetActive(false);
        hint.Hints.SetActive(true);
        hint.wintextholder.SetActive(false);
        StartHintsCoroutine();
    }
    [ContextMenu("Show Win Text")]
    public void ShowWinText ()
    {
        StopHintsCoroutine();
        hint.Hints.SetActive(false);
        
    }
    [ContextMenu("Hide Win Text")]
    public void HideWinText ()
    {
        hint.Hints.SetActive(true);
        StartHintsCoroutine();
    }

    public bool IsLanguageChanged ()
    {
        return currentLanguage != LanguageMan.instance.ActiveLanguage;
    }


    public void ChangeTheActiveLanguage ()
    {
        HintInfo currentHintInfo = GetHintInfo(currentLanguage);
        int currentIndex = 0;
        if (IsLanguageChanged())
        {
            Debug.Log("Language changed. Updating hints.");
            currentLanguage = LanguageMan.instance.ActiveLanguage;
            currentHintInfo = GetHintInfo(currentLanguage);
            if (currentHintInfo == null || currentHintInfo.hint.Length == 0)
            {
                Debug.LogWarning("No hints for new language. Using first available.");
                currentHintInfo = hintsInfo.Length > 0 ? hintsInfo [0] : null;
                if (currentHintInfo == null)
                {
                    Debug.LogError("No fallback hints found.");
                }
            }
            currentIndex = hintIndex;
        }

        hint.hintImage.sprite = currentHintInfo.hint [currentIndex];
    }
}
