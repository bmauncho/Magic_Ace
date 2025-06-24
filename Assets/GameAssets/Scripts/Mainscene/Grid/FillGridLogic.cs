using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGridLogic : MonoBehaviour
{

    public IEnumerator normalfillGrid ( List<CardPos> CardSlots,bool isFirstTime,WildEffect wildEffect,winningBg_Wild winningBg_Wild )
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int col = 0;
        int row = 0;
        int activeAnimations = 0;
        float delayIncrement = 0.1f;

        // Debug.Log("normal spin");

        if (!isFirstTime)
        {
            CommandCenter.Instance.soundManager_.PlaySound("Base_Icon_Land");
        }

        List<int> wildColumns = new List<int>();
        List<(string cardName, List<(int col, int row)>)> wildCards = new List<(string cardName, List<(int col, int row)>)>();
        Card cardComponent = null;
        List<(Card wildcard, List<(int col, int row)>)> wildWinCards = new List<(Card wildcard, List<(int col, int row)>)>();
        int scatterCount = 0;
        for (col = 0 ; col < colCount ; col++)
        {
            bool preTriggerWild = false;
            var currDeck = Decks [col];
            int currentAnims = 0;
            bool ScatterFound = false;
            for (row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;

                // Check for SUPER_JOKER in slot

                Card existingCard = slot.GetComponentInChildren<Card>();

                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);
                cardComponent = newCard.GetComponent<Card>();

                if (isFirstTime)
                {
                    CommandCenter.Instance.cardManager_.setUpfirstTimeCards(cardComponent , col , row);
                }
                else
                {
                    CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);

                    if (cardComponent.GetCardType() == CardType.SCATTER)
                    {
                        ScatterFound = true;
                        scatterCount++;
                        wildColumns.Add(col);
                        wildCards.Add((cardComponent.GetCardType().ToString(), new List<(int col, int row)> { (col, row) }));
                    }

                    if (wildCards.Count >= 2)
                    {
                        preTriggerWild = true;
                    }

                    if (preTriggerWild && cardComponent.GetCardType() == CardType.SCATTER)
                    {
                        wildEffect.ToggleEffect(col);
                        List<(int col, int row)> cardPos = new List<(int col, int row)>();
                        cardPos.Add((col, row));
                        wildWinCards.Add((cardComponent, cardPos));
                    }
                }

                slot.AddOwner(newCard); // Ensure owner is set properly

                activeAnimations++;
                currentAnims++;
                cardComponent.OnComplete += () => activeAnimations--;
                cardComponent.OnComplete += () => currentAnims--;

                float delay = preTriggerWild ? row * delayIncrement : 0f;
                yield return StartCoroutine(DelayedMove(cardComponent , target , delay , slot , false));
                //if (ScatterFound)
                //{
                //    if (scatterCount > 0 && scatterCount < 2)
                //    {
                //        CommandCenter.Instance.soundManager_.PlaySound("Base_C1_Hit_01");
                //    }
                //    else if (scatterCount > 1 && scatterCount < 3)
                //    {
                //        CommandCenter.Instance.soundManager_.PlaySound("Base_C1_Hit_02");
                //    }
                //    else if (scatterCount >= 3)
                //    {
                //        CommandCenter.Instance.soundManager_.PlaySound("Base_C1_Hit_03");
                //    }
                //}
            }

            if (ScatterFound)
            {
                playscaterSound(col);
            }


            if (preTriggerWild)
            {
                wildEffect.ToggleEffect(col + 1);
                winningBg_Wild.Activate();
                winningBg_Wild.Movebg(col);
                //move wild card to winslot
                moveCardsToWinSlot(CardSlots,wildWinCards , col);
                CommandCenter.Instance.soundManager_.PlaySound("Common_ReelSpeedUp");
            }
            ScatterFound = false;
        }


        while (activeAnimations > 0)
            yield return null;
        wildEffect.DisAbleEffect();
        Debug.Log("all animations Done!");
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(()=>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    public IEnumerator quickfillGrid ( List<CardPos> CardSlots )
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int colCount = 5;
        int rowCount = 4;
        int activeAnimations = 0;
        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;
                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(() =>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    public IEnumerator turbofillGrid ( List<CardPos> CardSlots)
    {
        Deck [] Decks = CommandCenter.Instance.deckManager_.GetDecks();
        int rowCount = 4;
        int colCount = 5;
        int activeAnimations = 0;
        for (int col = 0 ; col < colCount ; col++)
        {
            var currDeck = Decks [col];
            for (int row = rowCount - 1 ; row >= 0 ; row--)
            {
                Slot slot = CardSlots [col].CardPosInRow [row].transform.GetComponent<Slot>();
                Transform target = CardSlots [col].CardPosInRow [row].transform;

                GameObject newCard = currDeck.DrawCard();
                newCard.transform.SetParent(target);

                Card cardComponent = newCard.GetComponent<Card>();
                CommandCenter.Instance.cardManager_.setUpCard(cardComponent , row , col);
                // check if is a wining card 
                activeAnimations++;
                cardComponent.OnComplete += () => activeAnimations--;

                cardComponent.moveCard(target , slot);
            }
        }

        // Wait until all animations are completed
        while (activeAnimations > 0)
            yield return null;
        CommandCenter.Instance.winLoseManager_.WinLoseSequence(() =>
        {
            Debug.Log("WinLoseSequence completed");
        });
    }

    // Separate coroutine to handle individual delays
    private IEnumerator DelayedMove ( Card card , Transform target , float delay , Slot _slot , bool isInitialization )
    {
        yield return new WaitForSeconds(delay);
        if (!card.gameObject.activeSelf)
        {
            Debug.LogWarning("Card is not active, skipping move.");
            card.gameObject.SetActive(true);
        }
        card.moveCard(target , _slot , isInitialization);
        yield return null;
    }

    private void playscaterSound ( int col )
    {
        SoundManager soundManager = CommandCenter.Instance.soundManager_;
        switch (col)
        {
            case 0:
                soundManager.PlaySound("Common_C1_Hit_01");
                break;
            case 1:
                soundManager.PlaySound("Common_C1_Hit_02");
                break;
            case 2:
                soundManager.PlaySound("Common_C1_Hit_03");
                break;
            case 3:
                soundManager.PlaySound("Common_C1_Hit_04");
                break;
            case 4:
                soundManager.PlaySound("Common_C1_Hit_05");
                break;
        }
    }

    private void moveCardsToWinSlot ( List<CardPos> CardSlots , List<(Card wildcard, List<(int col, int row)> wildCards)> cardComponents , int column )
    {
        List<CardPos> winCardSlots = new List<CardPos>(CommandCenter.Instance.winLoseManager_.GetWinCardSlots());

        foreach (var (wildcard, wildCards) in cardComponents)
        {
            foreach (var (col, row) in wildCards)
            {
                if (col >= 0 && col < CardSlots.Count && row >= 0 && row < CardSlots [col].CardPosInRow.Count)
                {
                    Slot slot = CardSlots [col].CardPosInRow [row].GetComponent<Slot>();
                    GameObject card = slot.GetTheOwner();
                    if (card != null)
                    {
                        WinSlot winSlot = winCardSlots [col].CardPosInRow [row].GetComponent<WinSlot>();
                        winSlot.AddOwner(card);
                        card.transform.SetParent(winSlot.transform);
                    }
                }
            }
        }

        cardComponents.Clear();
        int colDiff = 5 - column;
        int currentCol = column;
        // so if current col is 1 activate any card type called wild in col 0
        // if  current col is 2 activate a any card type called wild  in 0,1 
        //etc
        if (colDiff > 0)
        {
            for (int colIndex = 0 ; colIndex < column ; colIndex++)
            {
                int rowCount = CardSlots [colIndex].CardPosInRow.Count;

                for (int row = 0 ; row < rowCount ; row++)
                {
                    Slot slot = CardSlots [colIndex].CardPosInRow [row].GetComponent<Slot>();
                    WinSlot winSlot = winCardSlots [colIndex].CardPosInRow [row].GetComponent<WinSlot>();
                    GameObject card = slot.GetTheOwner();

                    if (card != null)
                    {
                        Card cardComp = card.GetComponent<Card>();
                        if (cardComp != null && cardComp.GetCardType() == CardType.SCATTER)
                        {
                            winSlot.AddOwner(card);
                            card.transform.SetParent(winSlot.transform);
                        }
                    }
                }
            }
        }

    }
}
