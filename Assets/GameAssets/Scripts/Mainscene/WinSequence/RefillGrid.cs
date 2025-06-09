using System.Collections;
using UnityEngine;

public class RefillGrid : MonoBehaviour
{
    APIManager apiManager;
    GridManager gridManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiManager = CommandCenter.Instance.apiManager_;
        gridManager = CommandCenter.Instance.gridManager_;
    }

    public IEnumerator RefillTheGrid ()
    {
        FeatureBuyMenu featureBuyMenu = CommandCenter.Instance.featureManager_.GetFeatureBuyMenu();
        if (CommandCenter.Instance.gameMode != GameMode.Demo &&
            featureBuyMenu.GetFeatureBuyOption() != FeaturBuyOption.None)
        {
            CommandCenter.Instance.gridManager_.SetIsRefilling(false);
            yield break;
        }

        if (CommandCenter.Instance.gameMode == GameMode.Demo)
        {
            //if free spin is active, setIsRefilling to false and break
        }
        Debug.Log("Refill Grid");
        apiManager.refillApi.FetchData();
        yield return new WaitWhile(() => !apiManager.refillApi.isRefillCardsFetched());
        gridManager.RefillGrid();
        yield return new WaitForSeconds(.1f);
        yield return null;
    }
}
