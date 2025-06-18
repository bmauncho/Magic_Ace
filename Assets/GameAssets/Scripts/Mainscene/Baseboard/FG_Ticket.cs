using UnityEngine;

public class FG_Ticket : MonoBehaviour
{
    public Animator TicketAnimator;
    public bool isTicketUsed = false;
    private void OnEnable ()
    {
        ResetTicketAnim();
    }
    public void PlayTicket_UsedAnimation ()
    {
        if (TicketAnimator != null)
        {
            TicketAnimator.Rebind();
            TicketAnimator.Play("Collected_Ticket_Used");
        }
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
