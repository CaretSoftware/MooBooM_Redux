using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI textField;
    private LanguageSetter languageSetter;

    public string key;

    private bool isAdded;

    void Awake() {
        languageSetter = FindObjectOfType<LanguageSetter>();
        textField = GetComponent<TextMeshProUGUI>();
        string value = LocalisationSystem.GetLocalisedValue(key);
        textField.text = value;
    }

    public void UpdateLanguage() {
        textField = GetComponent<TextMeshProUGUI>();
        string value = LocalisationSystem.GetLocalisedValue(key);
        textField.text = value;
    }

    private void OnEnable() {
        if (languageSetter != null && !isAdded) {
            languageSetter.AddLanguageSetter(this);

            // Big change
        }
        isAdded = true;
    }
}