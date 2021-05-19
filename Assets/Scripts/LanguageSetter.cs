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
      
    }

    public void ChangeToSwedish() {
        LocalisationSystem.language = LocalisationSystem.Language.Swedish;
        PlayerPrefs.SetString("Language", LocalisationSystem.GetLanguage());
        for (int i = 0; i < textLocaliserUIs.Count; i++) {
            textLocaliserUIs[i].UpdateLanguage();
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
