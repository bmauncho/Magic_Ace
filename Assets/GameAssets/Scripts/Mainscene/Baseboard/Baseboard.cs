using UnityEngine;

public class Baseboard : MonoBehaviour
{
    public GameObject Gems;
    public GameObject FreeGameGems;
    [Header("Baseboard Effects")]
    public GameObject FreeGem_CollectFx;
    public GameObject Upgrade_Tickets;
    int usedTickets = 0;
    public FG_Ticket [] UpgradeTickets;
    public GemSlot [] gems;

    public void ActivateUpgradeMultipliers ()
    {
        Upgrade_Tickets.SetActive(true);
        ResetGems();
        Gems.SetActive(false);
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
