using UnityEngine;

public class SpinAnims : MonoBehaviour
{
    public GameObject spinAnimObj;
    public GameObject idleSpinAnimObj;
    public Animator SpinAnim;
    public Animator SpinEffect;
    public Animator idleSpinAnim;
  
    public void PlaySpinAnim ()
    {
        if (SpinAnim != null)
        {
            spinAnimObj.SetActive(true);
            SpinAnim.Rebind();
            SpinEffect.Rebind();
        }

        Invoke(nameof(StopSpinAnim) , 1.2f); // Stop the animation after 1 second
    }

    public void StopSpinAnim ()
    {
        if (SpinAnim != null)
        {
            spinAnimObj.SetActive(false);
            SpinAnim.Rebind();
            SpinEffect.Rebind();
        }
    }

    public void PlayIdleSpinAnim ()
    {
        if (idleSpinAnim != null)
        {
            idleSpinAnimObj.SetActive(true);
            idleSpinAnim.Rebind();
        }
    }

    public void StopIdleSpinAnim ()
    {
        if (idleSpinAnim != null)
        {
            idleSpinAnimObj.SetActive(false);
            idleSpinAnim.Rebind();
        }
    }
}
