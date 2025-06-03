using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum cannonType
{
    cannon1,
    cannon2,
    cannon3,
    cannon4,
}

[System.Serializable]
public class CannonData
{
    public cannonType type;
    public Multipliers currentMultiplier;
    public GameObject cannon;
    public TMP_Text cannonMultiplier;
    public Image cannonImage;
}
public class TopBanner : MonoBehaviour
{
    [Header("Cannon Data")]
    public CannonData [] cannons;
    
    public void UpdateCanon(cannonType cannonType,Multipliers multipliers,string theMultiplier,Sprite thecanon)
    {
        for(int i=0 ; i <cannons.Length ; i++)
        {
            if (cannons [i].type == cannonType)
            {
                cannons [i].currentMultiplier = multipliers;
                cannons [i].cannonMultiplier.text = theMultiplier;
                if (cannons [i].cannonImage != null)
                {
                    cannons [i].cannonImage.sprite = thecanon;
                }
            }
        }
    }
}
