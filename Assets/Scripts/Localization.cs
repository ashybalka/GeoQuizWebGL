using TMPro;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private string localizationLanguage;
    public string localizationRu;
    public string localizationEn;

    private TMP_Text text;
    void Start()
    {
        localizationLanguage = SaveLoader.SaveResources.Language;
        text = GetComponent<TMP_Text>();
        text.text = localizationLanguage switch
        {
            "Ru-ru" => localizationRu,
            "En-en" => localizationEn,
            _ => localizationRu,
        };
    }
}
