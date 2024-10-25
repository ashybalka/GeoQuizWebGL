using System;
using System.Collections.Generic;
using UnityEngine;

public class CountryStats : MonoBehaviour
{
    [SerializeField] TextAsset CountryStatsJson;
    public CountriesList Country;

    private void Awake()
    {
        LoadFromJson();
    }
    public void LoadFromJson()
    {
        Country = JsonUtility.FromJson<CountriesList>(CountryStatsJson.ToString());
    }
}
[Serializable]
public class CountriesList
{
    public List<Country> _countriesList;
}

[Serializable]
public class Country
{
    public int Id;
    public Name Name;
    public Capital Capital;
    public string Language;
}
[Serializable]
public class Name
{
    public string EnName;
    public string RuName;
    public string TrName;
}

[Serializable]
public class Capital
{
    public string EnName;
    public string RuName;
    public string TrName;
}
[Serializable]
public class Currency
{
    public string EnName;
    public string RuName;
    public string TrName;
}


