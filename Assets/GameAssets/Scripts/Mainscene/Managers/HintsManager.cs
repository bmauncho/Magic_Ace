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

    private Coroutine hintsCoroutine;
    Tween tween;
    private void Start ()
    {
        if (LanguageMan.instance)
        {
            currentLanguage = LanguageMan.instance.ActiveLanguage;
        }

        StartHintsCoroutine();
    }

    private void Update ()
    {
        if (LanguageMan.instance && currentLanguage != LanguageMan.instance.ActiveLanguage)
        {
            Debug.Log("Language changed to: " + LanguageMan.instance.ActiveLanguage);
            currentLanguage = LanguageMan.instance.ActiveLanguage;
        }
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
            // Detect language change dynamically
            if (LanguageMan.instance && currentLanguage != LanguageMan.instance.ActiveLanguage)
            {
                Debug.Log("Language changed during coroutine. Updating hints.");
                currentLanguage = LanguageMan.instance.ActiveLanguage;
                currentHintInfo = GetHintInfo(currentLanguage);

                if (currentHintInfo == null || currentHintInfo.hint.Length == 0)
                {
                    Debug.LogWarning("No hints for new language. Using first available.");
                    currentHintInfo = hintsInfo.Length > 0 ? hintsInfo [0] : null;

                    if (currentHintInfo == null)
                    {
                        Debug.LogError("No fallback hints found.");
                        yield break;
                    }
                }

                currentIndex = 0;
            }

            // Show hint
            hint.hintImage.sprite = currentHintInfo.hint [currentIndex];

            // Animate
            hint.hintRect.anchoredPosition = new Vector2(0 , hint.hintRect.anchoredPosition.y);
            tween = hint.hintRect.DOAnchorPosX(-700 , duration)
                .SetDelay(2f)
                .OnComplete(() =>
                {
                    hint.hintRect.anchoredPosition = new Vector2(0 , hint.hintRect.anchoredPosition.y);
                    hint.hintRect.gameObject.SetActive(false);
                });

            Debug.Log($"Showing hint: {currentHintInfo.hint [currentIndex].name}");

            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(2f);
            hint.hintRect.gameObject.SetActive(true);

            // Next hint
            currentIndex = ( currentIndex + 1 ) % currentHintInfo.hint.Length;
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
        hint.wintextholder.SetActive(true);
        hint.winText.gameObject.SetActive(true);
    }
    [ContextMenu("Hide Win Text")]
    public void hideWinText ()
    {
        hint.Hints.SetActive(true);
        hint.wintextholder.SetActive(false);
        hint.winText.gameObject.SetActive(false);
        StartHintsCoroutine();
    }
}
