using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSetter : MonoBehaviour {

    private static List<TextLocaliserUI> textLocaliserUIs;

	private void Awake() {
        if (PlayerPrefs.GetString("Language") != null) {
            string languageID = PlayerPrefs.GetString("Language");
            LocalisationSystem.SetLanguage(languageID);
        } else {
            LocalisationSystem.SetLanguageBySystem();
            string languageID = LocalisationSystem.GetLanguage();
            PlayerPrefs.SetString("Language", languageID);
        }

        TextLocaliserUI[] textLocaliserUIsArray = FindObjectsOfType<TextLocaliserUI>();
        textLocaliserUIs = new List<TextLocaliserUI>(textLocaliserUIsArray);
        /*if (!PlayerPrefs.GetString("Language", "defaultValue").Equals("defaultValue"))
        {
            LocalisationSystem.SetLanguage(PlayerPrefs.GetString("Language"));
        }
        else
        {
            LocalisationSystem.SetLanguageBySystem();
            PlayerPrefs.SetString("Language", LocalisationSystem.GetLanguage());
            Debug.Log("Changed language by system");
        }*/

    }

    public void ChangeToSwedish() {
        LocalisationSystem.language = LocalisationSystem.Language.Swedish;
        PlayerPrefs.SetString("Language", LocalisationSystem.GetLanguage());
        for (int i = 0; i < textLocaliserUIs.Count; i++) {
            textLocaliserUIs[i].UpdateLanguage();
            /*if (LocalisationSystem.language.Equals(LocalisationSystem.Language.Swedish))
            {
                LocalisationSystem.language = LocalisationSystem.Language.English;
            }
            else
            {
                LocalisationSystem.language = LocalisationSystem.Language.Swedish;
            }
            PlayerPrefs.SetString("Language", LocalisationSystem.GetLanguage());
            for (int i = 0; i < textLocaliserUI.Length; i++) {
                textLocaliserUI[i].UpdateLanguage(); 
            }
            Debug.Log("Toggle toggle! " + LocalisationSystem.GetLanguage());*/
        }
    }

    public void ChangeToEnglish() {
        LocalisationSystem.language = LocalisationSystem.Language.English;
        PlayerPrefs.SetString("Language", LocalisationSystem.GetLanguage());
        for (int i = 0; i < textLocaliserUIs.Count; i++) {
            textLocaliserUIs[i].UpdateLanguage();
        }
    }

    public void AddLanguageSetter(TextLocaliserUI ls) {
        if (!textLocaliserUIs.Contains(ls)) {
            textLocaliserUIs.Add(ls);
            ls.UpdateLanguage();
        }
    }
}
