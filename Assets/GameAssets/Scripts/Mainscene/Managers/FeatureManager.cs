using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum Features
{
    None,
    Feature_A,
    Feature_B,
    Feature_C,
}

public class FeatureManager : MonoBehaviour
{
    [SerializeField] private FeatureBuyMenu featureBuyMenu;
    [SerializeField] private Features features;
    [SerializeField] private int refillCounter = 0;
    [SerializeField] private int spinCounter = 0;

    [SerializeField] private GameObject FeatureBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CommandCenter.Instance)
        {
            if (features == Features.Feature_A
                   || features == Features.Feature_B
                   || features == Features.Feature_C)
            {
                FeatureBtn.GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                if (CommandCenter.Instance.gridManager_.IsFirstTime())
                {
                    FeatureBtn.GetComponentInChildren<Button>().interactable = false;
                }
                else
                {
                    FeatureBtn.GetComponentInChildren<Button>().interactable = true;
                }
            }
        }
    }

    public FeatureBuyMenu GetFeatureBuyMenu ()
    {
        return featureBuyMenu;
    }

    public Features GetActiveFeature ()
    {
        return features;
    }

    public void setFeature_A ()
    {
        features = Features.Feature_A;
    }

    public void setFeature_B ()
    {
        features = Features.Feature_B;
    }

    public void setFeature_C ()
    {
        features = Features.Feature_C;
    }

    public Feature_A_Sequence GetSequence ()
    {
        return GetComponent<Feature_A_Sequence>();
    }

    public Feature_B_Sequence GetSequence_B ()
    {
        return GetComponent<Feature_B_Sequence>();
    }

    public Feature_C_Sequence GetSequence_C ()
    {
        return GetComponent<Feature_C_Sequence>();
    }

    public void ShowFeature ()
    {
        CommandCenter.Instance.mainMenuController_.hideFeatureMenu();
        Invoke(nameof(spin) , .5f);
    }

    public List<GridInfo> spinInfo ( int counter )
    {
        if (counter < 0)
        {
            counter = 0;
        }

        var spininfo = new List<GridInfo>();
        switch (features)
        {
            case Features.Feature_A:
                break;
            case Features.Feature_B:
                break;
            case Features.Feature_C:
                break;
        }

        return spininfo;
    }

    public List<GridInfo> reffillInfo ( int counter = 0 )
    {

        if (counter < 0)
        {
            counter = 0;
        }

        var refillinfo = new List<GridInfo>();

        switch (features)
        {
            case Features.Feature_A:
                switch (counter)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                break;

            case Features.Feature_B:
                switch (counter)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                break;
            case Features.Feature_C:
                switch (counter)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
                break;
        }

        return refillinfo;
    }


    public CardDatas Setrefill ( int col , int row )
    {
        switch (refillCounter)
        {
            case 0:
                return null;
            case 1:
                return null;
            case 2:
                return null;
            case 3:
                return null;
            case 4:
                return null;
            case 5:
                return null;
        }
        return null;
    }

    public CardDatas Setrefill_B ( int col , int row )
    {
        switch (refillCounter)
        {
            case 0:
                return null;
            case 1:
                return null;
            case 2:
                    
            case 3:
                return null;
            case 4:
                return null;
            case 5:
                return null;
        }
        return null;
    }

    public CardDatas Setrefill_C ( int col , int row )
    {
        switch (refillCounter)
        {
            case 0:
                return null;
            case 1:
                return null;
            case 2:
                return null;
            case 3:
                return null;
            case 4:
                return null;
        }
        return null;
    }

    private void spin ()
    {
        CommandCenter.Instance.spinManager_.Spin();
    }

    public void IncreaseRefillCounter ()
    {
        if (features == Features.None) { return; }
        Debug.Log("refiilCounter which call :" + refillCounter);
        refillCounter++;
    }

    public void IncreaseSpinCounter ()
    {
        if (features == Features.None) { return; }
        spinCounter++;
    }

    public void ResetFeatures ()
    {
        features = Features.None;
    }

    public int GetRefillCounter ()
    {
        return refillCounter;
    }

    public int GetSpinCouter ()
    {
        return spinCounter;
    }
}
