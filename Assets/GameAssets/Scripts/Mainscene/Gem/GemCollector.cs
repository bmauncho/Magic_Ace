using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollector : MonoBehaviour
{
    PoolManager poolManager;
    public RectTransform container;
    public RectTransform [] targetRectTransform;
    private void Start ()
    {
        poolManager = CommandCenter.Instance.poolManager_;
    }
    public IEnumerator CollectGems ( List<GameObject> remainingSuperJokerCards )
    {
        if (remainingSuperJokerCards == null || remainingSuperJokerCards.Count == 0)
        {
            Debug.LogWarning("No Super Joker cards to collect gems from.");
            yield break;
        }

        if (IsAllGemsCollected())
        {
            Debug.LogWarning("All gems are already collected!");
            yield break;
        }

        Sequence seq = DOTween.Sequence();

        // Determine how many slots are available
        int availableSlots = 0;
        foreach (var target in targetRectTransform)
        {
            GemSlot gemSlot = target.GetComponent<GemSlot>();
            if (gemSlot != null && !gemSlot.IsOwnerAvailable())
            {
                availableSlots++;
            }
            else if (gemSlot == null) // No gem slot component, assume available
            {
                availableSlots++;
            }
        }

        int gemsToCollect = Mathf.Min(availableSlots , remainingSuperJokerCards.Count);
        int activeAnims = gemsToCollect;

        int collected = 0;
        foreach (var card in remainingSuperJokerCards)
        {
            if (collected >= gemsToCollect)
                break;

            Card cardComponent = card.GetComponent<Card>();
            RectTransform cardRect = cardComponent.GetCard();
            RectTransform parent = cardComponent.GetCardImgHolder();
            Vector3 cardPos = cardRect.transform.position;

            GameObject Gem = poolManager.GetFromPool(PoolType.Gems , cardPos , Quaternion.identity , parent.transform);
            Gem gemComponent = Gem.GetComponent<Gem>();

            // jump to target
            RectTransform target = GetTarget();
            if (target == null)
                continue;

            GemSlot gemSlotComponent = target.GetComponent<GemSlot>();
            if (gemSlotComponent != null)
            {
                gemSlotComponent.AddOwner(gemComponent.gameObject);
            }

            gemComponent.CollectGem(target , container);
            gemComponent.OnComplete += () => activeAnims--;

            collected++;
        }

        yield return new WaitUntil(() => activeAnims <= 0);
        Debug.Log("All allowed gems collected!");
    }


    private RectTransform GetTarget ()
    {
        foreach (var target in targetRectTransform)
        {
            if (target)
            {
                GemSlot gemSlot = target.GetComponent<GemSlot>();
                if (gemSlot)
                {
                    if (!gemSlot.IsOwnerAvailable())
                    {
                        return target;
                    }
                }
                else
                {
                    return target; // Return the first available target if no GemSlot component is found
                }
            }
        }

        return targetRectTransform [0];
    }

    public bool IsAllGemsCollected ()
    {
        int gemsCollected = 0;
        foreach (var target in targetRectTransform)
        {
            GemSlot gemSlot = target.GetComponent<GemSlot>();
            if (gemSlot && gemSlot.IsOwnerAvailable())
            {
                gemsCollected++;
            }
        }
        if(gemsCollected >= targetRectTransform.Length)
        {
            Debug.Log("All gems collected!");
            return true; // All gems are collected
        }
        return false; 
    }

}
