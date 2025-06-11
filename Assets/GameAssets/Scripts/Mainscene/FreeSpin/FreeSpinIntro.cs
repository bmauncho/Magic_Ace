using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FreeSpinIntro : MonoBehaviour
{
    public GameObject freespinInfo;
    public GameObject freeSpinTicketEffect;
    public GameObject StartBtn;
    [Header("Effects")]
    public GameObject Light_1;
    public GameObject Light_2;
    
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
        freeSpinTicketEffect.SetActive(true);
        Light_1.gameObject.SetActive(true);
        Animator animator = freeSpinTicketEffect.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            // Wait until the animation state is "FreeSpinTicketEffect"
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("IntroTicketEffect"))
            {
                yield return null;
            }

            // Wait until the animation finishes
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(0.25f);
        freeSpinTicketEffect.SetActive(false);
        yield return new WaitForSeconds(1f);
        Light_1.GetComponent<Animator>().Play("IntroLightEffect_1_breathe");
        Light_2.gameObject.SetActive(true);
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
        Light_1.GetComponent<Animator>().Rebind();
        Light_2.GetComponentInChildren<Animator>().Rebind();
        Light_1.gameObject.SetActive(false);
        Light_2.gameObject.SetActive(false);
    }

    public void DeactivateFreeIntro ()
    {
        ResetIntro();
        gameObject.SetActive(false);
    }
}
