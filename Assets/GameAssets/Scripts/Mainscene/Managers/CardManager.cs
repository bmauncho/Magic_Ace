using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public enum CardType { ACE, KING, QUEEN, JACK, SPADE, HEART, DIAMOND, CLUB, SUPER_JOKER, BIG_JOKER, SMALL_JOKER, SCATTER, WILD };
[System.Serializable]
public class CardData
{
    public CardType CardType;
    public Sprite card;
}

[System.Serializable]
public class CardInfo
{
    public CardType CardType;
    public bool isGolden = false;
}
[System.Serializable]
public class CardWeight
{
    public CardType cardType;
    [UnityEngine.Range(1 , 20)] public float weight;

    public CardWeight ( CardType type , float w )
    {
        cardType = type;
        weight = w;
    }
}
public class CardManager : MonoBehaviour
{
    APIManager apiManager;
    [SerializeField] private Sprite normalBg;
    [SerializeField] private Sprite goldenBg;
    [SerializeField] private Sprite superJockerBg;
    [SerializeField] private Sprite bigJockerBg;
    [SerializeField] private Sprite smallJockerBg;
    [SerializeField] private Sprite bigJockerOutline;
    [SerializeField] private Sprite smallJockerOutline;
    [SerializeField] private CardData [] cardDatas;

    [Header("Card Probabilities")]
    [SerializeField, UnityEngine.Range(0 , 1)] private float goldenChance = 0.2f;  // Default 20%
    [SerializeField, UnityEngine.Range(0 , 1)] private float wildChance = 0.1f;    // Default 10%

    [SerializeField, UnityEngine.Range(0f , 1f)] private float smalljokerChance = 0.1f; // Default 10%
    [SerializeField, UnityEngine.Range(0f , 1f)] private float bigjokerChance = 0.05f; // Default 5%
    [SerializeField, UnityEngine.Range(0f , 1f)] private float SuperjokerChance = 0.025f; // Default 2.5%


    private float totalWeight = 0f; // Total weight of all card types
    [Header("Card Type Weights")]
    [SerializeField] private List<CardWeight> cardWeights = new List<CardWeight>();
    private void Awake ()
    {
        InitializeDefaultWeights();
    }
    private void Start ()
    {
        apiManager = CommandCenter.Instance.apiManager_;
    }

    private void InitializeDefaultWeights ()
    {
        if (cardWeights.Count == 0)
        {
            cardWeights.Add(new CardWeight(CardType.ACE , 7f));
            cardWeights.Add(new CardWeight(CardType.KING , 7f));
            cardWeights.Add(new CardWeight(CardType.QUEEN , 7f));
            cardWeights.Add(new CardWeight(CardType.JACK , 7f));
            cardWeights.Add(new CardWeight(CardType.SPADE , 7f));
            cardWeights.Add(new CardWeight(CardType.HEART , 7f));
            cardWeights.Add(new CardWeight(CardType.DIAMOND , 7f));
            cardWeights.Add(new CardWeight(CardType.CLUB , 7f));
        }

        totalWeight = cardWeights.Sum(weight => weight.weight); // Precompute total weights
    }

    public void setUpfirstTimeCards ( Card card , int col , int row )
    {
        if (!card) { return; }
        Card cardComponent = card.GetComponent<Card>();
        var cardInfoData = apiManager.gameDataApi.startCards [col].List [row];
        CardDatas cardInfo = new CardDatas
        {
            name = cardInfoData.name ,
            isGolden = cardInfoData.isGolden ,
            substitute = cardInfoData.substitute ,
        };


        //convert the string to enum 
        if (Enum.TryParse(typeof(CardType) , cardInfo.name , true , out var cardType))
        {
            cardComponent.SetCardType((CardType)cardType);
        }

        // Set the card type
        CardType type = cardComponent.GetCardType();
        bool isGolden = cardInfoData.isGolden;
        ConfigureCardVisuals(card , type , isGolden);
    }

    public void resetCard ( Card card )
    {
        if (!card) { return; }
        Card cardComponent = card.GetComponent<Card>();
        cardComponent.SetCardType();
        CardType type = cardComponent.GetCardType();
        bool isGolden = false;
        ConfigureCardVisuals(card , type , isGolden);
    }

    public void setUpCard ( Card card , int row , int col )
    {
        if (!card) { return; }
        Card cardComponent = card.GetComponent<Card>();
        var cardInfoData = apiManager.gameDataApi.GetCardInfo(col , row);
        CardDatas cardInfo = new CardDatas
        {
            name = cardInfoData.name ,
            isGolden = cardInfoData.isGolden ,
            substitute = cardInfoData.substitute ,
        };


        //convert the string to enum 
        if (Enum.TryParse(typeof(CardType) , cardInfo.name , true , out var cardType))
        {
            cardComponent.SetCardType((CardType)cardType);
        }

        // Set the card type
        CardType type = cardComponent.GetCardType();
        bool isGolden = cardInfoData.isGolden;
        ConfigureCardVisuals(card , type , isGolden);
    }

    public void setUpRefillCard ( Card card , int row , int col )
    {
        if (!card) { return; }
        Card cardComponent = card.GetComponent<Card>();
        //temp should use refill Api
        var cardInfoData = apiManager.refillApi.GetCardInfo(col , row);

        CardDatas cardInfo = new CardDatas
        {
            name = cardInfoData.name ,
            isGolden = cardInfoData.isGolden ,
            substitute = cardInfoData.substitute ,
        };
        //Debug.Log("card info: " + cardInfo.name + " isGolden: " + cardInfo.isGolden);
        //convert the string to enum 
        if (Enum.TryParse(typeof(CardType) , cardInfo.name , true , out var cardType))
        {
            cardComponent.SetCardType((CardType)cardType);
        }

        // Set the card type
        CardType type = cardComponent.GetCardType();
        // Debug.Log("card type: " + type);

        if (type == CardType.WILD || type == CardType.BIG_JOKER)
        {
            //Debug.Log("is wild Card");
            cardComponent.SetCardType(CardType.ACE);
        }
        type = cardComponent.GetCardType();
        bool isGolden = cardInfoData.isGolden;
        ConfigureCardVisuals(card , type , isGolden);

    }

    public void SetSpecificCard ( Card card , CardType cardType )
    {
        if (!card) { return; }
        Card cardComponent = card.GetComponent<Card>();
        CardDatas cardInfo = new CardDatas
        {
            name = cardType.ToString() ,
            isGolden = false ,
            substitute = null ,
        };
        ConfigureCardVisuals(card , cardType , cardInfo.isGolden);
    }


    public CardInfo GetRandomnCard ( int col )
    {
        CardInfo info = new CardInfo();
        float randomValue = Random.Range(0 , totalWeight);
        float cumulative = 0;

        foreach (var weight in cardWeights)
        {
            cumulative += weight.weight;
            if (randomValue <= cumulative)
            {
                info.CardType = weight.cardType;
                break;
            }
        }

        if (Random.value < wildChance)
        {
            info.CardType = CardType.SCATTER;
        }

        if (col >= 2 && col <= 4 && Random.value < goldenChance)
        {
            info.isGolden = true;
        }

        return info;
    }

    public Sprite GetCard ( CardType cardType )
    {
        for (int i = 0 ; i < cardDatas.Length ; i++)
        {
            if (cardDatas [i].CardType == cardType)
            {
                return cardDatas [i].card;
            }
        }
        return null;
    }

    public void ConfigureCardVisuals ( Card card , CardType type , bool isGolden )
    {
        if (type == CardType.SUPER_JOKER)
        {
            card.showSuperJocKer(superJockerBg , GetCard(type));
        }
        else if (type == CardType.BIG_JOKER)
        {
            card.showBigJocKer(bigJockerBg , GetCard(type) , bigJockerOutline);
        }
        else if (type == CardType.SMALL_JOKER)
        {
            card.showSmallJocKer(smallJockerBg , GetCard(type) , smallJockerOutline);
        }
        else if (type == CardType.SCATTER)
        {
            card.showWild(GetCard(type));
        }
        else
        {
            if (isGolden)
            {
                card.showGoldenCard(goldenBg , GetCard(type));
            }
            else
            {
                card.ShowNormalCard(normalBg , GetCard(type));
            }
        }
    }

    public CardType GetRandomJoker ()
    {
        float roll = Random.value;

        if (roll < SuperjokerChance)
            return CardType.SUPER_JOKER;
        else if (roll < SuperjokerChance + bigjokerChance)
            return CardType.BIG_JOKER;
        else if (roll < SuperjokerChance + bigjokerChance + smalljokerChance)
            return CardType.SMALL_JOKER;

        // If no joker is selected
        return CardType.SMALL_JOKER;
    }


    public void SetGoldenChance ( float GoldenChance = 0.05f )
    {
        goldenChance = GoldenChance;
    }

    public void SetWildChance ( float wildchance = 0.05f )
    {
        if (wildChance <= 0) { return; }
        wildChance = wildchance;
    }
}
