using System;
using System.Collections;
using UnityEngine;

public class FG_Ticket : MonoBehaviour
{
    public GameObject Ticket;
    public Animator TicketAnimator;
    public bool isTicketUsed = false;
    private void OnEnable ()
    {
        ResetTicketAnim();
    }
    public void PlayTicket_UsedAnimation ()
    {
        Ticket.SetActive(true);
        if (TicketAnimator != null)
        {
            TicketAnimator.Rebind();
            TicketAnimator.Play("Collected_Ticket_Used");
            CommandCenter.Instance.soundManager_.PlaySound("Ticket_Used");
            StartCoroutine(waitAndDisable());
        }
    }
    IEnumerator waitAndDisable ()
    {
        // Wait until the animation state is "freeSpinReTriggerAnim"
        while (!TicketAnimator.GetCurrentAnimatorStateInfo(0).IsName("Collected_Ticket_Used"))
        {
            yield return null;
        }

        // Wait until the animation finishes
        while (TicketAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Ticket.SetActive(false);
        yield return null;
    }

    public void ResetTicketAnim ()
    {
        if (TicketAnimator != null)
        {
            TicketAnimator.Rebind();
            TicketAnimator.Play("Collected_Ticket_Fx");
        }
    }
}
