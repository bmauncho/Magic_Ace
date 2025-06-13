using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BetLimitsInfo : MonoBehaviour
{
    public TMP_Text modeName;

    public void SetText( string text )
    {
        if (modeName != null)
        {
            modeName.text = text;
        }
    }

    public IEnumerator bounce ()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind();
            // Wait until the animation state is "freeSpinReTriggerAnim"
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("BetlimitsAnim"))
            {
                yield return null;
            }

            // Wait until the animation finishes
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }
        yield return null;
    }
}
