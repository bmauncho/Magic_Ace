using UnityEngine;

public class WifiUI : MonoBehaviour
{
    public GameObject Goodwifi;
    public GameObject Okaywifi;
    public GameObject Badwifi;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        if (PromptManager.Instance == null)
            return;

        var checker = PromptManager.Instance.GetInternetConnectionChecker();

        // Default to "bad" if no checker
        if (checker == null)
        {
            SetWifiState(false , false , true);
            return;
        }

        switch (checker.connection)
        {
            case InternetConnection.Best:
                SetWifiState(true , false , false);
                break;
            case InternetConnection.Good:
            case InternetConnection.Okay:
                SetWifiState(false , true , false);
                break;
            case InternetConnection.Bad:
                SetWifiState(false , false , true);
                break;
        }
    }

    void SetWifiState ( bool good , bool okay , bool bad )
    {
        Goodwifi.SetActive(good);
        Okaywifi.SetActive(okay);
        Badwifi.SetActive(bad);
    }
}
