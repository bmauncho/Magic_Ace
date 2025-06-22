using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    PoolManager effectsPoolMan;
    [SerializeField] private GameObject SlotsHolder;
    [SerializeField] private List<CardPos> CardEffectsSlots = new List<CardPos>();

    private void Start ()
    {
        effectsPoolMan = CommandCenter.Instance.poolManager_;
    }
    [ContextMenu("InitializeWinSlots")]
    void InitializeWinSlots ()
    {
        List<GameObject> items = new List<GameObject>();
        foreach (Transform t in SlotsHolder.transform)
        {
            items.Add(t.gameObject);
        }

        CardEffectsSlots.Clear();
        for (int col = 0 ; col < 5 ; col++)
        {
            CardEffectsSlots.Add(new CardPos());
        }
        int index = 0;
        for (int col = 0 ; col< CardEffectsSlots.Count ; col++)
        {
            for (int row = 0 ; row < 4 ; row++)
            {
                CardEffectsSlots [col].CardPosInRow.Add(items [index]);
                index++;
            }
        }
    }

    public List<CardPos> GetGridCards ()
    {
        return CardEffectsSlots;
    }

    public void ShowWinEffect(List<winCardData> WinningCards )
    {
        for(int i = 0 ; i < WinningCards.Count ; i++)
        {
            for (int j = 0 ; j < CardEffectsSlots.Count ; j++)
            {
                for (int k = 0 ; k < CardEffectsSlots [j].CardPosInRow.Count ; k++)
                {
                    if (WinningCards [i].col ==j && WinningCards [i].row == k)
                    {
                        if(WinningCards [i].name == CardType.SCATTER.ToString())
                        {
                            CardEffectsSlots [j].CardPosInRow [k].SetActive(false);
                            continue;
                        }
                        Vector3 pos = CardEffectsSlots [j].CardPosInRow [k].transform.position;
                        Transform parent = CardEffectsSlots [j].CardPosInRow [k].transform;
                        GameObject effect = effectsPoolMan.GetFromPool(PoolType.Fx,pos , Quaternion.identity , parent);
                        EffectsSlots effectsSlots = CardEffectsSlots [j].CardPosInRow [k].GetComponent<EffectsSlots>();
                        effectsSlots.AddOwner(effect);
                        CardEffectsSlots [j].CardPosInRow [k].SetActive(true);
                    }
                }
            }
        }

        Invoke(nameof(HideEffect) , 1f);
    }

    private void HideEffect ()
    {
        for (int i = 0 ; i < CardEffectsSlots.Count ; i++)
        {
            for (int j = 0 ; j < CardEffectsSlots [i].CardPosInRow.Count ; j++)
            {
                GameObject obj = CardEffectsSlots [i].CardPosInRow [j];
                if (obj.activeSelf)
                {
                    EffectsSlots effectsSlots = obj.GetComponent<EffectsSlots>();
                    GameObject effectObj = obj.GetComponent<EffectsSlots>().GetTheOwner();
                    if (effectObj != null)
                    {
                        effectsPoolMan.ReturnToPool(PoolType.Fx,effectObj);
                        effectsSlots.RemoveOwner();
                    }
                    CardEffectsSlots [i].CardPosInRow [j].SetActive(false);
                }
            }
        }
    }
}
