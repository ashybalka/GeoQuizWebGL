using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject CountryPrefab, PointTextPrefab;

    [SerializeField] GameObject GamePanel, StartPanel, WinPanel;

    [SerializeField] TMP_Text countryName;
    [SerializeField] TMP_Text scoreText, levelText, healtText, pointsTotalText;
    [SerializeField] GameObject CountriesObjectsParent;

    [SerializeField] TMP_Text WinPoints;

    public int etalonCountryId;
    public int level;
    public int score;
    private int health = 3;

    public int pointsTotal;

    private CountryStats countryStats;

    private void Start()
    {
        countryStats = GameObject.Find("Loader").GetComponent<CountryStats>();

        pointsTotalText.text = "0";

        GetLoad();
    }
    public void CreateNewQuestion(string gameMode)
    {
        CleanGameScreen();

        int[] countryArray;
        if (level <= 8)
        {
            countryArray = new int[level + 2];
        }
        else 
        {
            countryArray = new int[10];
        }

        //Etalon country
        int i = Random.Range(0, countryStats.Country._countriesList.Count);

        countryArray[0] = countryStats.Country._countriesList.Where(c => c.Id == i).First().Id;
        etalonCountryId = countryStats.Country._countriesList.Where(c => c.Id == i).First().Id;
 
        int k = 1;

        while ( k < countryArray.Length)
        {
            Debug.Log("Count " + countryStats.Country._countriesList.Count);
            int z = Random.Range(0, countryStats.Country._countriesList.Count);
            Debug.Log(z);
            int tempId = countryStats.Country._countriesList.Where(c => c.Id == z).First().Id;
            if (!countryArray.Contains(tempId))
            {
                countryArray[k] = tempId;
                k++;
            }
        }

        countryArray = RandomizeWithFisherYates(countryArray);

        for (int j = 0; j < countryArray.Length; j++)
        { 
            var generatedCountry = Instantiate(CountryPrefab);
            generatedCountry.transform.SetParent(CountriesObjectsParent.transform, false);
            generatedCountry.GetComponent<CountryController>().countryId = countryArray[j];
            generatedCountry.GetComponent<CountryController>().gameMode = gameMode;

            switch (gameMode)
            {
                case "Country":
                    string spriteName = "Images/Flags/" + countryStats.Country._countriesList.Where(c => c.Id == countryArray[j]).First().Name.EnName;
                    generatedCountry.GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteName);
                    break;
                case "Capital":
                    generatedCountry.GetComponent<Image>().sprite = null;
                    generatedCountry.GetComponentInChildren<TMP_Text>().text = TranslateCapitalName(countryArray[j]);
                    break;
            }
        }

        scoreText.text = score.ToString();
        levelText.text = level.ToString();

        countryName.text = TranslateCountryName(i);
        /*
        switch (gameMode)
        {
            case "Country":
                countryName.text = TranslateCountryName(i);
                break;
            case "Capital":
                countryName.text = TranslateCountryName(i);
                break;
        }*/

    }

    public void FakeChangeScene()
    {
        if (StartPanel.activeSelf)
        {
            StartPanel.SetActive(false);
        }
        if (!GamePanel.activeSelf)
        {
            GamePanel.SetActive(true);
        }
    }

    public string TranslateCountryName(int id)
    {
        return SaveLoader.SaveResources.Language switch
        {
            "En-En" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Name.EnName,
            "Ru-ru" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Name.RuName,
            "Tr-tr" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Name.TrName,
            _ => countryStats.Country._countriesList.Where(c => c.Id == id).First().Name.RuName,
        };
    }

    public string TranslateCapitalName(int id)
    {
        return SaveLoader.SaveResources.Language switch
        {
            "En-En" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Capital.EnName,
            "Ru-ru" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Capital.RuName,
            "Tr-tr" => countryStats.Country._countriesList.Where(c => c.Id == id).First().Capital.TrName,
            _ => countryStats.Country._countriesList.Where(c => c.Id == id).First().Capital.RuName,
        };
    }

    public static int[] RandomizeWithFisherYates(int[] array)
    {
        int count = array.Length;
        while (count > 1)
        {
            int i = Random.Range(0, count);
            count--;
            (array[i], array[count]) = (array[count], array[i]);
        }
        return array;
    }

    public void CleanGameScreen()
    {
        Image[] countries = CountriesObjectsParent.GetComponentsInChildren<Image>();
        foreach (Image co in countries)
        {
            Destroy(co.gameObject);
        }
    }

    public void Damage(int dmg, GameObject parent)
    {
        ShowPoints(dmg, parent);
        if (dmg == 0)
        {
            health--;
            healtText.text = health.ToString();
        }

        if (health <= 0)
        {
            StartCoroutine(WaitingforWinpanel());
        }
    }

    public void ShowPoints(int dmg, GameObject parent)
    {
        var PointText = Instantiate(PointTextPrefab, transform.parent);
        PointText.GetComponent<TMP_Text>().text = "+" + dmg;
        PointText.transform.SetParent(parent.transform.parent.transform.parent.transform, false);
    }

    public void RestartGame()
    {
        pointsTotal += score;
        MySave();
        SceneManager.LoadScene(0);    
    }

    public void AddHealth()
    { 
        health++;
        healtText.text = health.ToString();
        WinPanel.SetActive(false);
    }
    IEnumerator WaitingforWinpanel()
    {
        yield return new WaitForSeconds(1f);
        WinPanel.SetActive(true);
        WinPoints.text = score.ToString();
    }

    public void GetLoad()
    {
        SaveLoader.LoadFromJson();
        pointsTotal = SaveLoader.SaveResources.PointsTotal;
        pointsTotalText.text = pointsTotal.ToString();
    }

    public void MySave()
    {
        SaveLoader.SaveResources.PointsTotal = pointsTotal;
        SaveLoader.SaveToJson();
    }
}
