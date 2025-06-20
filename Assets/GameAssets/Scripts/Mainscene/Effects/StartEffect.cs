using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartEffect : MonoBehaviour
{
    [SerializeField] private GameObject [] effects;
    public void showEffect ()
    {
        StartCoroutine(ShowEffectsSequentially());
    }

    private IEnumerator ShowEffectsSequentially ()
    {
        for (int i = 0 ; i < effects.Length ; i++)
        {
            yield return new WaitForSeconds(0.25f);
            CanvasGroup canvasGroup = effects [i].GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1.0f;
            effects [i].SetActive(true);
        }
    }

    public void HideEffect ()
    {
        StartCoroutine(HideEffectsSequentially());
    }

    private IEnumerator HideEffectsSequentially ()
    {
        for (int i = 0 ; i < effects.Length ; i++)
        {
            yield return new WaitForSeconds(0.1f);

            CanvasGroup canvasGroup = effects [i].GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            int capturedIndex = i; // Capture current value of i

            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0 , 0.25f).OnComplete(() =>
                {
                    effects [capturedIndex].SetActive(false);
                });
            }
            else
            {
                effects [i].SetActive(false);
            }
        }
    }

}
