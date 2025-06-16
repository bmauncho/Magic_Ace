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
        //yield return new WaitForSeconds(.5f);
        //set Joker Card
        CardManager cardManager = CommandCenter.Instance.cardManager_;
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        RefillApi refillApi = CommandCenter.Instance.apiManager_.refillApi;
        HashSet<string> usedPositions = new HashSet<string>();
        for (int i = 0 ; i < remainingGoldenCards.Count ; i++)
        {
            GameObject cardObject = remainingGoldenCards [i].card;
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
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions);
            }
            else if (featureManager.GetActiveFeature() == Features.Feature_B)
            {
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions);
            }
            else if (featureManager.GetActiveFeature() == Features.Feature_C)
            {
                cardType = SetCardtypeForFeatures(cardComponent.GetCardType().ToString() , usedPositions);
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
            //CommandCenter.Instance.apiManager_.gameDataApi.UpdateGameDataApiList(cardDatas);
        }

        //yield return new WaitForSeconds(.25f);
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
        var activeFeature = featureManager.GetActiveFeature();
        int counter = featureManager.GetRefillCounter();

        switch (activeFeature)
        {

            case Features.Feature_A:
            case Features.Feature_B:
                if (counter == 3)
                {
                    // Debug.Log("Refill");
                    CommandCenter.Instance.featureManager_.IncreaseRefillCounter();
                }
                break;

            case Features.Feature_C:
                if (counter == 1 && refill == 1)
                {
                    CommandCenter.Instance.featureManager_.IncreaseRefillCounter();
                }
                refill++;
                break;
        }
        CommandCenter.Instance.winLoseManager_.winSequence_.SetIsFlipping(false);
        yield return null;
    }

    private CardType SetCardtypeForFeatures ( string cardname , HashSet<string> usedPositions )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        GameDataApi gameDataApi = CommandCenter.Instance.apiManager_.gameDataApi;

        int diff = featureManager.GetRefillCounter() - 1;
        int counter = featureManager.GetRefillCounter();
        int spinDiff = featureManager.GetSpinCouter() - 1;
        int spinCounter = featureManager.GetSpinCouter();
        //Debug.Log($"{diff} / {counter}");

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                string positionKey = $"{i}_{j}";
                if (usedPositions.Contains(positionKey))
                    continue;
                if (diff <= 0)
                {
                    //Debug.Log(1);
                    if (featureManager.GetActiveFeature() == Features.Feature_C)
                    {
                        var refillCard = featureManager.reffillInfo(diff) [i].List [j];
                        if (refillCard.isGolden && refillCard.name == cardname)
                        {
                            //Debug.Log("Found!");
                            name = featureManager.reffillInfo(counter) [i].List [j].name;

                            // Debug.Log(2.4);

                            // Debug.Log($"name {name}, [row {i} : col {j}]");
                            usedPositions.Add(positionKey);
                            if (Enum.TryParse(typeof(CardType) , name , true , out var cardType))
                            {
                                return (CardType)cardType;
                            }
                        }
                    }
                    else
                    {
                        var gridCard = gameDataApi.GetGridCardInfo() [i].List [j];
                        if (gridCard.isGolden && gridCard.name == cardname)
                        {
                            Debug.Log(1.1);
                            string name = featureManager.reffillInfo(counter) [i].List [j].name;
                            //Debug.Log($"name {name}, [row {i} : col {j}]");
                            usedPositions.Add(positionKey);
                            if (Enum.TryParse(typeof(CardType) , name , true , out var cardType))
                            {
                                return (CardType)cardType;
                            }
                        }
                    }

                }
                else
                {
                    var refillCard = featureManager.reffillInfo(diff) [i].List [j];
                    var spinCard = featureManager.spinInfo(spinDiff) [i].List [j];
                    var name = string.Empty;
                    // Debug.Log(2.3);
                    if (refillCard.isGolden && refillCard.name == cardname)
                    {
                        //Debug.Log("Found!");
                        name = featureManager.reffillInfo(counter) [i].List [j].name;

                        //Debug.Log(2.4);

                        //Debug.Log($"name {name}, [row {i} : col {j}]");
                        usedPositions.Add(positionKey);
                        if (Enum.TryParse(typeof(CardType) , name , true , out var cardType))
                        {
                            return (CardType)cardType;
                        }
                    }

                }
            }
        }

        return CardType.SMALL_JOKER;
    }

    public IEnumerator JumpBigJokerCards ( List<GameObject> BigJokerCards )
    {
        //Debug.Log("jump BigJoker");
        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());
        List<(int col, int row)> AvailablePositions = new List<(int col, int row)>();

        if (CommandCenter.Instance.gameMode == GameMode.Demo)
        {
            //Debug.Log("Demo Mode");

            // Find available positions that don't have any Joker/Wild cards
            for (int i = 0 ; i < CardSlots.Count ; i++)
            {
                for (int j = 0 ; j < CardSlots [i].CardPosInRow.Count ; j++)
                {
                    Slot slot = CardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                    if (slot != null && slot.GetTheOwner() != null)
                    {
                        Card card = slot.GetTheOwner().GetComponent<Card>();
                        if (card != null &&
                            card.GetCardType() != CardType.SUPER_JOKER &&
                            card.GetCardType() != CardType.BIG_JOKER &&
                            card.GetCardType() != CardType.SMALL_JOKER &&
                            card.GetCardType() != CardType.SCATTER)
                        {
                            AvailablePositions.Add((i, j));
                        }
                    }
                }
            }
            yield return JumpSequence(BigJokerCards , AvailablePositions);
        }
        else
        {
            //Debug.Log("Real Mode");
            List<(int col, int row)> newAvailablePositions = new List<(int col, int row)>();
            RefillApi refillApi = CommandCenter.Instance.apiManager_.refillApi;
            JokerData [] jokerData = refillApi.RefillResponse().gameState.specialSymbols.jokerCards;
            // Find available positions that don't have any Joker/Wild cards
            for (int i = 0 ; i < CardSlots.Count ; i++)
            {
                for (int j = 0 ; j < CardSlots [i].CardPosInRow.Count ; j++)
                {
                    Slot slot = CardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                    if (slot != null && slot.GetTheOwner() != null)
                    {
                        Card card = slot.GetTheOwner().GetComponent<Card>();
                        if (card != null &&
                            card.GetCardType() != CardType.SUPER_JOKER &&
                            card.GetCardType() != CardType.BIG_JOKER &&
                            card.GetCardType() != CardType.SMALL_JOKER &&
                            card.GetCardType() != CardType.SCATTER)
                        {
                            AvailablePositions.Add((i, j));
                        }
                    }
                }
            }


            for (int i = 0 ; i < jokerData.Length ; i++)
            {
                int col = jokerData [i].position.reel;
                int row = jokerData [i].position.row;
                for (int j = 0 ; j < AvailablePositions.Count ; j++)
                {
                    if (col == AvailablePositions [j].col && row == AvailablePositions [j].row)
                    {
                        newAvailablePositions.Add(AvailablePositions [j]);
                    }
                }
            }
            yield return JumpSequence(BigJokerCards , newAvailablePositions);
        }

    }

    private IEnumerator JumpSequence ( List<GameObject> BigJokerCards , List<(int col, int row)> AvailablePositions )
    {

        Debug.Log("JumpSequence");
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        PoolManager poolManager = CommandCenter.Instance.poolManager_;

        List<CardPos> CardSlots = new List<CardPos>(gridManager.GetGridCards());

        HashSet<(int col, int row)> usedIndices = new HashSet<(int col, int row)>();

        HashSet<(int col, int row)> indexesList = new HashSet<(int col, int row)>();

        int bigJokerCount = BigJokerCards.Count;

        var random = new Random();
        indexesList.Clear(); // Clear old indexes

        if (featureManager.GetActiveFeature() == Features.Feature_A)
        {
            // Debug.Log("Feature_A Jump");
            for (int i = 0 ; i < bigJokerCount ; i++)
            {
                List<(int, int)> holderList = new List<(int, int)>(GetJumpPositions());
                //depending on which refill set jumpPoints
                yield return StartCoroutine(jumpBigJokerCards(BigJokerCards [i] , holderList , CardSlots));
            }
        }
        else if (featureManager.GetActiveFeature() == Features.Feature_B)
        {
            //Debug.Log("Feature_B Jump");
            for (int i = 0 ; i < bigJokerCount ; i++)
            {
                List<(int, int)> holderList = new List<(int, int)>(GetJumpPositions());
                //depending on which refill set jumpPoints
                yield return StartCoroutine(jumpBigJokerCards(BigJokerCards [i] , holderList , CardSlots));
            }
        }
        else if (featureManager.GetActiveFeature() == Features.Feature_C)
        {
            //Debug.Log("Feature_C Jump");
            for (int i = 0 ; i < bigJokerCount ; i++)
            {
                List<(int, int)> holderList = new List<(int, int)>(GetJumpPositions());
                //depending on which refill set jumpPoints
                yield return StartCoroutine(jumpBigJokerCards(BigJokerCards [i] , holderList , CardSlots));
            }
        }
        else
        {
            //Debug.Log("Normal Jump");
            foreach (var bigjoker in BigJokerCards)
            {
                foreach (var pos in AvailablePositions.Where(p => !usedIndices.Contains(p)).OrderBy(x => random.Next()).Take(2))
                {
                    indexesList.Add(pos);
                }
            }

            List<(int col, int row)> newindexesList = new List<(int col, int row)>();
            foreach (var holder in indexesList)
            {
                newindexesList.Add(holder);
            }

            int currentIndex = 0;

            int completed = 0;

            for (int i = 0 ; i < bigJokerCount ; i++)
            {
                int indicesPerCard = indexesList.Count / bigJokerCount;

                int extraIndices = indexesList.Count % bigJokerCount;
                List<(int, int)> holderList = new List<(int, int)>();
                int countToAssign = indicesPerCard + ( i < extraIndices ? 1 : 0 );
                //Debug.Log("count to Asign : " + countToAssign);
                for (int j = 0 ; j < countToAssign && currentIndex < indexesList.Count ; j++)
                {
                    //Debug.Log($"{usedIndices.Count} Usedindexes list");
                    if (!usedIndices.Contains(newindexesList [currentIndex]))
                    {
                        //Debug.Log("adding jump indexes");
                        holderList.Add(newindexesList [currentIndex]);
                        usedIndices.Add(newindexesList [currentIndex]);
                        currentIndex++;
                    }
                }
                //Debug.Log($"holder Count {holderList.Count}");
                if (holderList.Count == 0)
                {
                    completed++;
                    continue;
                }

                yield return StartCoroutine(jumpBigJokerCards(BigJokerCards [i] , holderList , CardSlots));
            }
        }


        yield return null;
    }

    private List<(int, int)> GetJumpPositions ()
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        GameDataApi gameDataApi = CommandCenter.Instance.apiManager_.gameDataApi;
        List<(int, int)> jumpPos = new List<(int, int)>();
        int diff = featureManager.GetRefillCounter() - 1;
        int counter = featureManager.GetRefillCounter();

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                var refillCard = featureManager.reffillInfo(diff) [i].List [j];
                if (refillCard.substitute == CardType.BIG_JOKER.ToString())
                {
                    jumpPos.Add((i, j));
                }
            }
        }
        return jumpPos;
    }

    public IEnumerator jumpBigJokerCards ( GameObject target , List<(int, int)> jumpCards , List<CardPos> CardSlots )
    {
        Debug.Log($"Jumping");
        if (jumpCards.Count <= 0)
        {
            yield break;
        }

        // Store initial position and rotation
        Vector3 initialPosition = target.transform.position;
        Quaternion initialRotation = target.transform.rotation;
        List<CardPos> winningCardSlots = new List<CardPos>(winLoseManager.GetWinCardSlots());
        List<(GameObject card, Transform parent)> newCards = new List<(GameObject card, Transform parent)>();

        foreach (var jumpCard in jumpCards)
        {
            int columnIndex = jumpCard.Item1;
            int rowIndex = jumpCard.Item2;
            Vector3 pos = CardSlots [columnIndex].CardPosInRow [rowIndex].transform.position;
            Transform parent = CardSlots [columnIndex].CardPosInRow [rowIndex].transform;
            Transform tempParent = winningCardSlots [columnIndex].CardPosInRow [rowIndex].transform;
            GameObject newCard = poolManager.GetFromPool(PoolType.Cards,pos , Quaternion.identity , tempParent);

            Slot slot = CardSlots [columnIndex].CardPosInRow [rowIndex].GetComponent<Slot>();
            GameObject oldCard = slot.GetTheOwner();
            slot.RemoveOwner();
            poolManager.ReturnToPool(PoolType.Cards,oldCard);

            slot.AddOwner(newCard);
            Card newCardComponent = newCard.GetComponent<Card>();
            newCardComponent.SetCardType(CardType.BIG_JOKER);
            CommandCenter.Instance.cardManager_.ConfigureCardVisuals(newCardComponent , CardType.BIG_JOKER , false);
            newCard.transform.SetPositionAndRotation(initialPosition , initialRotation);
            newCards.Add((newCard, parent));
            CardDatas cardDatas = new CardDatas
            {
                name = CardType.BIG_JOKER.ToString() ,
                isGolden = false
            };
            CommandCenter.Instance.apiManager_.gameDataApi.UpdateGameDataApi(cardDatas , rowIndex , columnIndex);
        }

        Sequence seq = DOTween.Sequence();

        foreach (var (newCard, parent) in newCards)
        {
            Transform targetPos = newCard.transform.parent;
            Sequence CardSeq = DOTween.Sequence();
            CardSeq.Append(newCard.transform.DOJump(targetPos.position , 3.0f , 1 , 1.0f))
                .AppendCallback(() => newCard.transform.SetParent(parent))
                .AppendCallback(() => newCard.transform.localPosition = Vector3.zero);

            seq.Join(CardSeq);
        }

        yield return seq.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        yield return null;
    }
}
