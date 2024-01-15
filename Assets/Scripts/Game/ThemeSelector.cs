using System.Collections.Generic;
using UnityEngine;

public class ThemeSelector : MonoBehaviour
{
    // Create a list to store the strings
    private List<string> stringList = new List<string>();

    void Start()
    {
        // Add strings to the list
        stringList.Add("History");
        stringList.Add("Geography");
        stringList.Add("Science");
        stringList.Add("Pop Culture");
        stringList.Add("Literature");
        stringList.Add("Music");
        stringList.Add("Movies");
        stringList.Add("Sports");
        stringList.Add("Art");
        stringList.Add("Food and Cooking");
        stringList.Add("Technology");
        stringList.Add("Nature and Wildlife");
        stringList.Add("Space and Astronomy");
        stringList.Add("Famous Quotes");
        stringList.Add("Mathematics");
        stringList.Add("TV Shows");
        stringList.Add("Video Games");
        stringList.Add("Cartoons");
        stringList.Add("Fashion and Design");
        stringList.Add("Internet");
    }

    public List<string> SelectRandomThemes(int count)
    {
        List<string> randomThemes = new List<string>();

        // Check if the count is valid
        if (count <= 0 || count > stringList.Count)
        {
            Debug.LogWarning("Invalid count for selecting random themes.");
            return randomThemes;
        }

        // Create a copy of the original list to avoid modifying it
        List<string> tempList = new List<string>(stringList);

        // Randomly select and add items to the randomThemes list
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            randomThemes.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        return randomThemes;
    }
}
