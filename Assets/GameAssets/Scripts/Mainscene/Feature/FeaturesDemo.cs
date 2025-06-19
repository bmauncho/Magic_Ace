using System.Collections.Generic;
using UnityEngine;

public class FeaturesDemo : MonoBehaviour
{
    public  void featureA ( List<GridInfo> gridInfos )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        // Use the demo 
        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardDatas = new CardDatas();
                gridInfos [i].List.Add(cardDatas);
                // Get a random card from the card manager

                cardDatas = new CardDatas
                {
                    name = featureManager.SetSpin_A(i , j).name ,
                    isGolden = featureManager.SetSpin_A(i , j).isGolden ,
                    substitute = featureManager.SetSpin_A(i , j).substitute ,
                };
                // Assign the card data to the grid
                gridInfos [i].List [j] = cardDatas;
                //Debug.Log(cardDatas.name);
            }
        }
    }

    public void featureB ( List<GridInfo> gridInfos )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        // Use the demo 
        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardDatas = new CardDatas();
                gridInfos [i].List.Add(cardDatas);
                // Get a random card from the card manager

                cardDatas = new CardDatas
                {
                    name = featureManager.SetSpin_B(i , j).name ,
                    isGolden = featureManager.SetSpin_B(i , j).isGolden ,
                    substitute = featureManager.SetSpin_B(i , j).substitute ,
                };
                // Assign the card data to the grid
                gridInfos [i].List [j] = cardDatas;
                //Debug.Log(cardDatas.name);
            }
        }
    }

    public void featureC ( List<GridInfo> gridInfos )
    {
        FeatureManager featureManager = CommandCenter.Instance.featureManager_;
        // Use the demo 
        for (int i = 0 ; i < 5 ; i++)
        {
            GridInfo gridInfo = new GridInfo();
            gridInfos.Add(gridInfo);
            for (int j = 0 ; j < 4 ; j++)
            {
                CardDatas cardDatas = new CardDatas();
                gridInfos [i].List.Add(cardDatas);
                // Get a random card from the card manager

                cardDatas = new CardDatas
                {
                    name = featureManager.SetSpin_C(i , j).name ,
                    isGolden = featureManager.SetSpin_C(i , j).isGolden ,
                    substitute = featureManager.SetSpin_C(i , j).substitute ,
                };
                // Assign the card data to the grid
                gridInfos [i].List [j] = cardDatas;
                //Debug.Log(cardDatas.name);
            }
        }
    }
}
