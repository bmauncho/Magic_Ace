using System.Collections.Generic;
using UnityEngine;
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
public class Card : MonoBehaviour
{
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
