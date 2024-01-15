using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class JsonToPrefab : MonoBehaviour
{
    public string folderPath = "Assets/Resources/Questions/ABCD/"; // Path to the folder containing JSON files
    public string prefabSavePath = "Assets/Prefabs/QuestionPrefab"; // Path to save generated prefabs

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

    // Public variables to store the extracted values
    public List<ThemeData> themesData = new List<ThemeData>();
    public string prefabName;

    // EDITOR TOOLS, unlock in unity, lock for compiling

    //void Start()
    //{
    //    LoadJSONFiles();
    //}

    //void LoadJSONFiles()
    //{
    //    DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
    //    FileInfo[] files = directoryInfo.GetFiles("*.json");

    //    foreach (FileInfo file in files)
    //    {
    //        string json = File.ReadAllText(file.FullName);
    //        prefabName = file.Name;
    //        RootData rootData = JsonUtility.FromJson<RootData>(json);

    //        foreach (var themeData in rootData.theme)
    //        {
    //            themesData.Add(themeData); // Add theme data to the public variable
    //        }

    //        CreateQuestionsHolderPrefab(); // Create prefab with QuestionsHolder script holding the extracted data
    //        themesData.Clear(); // Clear the list for the next theme
    //    }
    //}

    //void CreateQuestionsHolderPrefab()
    //{
    //    GameObject prefab = new GameObject(prefabName);

    //    // Add QuestionsHolder script component and set its data
    //    QuestionsHolder questionsHolder = prefab.AddComponent<QuestionsHolder>();
    //    questionsHolder.Initialize(themesData);

    //    // Saving the prefab in the specified folder
    //    string prefabPath = prefabSavePath + "/" + prefabName + ".prefab";
    //    prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
    //    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
    //    DestroyImmediate(prefab); // Destroy the temporary GameObject
    //}
}
