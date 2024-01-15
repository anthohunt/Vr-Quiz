using Oculus.Interaction;
using Oculus.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public DisplayManager displayManager;
    public GameManager gameManager;

    public List<GameObject> bigRedBuzzers;
    public List<GameObject> greyBuzzers;
    public List<TranslateTransform> translateTransforms;
    public List<GameObject> transitionPanel;

    public GameObject quizCanvas;
    public GameObject titlePanel;
    public GameObject themePanel;
    public GameObject questionPanel;
    public GameObject endPanel;

    public Transform crownPlayer;

    public Material buzzerInitialMaterial;
    public Material buzzerSelectedMaterial;
    public bool buzzersHaveBeenSelected;

    public TextMeshProUGUI themeA;
    public TextMeshProUGUI themeB;
    public TextMeshProUGUI themeC;
    public TextMeshProUGUI themeD;
    public string selectedTheme;
    public string remoteSelectedTheme;


    public void UpdateEnvironment(int gameMode)
    {
        // visual animation
        quizCanvas.transform.localRotation = Quaternion.Euler(0, -180, 0);
        quizCanvas.GetComponent<TranslateTransform>().isReadyToMove = true;

        int currentGameModeIndex = gameMode - 1;

        if (gameMode == 1)
        {
            titlePanel.SetActive(false);
            transitionPanel[currentGameModeIndex].SetActive(true);
        }
        else if (gameMode == 2)
        {
            BuzzerPlacer();
        }
        else if (gameMode == 3)
        {
            BuzzerActivation("allDeactivated");
            BuzzerPlacer();
        }
        else if (gameMode == -1)
        {
            endPanel.SetActive(true);
        }

        if (gameMode > 1)
        {
            transitionPanel[currentGameModeIndex - 1].SetActive(false);
            transitionPanel[currentGameModeIndex].SetActive(true);
        }
    }

    public void DisplayThemePanel(List<String> themes)
    {
        themeA.text = themes[0];
        themeB.text = themes[1];
        themeC.text = themes[2];
        themeD.text = themes[3];

        HideAllTransitionPanels();
        questionPanel.SetActive(false);
        themePanel.SetActive(true);
    }

    public void SelectedTheme(int button)
    {
        if (button == 1) 
        {
            selectedTheme = themeA.text;
        }
        if (button == 2)
        {
            selectedTheme = themeB.text;
        }
        if (button == 3)
        {
            selectedTheme = themeC.text;
        }
        if (button == 4)
        {
            selectedTheme = themeD.text;
        }

        if(gameManager.isHost)
        {
            displayManager.AskQuestion(true);
        }
        else
        {
            remoteSelectedTheme = selectedTheme;
        }
    }

    public void DisplayQuestionPanel(bool state = true)
    {
        HideAllTransitionPanels();
        themePanel.SetActive(false);
        questionPanel.SetActive(state);
    }

    public void HideAllTransitionPanels()
    {
        foreach (GameObject obj in transitionPanel)
        {
            obj.SetActive(false);
        }
    }

    public void BuzzerPlacer()
    {
        foreach (TranslateTransform obj in translateTransforms)
        {
            obj.isReadyToMove = true;
        }
    }

    public void BuzzerActivation(string state)
    {
        if (state == "allActivated")
        {
            foreach (GameObject obj in bigRedBuzzers)
            {
                obj.GetComponent<MeshRenderer>().material = buzzerInitialMaterial;
                buzzersHaveBeenSelected = false;
            }
        }
        else if (state == "allDeactivated")
        {
            foreach (GameObject obj in bigRedBuzzers)
            {
                obj.GetComponent<MeshRenderer>().material = buzzerSelectedMaterial;
                buzzersHaveBeenSelected = true;
            }
        }
    }

    public void PlaceCrown(string player)
    {
        crownPlayer.gameObject.SetActive(true);

        if (player == "local")
        {
            crownPlayer.SetParent(GameObject.Find("Local Avatar").transform.GetChild(2));
        }
        else if (player == "remote")
        {
            crownPlayer.SetParent(GameObject.Find("Remote Avatar").transform.GetChild(2));
        }
        
        crownPlayer.localPosition = new Vector3(0.25f, 0, 0);
        crownPlayer.localEulerAngles = new Vector3(0, 90, 0);
    }
}