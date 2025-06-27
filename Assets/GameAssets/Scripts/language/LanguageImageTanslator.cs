using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LanguageImage
{
    public TheLanguage LanguageName; // Name of the language
    public Sprite lanImage; // Image associated with the language
}
public class LanguageImageTanslator : MonoBehaviour
{
    public Image TheImage; // Reference to the Image component
    public LanguageImage [] LanguageImages; // Array of LanguageImage objects

    private void Start ()
    {
        if (LanguageMan.instance)
        {
            LanguageMan.instance.onLanguageRefresh.AddListener(changeLanguage); // Subscribe to the language change event
        }
    }
    public void changeLanguage ()
    {
        SetLanguageImage(LanguageMan.instance.ActiveLanguage); // Set the image based on the active language from LanguageMan   
    }

    public void SetLanguageImage ( TheLanguage language )
    {
        // Find the corresponding image for the given language
        foreach (var langImage in LanguageImages)
        {
            if (langImage.LanguageName == language)
            {
                TheImage.sprite = langImage.lanImage; // Set the Image component's sprite
                return; // Exit after setting the image
            }
        }

        TheImage.sprite = LanguageImages [0].lanImage; // Fallback to the first image if no match is found
        Debug.LogWarning("Language image not found for: " + language);
    }
}
