using UnityEngine;
using UnityEngine.UI;

public class ExtraBetEffectHandler : MonoBehaviour
{
    public GameObject extrabetEffect;
    public GameObject extrabetMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponentInChildren<Button>().interactable)
        {
            extrabetEffect.SetActive(true);
            extrabetMenu.SetActive(true);
        }
        else
        {
            extrabetEffect.SetActive(false);
            extrabetMenu.SetActive(false);
        }
    }
}
