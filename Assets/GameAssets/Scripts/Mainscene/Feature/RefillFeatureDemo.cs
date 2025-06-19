using System.Collections.Generic;
using UnityEngine;

public class RefillFeatureDemo : MonoBehaviour
{
    APIManager apiManager;
    GridManager gridManager;
    private void Start ()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        gridManager = CommandCenter.Instance.gridManager_;
    }
    public void FeatureA ( List<GridInfo> RefillCardsData ,bool isRefillCardsfetched )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        //card info list
        List<GridInfo> GameCardsData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        //slots list
        List<CardPos> GameCardSlots = new List<CardPos>(gridManager.GetGridCards());

        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo info = new GridInfo();
            RefillCardsData.Add(info);
        }

        List<GridInfo> GameCardsApiData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardData = new CardDatas();
                RefillCardsData [i].List.Add(cardData);
                Slot slot = GameCardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                cardData = new CardDatas
                {
                    name = featureManager.Setrefill_A(i , j).name ,
                    isGolden = featureManager.Setrefill_A(i , j).isGolden ,
                    substitute = featureManager.Setrefill_A(i , j).substitute ,
                };

                RefillCardsData [i].List [j] = new CardDatas
                {
                    name = cardData.name ,
                    isGolden = cardData.isGolden ,
                    substitute = cardData.substitute ,
                };
            }
        }
        isRefillCardsfetched = true;
    }

    public void FeatureB ( List<GridInfo> RefillCardsData , bool isRefillCardsfetched )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        //card info list
        List<GridInfo> GameCardsData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        //slots list
        List<CardPos> GameCardSlots = new List<CardPos>(gridManager.GetGridCards());

        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo info = new GridInfo();
            RefillCardsData.Add(info);
        }

        List<GridInfo> GameCardsApiData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardData = new CardDatas();
                RefillCardsData [i].List.Add(cardData);
                Slot slot = GameCardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                cardData = new CardDatas
                {
                    name = featureManager.Setrefill_B(i , j).name ,
                    isGolden = featureManager.Setrefill_B(i , j).isGolden ,
                    substitute = featureManager.Setrefill_B(i , j).substitute ,

                };

                RefillCardsData [i].List [j] = new CardDatas
                {
                    name = cardData.name ,
                    isGolden = cardData.isGolden ,
                    substitute = cardData.substitute ,

                };
            }
        }
        isRefillCardsfetched = true;
    }

    public void FeatureC ( List<GridInfo> RefillCardsData , bool isRefillCardsfetched )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        //card info list
        List<GridInfo> GameCardsData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        //slots list
        List<CardPos> GameCardSlots = new List<CardPos>(gridManager.GetGridCards());

        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo info = new GridInfo();
            RefillCardsData.Add(info);
        }

        List<GridInfo> GameCardsApiData = new List<GridInfo>(apiManager.gameDataApi.GetGridCardInfo());

        for (int i = 0 ; i < 5 ; i++)
        {
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardData = new CardDatas();
                RefillCardsData [i].List.Add(cardData);
                Slot slot = GameCardSlots [i].CardPosInRow [j].GetComponent<Slot>();
                cardData = new CardDatas
                {
                    name = featureManager.Setrefill_C(i , j).name ,
                    isGolden = featureManager.Setrefill_C(i , j).isGolden ,
                    substitute = featureManager.Setrefill_C(i , j).substitute ,
                };

                RefillCardsData [i].List [j] = new CardDatas
                {
                    name = cardData.name ,
                    isGolden = cardData.isGolden ,
                    substitute = cardData.substitute ,

                };
            }
        }
        isRefillCardsfetched = true;
    }

}
