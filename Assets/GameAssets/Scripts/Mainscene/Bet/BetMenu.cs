using DG.Tweening;
using UnityEngine;

public class BetMenu : MonoBehaviour
{
    [SerializeField] private bool IsBetMenUActive = false;
    [SerializeField] private GameObject betMenu;
    [SerializeField] private RectTransform betOptions;

    private void showBetMenu ()
    {
        IsBetMenUActive = true;
        betMenu.SetActive(true);
        betOptions.gameObject.SetActive(true);
        betOptions.DOAnchorPos(Vector2.zero , .25f);
    }

    private void hideBetMenu ()
    {
        IsBetMenUActive = false;
        Vector2 target = new Vector2(0 , -300f);
        betOptions.DOAnchorPos(target , .25f).OnComplete(() =>
        {
            betOptions.gameObject.SetActive(false);
            betMenu.SetActive(false);
        });
    }

    public void ToggleBetMenu ()
    {
        if (IsBetMenUActive)
        {
            hideBetMenu();
        }
        else
        {
            showBetMenu();
        }
    }
}
