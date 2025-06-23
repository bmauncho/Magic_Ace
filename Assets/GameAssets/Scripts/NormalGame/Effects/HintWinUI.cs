using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HintWinInfo
{
    public Sprite hintBG;
    public Sprite hintFrame;
}

public class HintWinUI : MonoBehaviour
{
    public Image BG;
    public Image BGFrame;

    public HintWinInfo [] hintWinInfos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
