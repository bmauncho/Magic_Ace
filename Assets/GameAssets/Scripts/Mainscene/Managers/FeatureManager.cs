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

    public void ShowFeature ()
    {
        CommandCenter.Instance.mainMenuController_.hideFeatureMenu();
        Invoke(nameof(spin) , .5f);
    }

    private void spin ()
    {
        CommandCenter.Instance.spinManager_.Spin();
    }

    public void ResetFeatures ()
    {
        features = Features.None;
    }
}
