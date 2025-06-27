using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class LanguageTextTranslatorInfo
{
    public TheLanguage Lan;
    public TMP_SpriteAsset spriteAsset;
    public List<string> assetName ;
}
public class LanguageTextTranslator : MonoBehaviour
{
    public TMP_Text textComponent;
    public LanguageTextTranslatorInfo [] languageTextTranslatorInfos;
    private void OnEnable ()
    {
        if(LanguageMan.instance)
        {
           LanguageMan.instance.onLanguageRefresh.AddListener(UpdateImageText);
           UpdateImageText();
        }
    }
    public void UpdateImageText ()
    {
        if(textComponent == null)
        {
            Debug.LogError("Text component is not assigned.");
            return;
        }

        if (languageTextTranslatorInfos.Length <= 0 || languageTextTranslatorInfos == null)
        {
            Debug.LogError("Language text translator infos are not assigned or empty.");
            return;
        }

        TheLanguage activeLanguage = LanguageMan.instance.ActiveLanguage;
        for(int i = 0 ; i < languageTextTranslatorInfos.Length ; i++)
        {
            if (languageTextTranslatorInfos [i].Lan == activeLanguage)
            {
                TMP_SpriteAsset spriteAsset = languageTextTranslatorInfos [i].spriteAsset;
                if (spriteAsset != null)
                {
                    textComponent.spriteAsset = spriteAsset;
                    string result = string.Empty;
                    for(int j = 0 ; j < languageTextTranslatorInfos [i].assetName.Count ; j++)
                    {
                        result += $"<sprite name={languageTextTranslatorInfos [i].assetName [j]}>";
                    }
                    Debug.Log(result);
                    textComponent.text = result;
                }
                else
                {
                    Debug.LogWarning("Sprite asset is not assigned for the language: " + activeLanguage);
                }
                return;
            }
        }


        Debug.LogWarning("No matching language found for: " + activeLanguage);
        for (int j = 0 ; j < languageTextTranslatorInfos [0].assetName.Count ; j++)
        {
            textComponent.text += $"<sprite name={languageTextTranslatorInfos [0].assetName [j]}>";
        }

    }
}
