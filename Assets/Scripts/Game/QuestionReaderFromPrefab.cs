using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestionReaderFromPrefab : MonoBehaviour
{
    // List to hold available QuestionsHolder prefabs in the scene
    private List<GameObject> questionsHolderPrefabs = new List<GameObject>();
    private List<GameObject> filteredPrefabs = new List<GameObject>();

    // Public variables to store the extracted values
    public List<QuestionsHolder.ThemeData> themesData;
    public List<string> questions = new List<string>();
    public List<string> trueAnswers = new List<string>();
    public List<List<string>> falseAnswers = new List<List<string>>();
    public List<string> explanations = new List<string>();

    public string themePrefabName;
    public int questionIndex;

    private void Start()
    {
        FindQuestionsHolderPrefabs();
    }

    public void CallRandomTheme()
    {
        LoadQuestionsFromRandomPrefab();
    }

    public void CallSelectedTheme(string theme)
    {
        LoadQuestionsFromSelectedTheme(theme);
    }

    public void CallThemeFromIndex()
    {
        LoadQuestionsFromPrefabName(themePrefabName);
    }

    // Find all available QuestionsHolder prefabs in the scene
    private void FindQuestionsHolderPrefabs()
    {
        QuestionsHolder[] holders = FindObjectsOfType<QuestionsHolder>();
        foreach (QuestionsHolder holder in holders)
        {
            questionsHolderPrefabs.Add(holder.gameObject);
        }
    }

    // Load questions from a randomly chosen QuestionsHolder prefab in the scene
    private void LoadQuestionsFromRandomPrefab()
    {
        if (questionsHolderPrefabs.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, questionsHolderPrefabs.Count);
            GameObject randomPrefab = questionsHolderPrefabs[randomIndex];

            themePrefabName = randomPrefab.name; // to send to remote client

            QuestionsHolder holder = randomPrefab.GetComponent<QuestionsHolder>();

            if (holder != null)
            {
                // Retrieve loaded themes data from QuestionsHolder
                themesData = holder.GetLoadedThemesData();

                // Extract and store the data in public variables
                ExtractData(themesData);

                // Destroy the selected QuestionsHolder prefab from the scene
                Destroy(randomPrefab);
            }
            else
            {
                Debug.LogError("QuestionsHolder script not found on the selected prefab!");
            }

            // Select a random question from the available pool.
            questionIndex = Random.Range(0, questions.Count);
        }
        else
        {
            Debug.LogError("No QuestionsHolder prefabs found in the scene!");
        }
    }
    public void LoadQuestionsFromSelectedTheme(string theme)
    {
        // Use the first word of the theme string as a filter for selecting a prefab
        string filter = theme.Split(' ')[0];

        // Filter the prefabs based on the first word of the theme
        filteredPrefabs = questionsHolderPrefabs.Where(prefab => prefab.name.StartsWith(filter)).ToList();

        if (filteredPrefabs.Count > 0)
        {
            // Select a random prefab from the filtered list
            int randomIndex = UnityEngine.Random.Range(0, filteredPrefabs.Count);
            GameObject selectedPrefab = filteredPrefabs[randomIndex];

            themePrefabName = selectedPrefab.name;

            QuestionsHolder holder = selectedPrefab.GetComponent<QuestionsHolder>();

            if (holder != null)
            {
                // Retrieve loaded themes data from QuestionsHolder
                themesData = holder.GetLoadedThemesData();

                // Extract and store the data in public variables
                ExtractData(themesData);

                // Optionally, destroy the selected QuestionsHolder prefab from the scene
               // Destroy(selectedPrefab);
            }
            else
            {
                Debug.LogError("QuestionsHolder script not found on the selected prefab!");
            }

            // Select a random question from the available pool.
            questionIndex = Random.Range(0, questions.Count);
        }
        else
        {
            Debug.LogError($"No QuestionsHolder prefabs found for the theme: {theme}");
        }
    }
    private void LoadQuestionsFromPrefabName(string name)
    {
        if (questionsHolderPrefabs.Count > 0)
        {
            GameObject namedPrefab = GetGameObjectByName(name);

            QuestionsHolder holder = namedPrefab.GetComponent<QuestionsHolder>();

            if (holder != null)
            {
                // Retrieve loaded themes data from QuestionsHolder
                themesData = holder.GetLoadedThemesData();

                // Extract and store the data in public variables
                ExtractData(themesData);

                // Destroy the selected QuestionsHolder prefab from the scene
                Destroy(namedPrefab);
            }
            else
            {
                Debug.LogError("QuestionsHolder script not found on the selected prefab!");
            }
        }
        else
        {
            Debug.LogError("No QuestionsHolder prefabs found in the scene!");
        }
    }


    public GameObject GetGameObjectByName(string nameToFind)
    {
        GameObject foundObject = questionsHolderPrefabs.FirstOrDefault(go => go != null && go.name == nameToFind);
        return foundObject;
    }

    // Extract and store the data in public variables
    private void ExtractData(List<QuestionsHolder.ThemeData> themes)
    {
        questions.Clear();
        trueAnswers.Clear();
        falseAnswers.Clear();
        explanations.Clear();

        foreach (var theme in themes)
        {
            foreach (var question in theme.Questions)
            {
                questions.Add(question.Question);
                trueAnswers.Add(question.TrueAnswer);
                falseAnswers.Add(question.FalseAnswer);
                explanations.Add(question.Explanation);
            }
        }
    }
}
