using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommentaryManager : MonoBehaviour
{
    SoundManager soundManager;
    public List<CardType> cards = new List<CardType>();
    bool comboPlayed = false;
    int commentaryIndex = 0;
    string[] extraCommentary =
    {
        "VO_Amazing_en",
        "VO_Awesome_en",
        "VO_Incredible_en",
        "VO_GetRich_en",
        "VO_TakeMoney_en",
    };
    private void Start ()
    {
        soundManager = CommandCenter.Instance.soundManager_;
    }

    public void ResetCommentaryIndex ()
    {
        commentaryIndex = 0;
    }

    public void PlayCommentary ( List<CardType> winningCards )
    {
        cards.Clear();
        comboPlayed = false ;
        //Debug.Log("commentary called");
        HashSet<CardType> cardTypes = new HashSet<CardType>(winningCards);
        StartCoroutine(PlayCommentarySequentially(cardTypes.ToList()));
        cards = new List<CardType>(cardTypes.ToList());
    }

    private IEnumerator PlayCommentarySequentially ( List<CardType> cardTypes )
    {
        foreach (CardType cardType in cardTypes)
        {
            switch (cardType)
            {
                case CardType.ACE:
                    // Play commentary for Ace
                    //Debug.Log("You won with Aces!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.KING:
                    // Play commentary for King
                    //Debug.Log("You won with Spades!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.QUEEN:
                    // Play commentary for Queen
                    //Debug.Log("You won with Queens!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.JACK:
                    // Play commentary for jack
                    //Debug.Log("You won with Jacks!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.SPADE:
                    // Play commentary for Spades
                    //Debug.Log("You won with Spades!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.HEART:
                    // Play commentary for Hearts
                    //Debug.Log("You won with Hearts!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.DIAMOND:
                    // Play commentary for Diamonds
                    //Debug.Log("You won with Diamonds!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;

                case CardType.CLUB:
                    // Play commentary for Clubs
                    //Debug.Log("You won with Clubs!");
                    soundManager.PlaySound(cardType.ToString() , false);
                    yield return new WaitForSeconds(0.25f);
                    yield return StartCoroutine(PlayComboCommentary());
                    break;
                default:
                    // Handle any other card types or unexpected cases
                    //Debug.Log("You won with an Scatter card type!");
                    yield return new WaitForSeconds(1.0f);
                    break;
            }
        }
    }


    private IEnumerator PlayComboCommentary ()
    {
       // Debug.Log("PlayComboCommentary");
        int Combo = CommandCenter.Instance.multiplierManager_.GetMultiplier();
        // Flag to track if the combo commentary has been played.

        if (!CommandCenter.Instance.freeSpinManager_.IsFreeGame())
        {
            if (!comboPlayed && Combo >= 2) // Check if combo is 2 or greater and not yet played.
            {
                switch (Combo)
                {
                    case 1:
                        break;
                    case 2:
                        soundManager.PlaySound("Lucky" , false);
                        yield return new WaitForSeconds(0.25f);
                        soundManager.PlaySound("Double" , false);
                        break;

                    case 3:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("Triple" , false);
                        break;

                    case 4:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("FourTimes" , false);
                        break;

                    case 5:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("FiveTimes" , false);
                        break;
                    case 6:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("SixTimes" , false);
                        break;
                    case 7:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("SevenTimes" , false);
                        break;
                    case 8:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("EightTimes" , false);
                        break;
                    case 9:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("NineTimes" , false);
                        break;
                    case 10:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("TenTimes" , false);
                        break;
                    default:
                        string VO = extraCommentary [commentaryIndex % extraCommentary.Length];
                        CommandCenter.Instance.soundManager_.PlaySound(VO);
                        commentaryIndex++;
                        break;

                }
                comboPlayed = true; // Set flag to true after playing commentary.
            }
        }
        else
        {
            if (!comboPlayed && Combo >= 2) // Check for regular game combo.
            {
                //Debug.Log("combo to play " + Combo);
                switch (Combo)
                {
                    case 2:
                        soundManager.PlaySound("Lucky" , false);
                        yield return new WaitForSeconds(0.25f);
                        soundManager.PlaySound("Double" , false);
                        break;

                    case 3:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("Triple" , false);
                        break;

                    case 4:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("FourTimes" , false);
                        break;

                    case 5:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("FiveTimes" , false);
                        break;
                    case 6:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("SixTimes" , false);
                        break;
                    case 7:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("SevenTimes" , false);
                        break;
                    case 8:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("EightTimes" , false);
                        break;
                    case 9:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("NineTimes" , false);
                        break;
                    case 10:
                        yield return new WaitForSeconds(0.1f);
                        soundManager.PlaySound("TenTimes" , false);
                        break;
                    default:
                        string VO = extraCommentary [commentaryIndex % extraCommentary.Length];
                        CommandCenter.Instance.soundManager_.PlaySound(VO);
                        commentaryIndex++;
                        break;
                }
                comboPlayed = true; // Set flag to true after playing commentary.
            }
        }
    }


    public List<CardType> GetCardTypes ( List<winCardData> winningCards )
    {
        HashSet<CardType> cardTypes = new HashSet<CardType>();
        string namePrefix = "GOLDEN_";

        foreach (var card in winningCards)
        {
            string cardType = card.name;

            string newName = cardType.StartsWith(namePrefix , System.StringComparison.OrdinalIgnoreCase)
                ? cardType.Substring(namePrefix.Length)
                : cardType;

            if (Enum.TryParse<CardType>(newName , ignoreCase: true , out var result))
            {
                cardTypes.Add(result);
            }
            else
            {
                Debug.LogError($"Invalid enum value: {newName}");
                return default;
            }
        }

        return cardTypes.ToList();
    }


}
