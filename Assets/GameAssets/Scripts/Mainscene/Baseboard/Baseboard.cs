using System.Collections;
using UnityEngine;

public class Baseboard : MonoBehaviour
{
    public GameObject Gems;
    public GameObject FreeGameGems;
    [Header("Baseboard Effects")]
    public GameObject FreeGem_CollectFx;
    public GameObject Upgrade_Tickets;
    public GameObject UpgradeUi;
    int usedTickets = 0;
    public FG_Ticket [] UpgradeTickets;
    public GemSlot [] gems;

    [ContextMenu("Upgrade Multipliers")]
    public void ActivateUpgradeMultipliers ()
    {
        Upgrade_Tickets.SetActive(true);
        ResetGems();
        Gems.SetActive(false);
        CommandCenter.Instance.multiplierManager_.UpgradeAnim();
    }

    IEnumerator ShowUpgradeUI ()
    {
        UpgradeUi.SetActive(true);
        yield return new WaitForSeconds(1f);
        UpgradeUi.SetActive(false);
    }

    public void DeactivateUpgradeMultipliers ()
    {
        Upgrade_Tickets.SetActive(false);
        Gems.SetActive(true);
        if(usedTickets > 0)
        {
            usedTickets = 0;
        }
    }

    public void UseTicket ()
    {
        for (int i = UpgradeTickets.Length - 1 ; i >= 0 ; i--)
        {
            if (UpgradeTickets [i] != null && !UpgradeTickets [i].isTicketUsed)
            {
                UpgradeTickets [i].PlayTicket_UsedAnimation();
                UpgradeTickets [i].isTicketUsed = true;
                usedTickets++;
                CommandCenter.Instance.soundManager_.PlaySound("Base_Ticket_Spent");
                break;
            }
        }
    }

    public void ResetGems ()
    {
        if (gems == null || gems.Length == 0) return;
        foreach (var gem in gems)
        {
            if (gem != null)
            {
                CommandCenter.Instance.poolManager_.ReturnToPool(PoolType.Gems,gem.GetTheOwner());
                gem.RemoveOwner();
            }
        }
    }

}
