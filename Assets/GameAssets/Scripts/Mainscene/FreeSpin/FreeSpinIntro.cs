using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FreeSpinIntro : MonoBehaviour
{
    public GameObject freespinInfo;
    public GameObject freeSpinTicketEffect;
    public IEnumerator ShowFreeSpinIntro ()
    {
        ResetIntro();
        fadeInParent();
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(true);
        StartCoroutine(ShowFreeSpinEffectCoroutine());
        fadeInInfo();
        // Additional logic for showing the free spin intro can be added here
    }

    private IEnumerator ShowFreeSpinEffectCoroutine ()
    {
        yield return new WaitForSeconds(0.25f);
        freeSpinTicketEffect.SetActive(true);

        Animator animator = freeSpinTicketEffect.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            // Wait until the animation state is "FreeSpinTicketEffect"
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FreeSpinTicketEffect"))
            {
                yield return null;
            }

            // Wait until the animation finishes
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        freeSpinTicketEffect.SetActive(false);
    }

    private void fadeInParent ()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Tween myTween = canvasGroup.DOFade(1 , 0.5f);
    }

    private void fadeInInfo ()
    {
        CanvasGroup canvasgroup2 = freespinInfo.GetComponent<CanvasGroup>();
        canvasgroup2.alpha = 0;
        Tween myTween2 = canvasgroup2.DOFade(1 , 0.5f);
    }

    public void ResetIntro ()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        CanvasGroup canvasgroup2 = freespinInfo.GetComponent<CanvasGroup>();
        canvasgroup2.alpha = 0;
        freeSpinTicketEffect.GetComponentInChildren<Animator>().Rebind();
    }
}
