using UnityEngine;

public class ExtraBetAnim : MonoBehaviour
{
    [SerializeField]private ExtraBetMenu extraBetMenu;
    
    public void ShowAnims ()
    {
        extraBetMenu.banner.GetComponent<Animator>().Play("ExtarBetbannerAnim");
        extraBetMenu.Banner_light.SetActive(true);
        extraBetMenu.Holder_1.SetActive(true);
        extraBetMenu.Holder_2.SetActive(true);
        extraBetMenu.Ticket.SetActive(true);
        Invoke(nameof(HideAnims) , 1f); 
    }

    public void HideAnims ()
    {
        extraBetMenu.banner.GetComponent<Animator>().Rebind();
        extraBetMenu.TopBannerGems.SetActive(false);
        extraBetMenu.Banner_light.SetActive(false);
        extraBetMenu.Holder_1.SetActive(false);
        extraBetMenu.Holder_2.SetActive(false);
    }
}
