using System.Collections.Generic;
using UnityEngine;

public class winningBg_Wild : MonoBehaviour
{
    [SerializeField]private int col;
    [SerializeField] private bool isActive = false;
    [SerializeField]private List<float> WinBgWidth = new List<float>()
    {
        74f,
        145f,
        215f,
        286f,
        360,
    };

    [ContextMenu("Activate")]
    public void Activate ()
    {
        isActive = true;
        GetComponent<CanvasGroup>().alpha = 1;
    }
    [ContextMenu("Deactivate")]
    public void Deactivate ()
    {
        isActive = false;
        GetComponent<CanvasGroup>().alpha = 0;
        resetBgSize();
    }
    [ContextMenu("Test")]
    public void test ()
    {
        col++;
        if(col >= WinBgWidth.Count)
        {
            col =WinBgWidth.Count;
        }
        Movebg(col);
    }

    public void Movebg(int col )
    {
        for( int i = 0; i < WinBgWidth.Count; i++ )
        {
            if(i == col)
            {
                RectTransform rect = GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(WinBgWidth [i] , rect.sizeDelta.y);
            }
        }
    }

    public void resetBgSize ()
    {
        col=-1;
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
    }

    public bool IsBgActive ()
    {
        return isActive;
    }
}
