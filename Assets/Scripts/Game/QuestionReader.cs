using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;

public class QuestionReader : MonoBehaviour
{
    [System.Serializable]
    public class RootData
    {
        public List<ThemeData> theme;
    }

    [System.Serializable]
    public class ThemeData
    {
        public List<QuestionData> Questions;
    }

    [System.Serializable]
    public class QuestionData
    {
        public string Question;
        public string TrueAnswer;
        public List<string> FalseAnswer;
        public string Explanation;
    }

    public string fileName;
    private string jsonFilePathRoot = "Assets/Resources/Questions/ABCD/";
    private string jsonFilePath;

    // Public variables to store the extracted values
    public List<ThemeData> themesData;
    public List<string> questions = new List<string>();
    public List<string> trueAnswers = new List<string>();
    public List<List<string>> falseAnswers = new List<List<string>>();
    public List<string> explanations = new List<string>();

    private RootData data; // Declare data at the class level

    public bool test;
    
    private void Update()
    {
        if (test)
        {
            test = false;
            CallRandomTheme();
        }
    }

    // calls a random theme and subtheme, and load questions
    public void CallRandomTheme()
    {
        string[] jsonFiles = Directory.GetFiles(jsonFilePathRoot, "*.json");

        if (jsonFiles.Length > 0)
        {
            // Select a random JSON file from the list
            fileName = Path.GetFileNameWithoutExtension(jsonFiles[UnityEngine.Random.Range(0, jsonFiles.Length)]);
            jsonFilePath = jsonFilePathRoot + fileName + ".json";

            if (File.Exists(jsonFilePath))
            {
                try
                {
                    string jsonText = File.ReadAllText(jsonFilePath);

                    // Deserialize the JSON data using Newtonsoft.Json
                    data = JsonConvert.DeserializeObject<RootData>(jsonText);

                    // Extract and store the data in public variables
                    themesData = data.theme;
                    questions.Clear();
                    trueAnswers.Clear();
                    falseAnswers.Clear();
                    explanations.Clear();

                    foreach (var theme in themesData)
                    {
                        foreach (var question in theme.Questions)
                        {
                            questions.Add(question.Question);
                            trueAnswers.Add(question.TrueAnswer);
                            falseAnswers.Add(question.FalseAnswer);
                            explanations.Add(question.Explanation);
                        }
                    }

                    // Log the content of the data variable
                    string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                    Debug.Log("Deserialized JSON Data:\n" + jsonData);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error deserializing JSON: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("JSON file not found at path: " + jsonFilePath);
            }
        }
        else
        {
            Debug.LogError("No JSON files found in directory: " + jsonFilePathRoot);
        }
    }
}