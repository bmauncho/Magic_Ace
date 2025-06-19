using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class FlipCards : MonoBehaviour
{
    WinLoseManager winLoseManager;
    GridManager gridManager;
    PoolManager poolManager;
    int refill = 0;
    private void Start ()
    {
        winLoseManager = CommandCenter.Instance.winLoseManager_;
        gridManager = CommandCenter.Instance.gridManager_;
        poolManager = CommandCenter.Instance.poolManager_;
    }

    public IEnumerator flipBack ( 
        List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards , 
        List<GameObject> remainingBigJokerCards,
        List<GameObject> remainingSuperJokerCards)
    {
        if (remainingGoldenCards == null || remainingGoldenCards.Count == 0)
        {
            yield break;
        }
        //set Joker Card
        CardManager cardManager = CommandCenter.Instance.cardManager_;
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        RefillApi refillApi = CommandCenter.Instance.apiManager_.refillApi;
        HashSet<string> usedPositions = new HashSet<string>();
        for (int i = 0 ; i < remainingGoldenCards.Count ; i++)
        {
            GameObject cardObject = remainingGoldenCards [i].card;
            int col = remainingGoldenCards [i].Positions [0].col;
            int row = remainingGoldenCards [i].Positions [0].row;
            cardObject.transform.localScale = Vector3.one;
            Card cardComponent = cardObject.GetComponent<Card>();
            CardType cardType = CardType.SMALL_JOKER;
            if (CommandCenter.Instance.gameMode == GameMode.Demo)
            {
                cardType = cardManager.GetRandomJoker(); // Call only once
            }
            else
            {
              
            }

            if (featureManager.GetActiveFeature() == Features.Feature_A)
            {
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions,col,row);
            }
            else if (featureManager.GetActiveFeature() == Features.Feature_B)
            {
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions,col , row);
                Debug.Log(cardType.ToString());
            }
            else if (featureManager.GetActiveFeature() == Features.Feature_C)
            {
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions, col , row);
            }

            cardManager.SetSpecificCard(cardComponent , cardType); // Use same type
            cardComponent.SetCardType(cardType);
            cardComponent.ShowCardBg();

            if (cardComponent.GetCardType() == CardType.BIG_JOKER)
            {
                remainingBigJokerCards.Add(cardObject);
            }

            if(cardComponent.GetCardType() == CardType.SUPER_JOKER)
            {
                remainingSuperJokerCards.Add(cardObject);
            }

            CardDatas cardDatas = new CardDatas
            {
                name = CardType.BIG_JOKER.ToString() ,
                isGolden = false
            };
        }

        Debug.Log("flip back");
        Sequence seq = DOTween.Sequence();
        List<GameObject> remainingBigJoker = new List<GameObject>();
        for (int i = 0 ; i < remainingGoldenCards.Count ; i++)
        {
            GameObject cardObject = remainingGoldenCards [i].card;
            Card cardComponent = cardObject.GetComponent<Card>();

            Sequence cardSeq = DOTween.Sequence();
            if (cardComponent.GetCardType() == CardType.SUPER_JOKER)
            {

                cardSeq.Append(cardObject.transform.DORotate(new Vector3(0 , 90 , 0) , 0.25f)) // rotate back to 90°
                   .AppendCallback(() => cardComponent.HideCardBg())                   // hide after 90°
                   .Append(cardObject.transform.DORotate(new Vector3(0 , 0 , 0) , 0.25f));// rotate back to 0°
            }
            else
            {

                cardSeq.Append(cardObject.transform.DORotate(new Vector3(0 , 90 , 0) , 0.25f)) // rotate back to 90°
                  .AppendCallback(() => cardComponent.HideCardBg())                   // hide after 90°
                  .Append(cardObject.transform.DORotate(new Vector3(0 , 0 , 0) , 0.25f));// rotate back to 0°
            }
            seq.Join(cardSeq);
        }

        yield return seq.WaitForCompletion();
        CommandCenter.Instance.winLoseManager_.winSequence_.SetIsFlipping(false);
        yield return null;
    }

    private CardType SetCardtypeForFeatures ( string cardname , HashSet<string> usedPositions,int col ,int row )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        GameDataApi gameDataApi = CommandCenter.Instance.apiManager_.gameDataApi;

        int refillCount= featureManager.GetRefillCounter();
        Debug.Log($"Refill Count: {refillCount}");
        //Debug.Log($"{diff} / {counter}");
        
        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                string positionKey = $"{i}_{j}";
                if (usedPositions.Contains(positionKey))
                    continue;
                if (col == i && row == j)
                {
                    string name = featureManager.reffillInfo(refillCount) [i].List [j].name;
                    Debug.Log($"Card Name: {name} at position {i},{j}");
                    usedPositions.Add(positionKey);
                    if (Enum.TryParse(typeof(CardType) , name , true , out var cardType))
                    {
                        return (CardType)cardType;
                    }
                }
               
            }
        }

        return CardType.SMALL_JOKER;
    }
}
