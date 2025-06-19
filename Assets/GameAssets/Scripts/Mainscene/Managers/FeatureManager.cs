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
    [SerializeField] private FeaturePicker featurePicker;
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
        spininfo = featurePicker.spinData(features , counter);
        return spininfo;
    }

    public List<GridInfo> reffillInfo ( int counter = 0 )
    {

        if (counter < 0)
        {
            counter = 0;
        }

        var refillinfo = new List<GridInfo>();
        refillinfo = featurePicker.refillData(features , counter);
        return refillinfo;
    }
    public CardDatas SetSpin_A ( int col , int row )
    {
        return featurePicker.featureA_spin(spinCounter, col , row);
    }
    public CardDatas SetSpin_B ( int col , int row )
    {
        return featurePicker.featureB_spin(spinCounter , col , row);
    }
    public CardDatas SetSpin_C ( int col , int row )
    {
        return featurePicker.featureC_spin(spinCounter , col , row);
    }

    public CardDatas Setrefill_A ( int col , int row )
    {
        return featurePicker.featureA_refill(refillCounter , col , row);
    }

    public CardDatas Setrefill_B ( int col , int row )
    {
        return featurePicker.featureB_refill(refillCounter,col,row);
    }

    public CardDatas Setrefill_C ( int col , int row )
    {
        return featurePicker.featureC_refill(refillCounter , col , row);
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
        if(features == Features.None) { return; }
        if (isFeature_Complete())
        {
            features = Features.None;
            spinCounter = 0;
            refillCounter = 0;
        }
    }

    public int GetRefillCounter ()
    {
        return refillCounter;
    }

    public int GetSpinCouter ()
    {
        return spinCounter;
    }

    public bool isFeature_A_Complete ()
    {
        if (features == Features.Feature_A)
        {
            if (spinCounter >= 1 && refillCounter >= 6)
            {
                return true;
            }
        }
        return false;
    }

    public bool isFeature_B_Complete ()
    {
        if (features == Features.Feature_B)
        {
            if (spinCounter >= 1 && refillCounter >= 2)
            {
                return true;
            }
        }
        return false;
    }

    public bool isFeature_C_Complete ()
    {
        if (features == Features.Feature_C)
        {
            if (spinCounter >= 1 && refillCounter >= 5)
            {
                return true;
            }
        }
        return false;
    }

    public bool isFeature_Complete ()
    {
        if (features == Features.Feature_A)
        {
            return isFeature_A_Complete();
        }
        else if (features == Features.Feature_B)
        {
            return isFeature_B_Complete();
        }
        else if (features == Features.Feature_C)
        {
            return isFeature_C_Complete();
        }
        return false;
    }
}
