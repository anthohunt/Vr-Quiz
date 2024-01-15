using UnityEngine;
using System.Collections.Generic;

public class QuestionsHolder : MonoBehaviour
{
    [System.Serializable]
    public class ThemeData
    {
        public List<JsonToPrefab.QuestionData> Questions;
    }

    // List to store loaded themes data
    [SerializeField]
    private List<ThemeData> loadedThemesData = new List<ThemeData>();

    // Function to initialize the QuestionsHolder with data
    public void Initialize(List<JsonToPrefab.ThemeData> themesData)
    {
        foreach (var themeData in themesData)
        {
            ThemeData newData = new ThemeData();
            newData.Questions = themeData.Questions;
            loadedThemesData.Add(newData);
        }
    }

    // Function to retrieve loaded themes data
    public List<ThemeData> GetLoadedThemesData()
    {
        return loadedThemesData;
    }
}
