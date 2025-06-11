using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FreeSpinRetrigger : MonoBehaviour
{
    public Animator animator_;
    public GameObject no_FreeSpins;
    public GameObject free_Spins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator retriggerFreeSpin ()
    {
        ResetRetrigger();
        Animator animator = animator_;
        if (animator != null)
        {
            // Wait until the animation state is "freeSpinReTriggerAnim"
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("freeSpinReTriggerAnim"))
            {
                yield return null;
            }

            // Wait until the animation finishes
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(.25f);
        free_Spins.SetActive(true);
        no_FreeSpins.SetActive(true);
    }


    public void ResetRetrigger ()
    {
        free_Spins.SetActive(false);
        no_FreeSpins.SetActive(false);
    }
}
