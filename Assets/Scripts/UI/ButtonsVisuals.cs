using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsVisuals : MonoBehaviour
{
    public NetworkAvatarSpawner networkAvatarSpawner;
    
    public GameObject p1ButtonA;
    public GameObject p1ButtonB;
    public GameObject p1ButtonC;
    public GameObject p1ButtonD;
    public GameObject p2ButtonA;
    public GameObject p2ButtonB;
    public GameObject p2ButtonC;
    public GameObject p2ButtonD;
    
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button buttonD;

    public void ActivateSelectedAnswer(int button, string player)
    {
        button++;
        
        if (networkAvatarSpawner.spawnedPosition == 1 && player == "localPlayer") 
        {
            if (button == 1)
            {
                p1ButtonA.SetActive(true);
            }
            else if (button == 2)
            {
                p1ButtonB.SetActive(true);
            }
            else if (button == 3)
            {
                p1ButtonC.SetActive(true);
            }
            else if (button == 4)
            {
                p1ButtonD.SetActive(true);
            }
        }
        else if (networkAvatarSpawner.spawnedPosition == 2 && player == "localPlayer")
        {
            if (button == 1)
            {
                p2ButtonA.SetActive(true);
            }
            else if (button == 2)
            {
                p2ButtonB.SetActive(true);
            }
            else if (button == 3)
            {
                p2ButtonC.SetActive(true);
            }
            else if (button == 4)
            {
                p2ButtonD.SetActive(true);
            }
        }

        if (networkAvatarSpawner.spawnedPosition == 1 && player == "remotePlayer")
        {
            if (button == 1)
            {
                p2ButtonA.SetActive(true);
            }
            else if (button == 2)
            {
                p2ButtonB.SetActive(true);
            }
            else if (button == 3)
            {
                p2ButtonC.SetActive(true);
            }
            else if (button == 4)
            {
                p2ButtonD.SetActive(true);
            }
        }

        if (networkAvatarSpawner.spawnedPosition == 2 && player == "remotePlayer")
        {
            if (button == 1)
            {
                p1ButtonA.SetActive(true);
            }
            else if (button == 2)
            {
                p1ButtonB.SetActive(true);
            }
            else if (button == 3)
            {
                p1ButtonC.SetActive(true);
            }
            else if (button == 4)
            {
                p1ButtonD.SetActive(true);
            }
        }
    }

    public void UpdateCorrectAnswerButtonColor(int correctButton)
    {
        Color initialColor = Color.grey;

        buttonA.image.color = initialColor;
        buttonB.image.color = initialColor;
        buttonC.image.color = initialColor;
        buttonD.image.color = initialColor;

        buttonA.GetComponentInChildren<TextMeshProUGUI>().alpha = 1;
        buttonB.GetComponentInChildren<TextMeshProUGUI>().alpha = 1;
        buttonC.GetComponentInChildren<TextMeshProUGUI>().alpha = 1;
        buttonD.GetComponentInChildren<TextMeshProUGUI>().alpha = 1;

        if (correctButton == 0)
        {
            buttonA.GetComponent<Image>().color = Color.green;
        }
        else if (correctButton == 1)
        {
            buttonB.GetComponent<Image>().color = Color.green;
        }
        else if (correctButton == 2)
        {
            buttonC.GetComponent<Image>().color = Color.green;
        }
        else if (correctButton == 3)
        {
            buttonD.GetComponent<Image>().color = Color.green;
        }
    }

    public void ReinitialiseButtonsVisuals()
    {
        buttonA.GetComponent<Image>().color = Color.grey;
        buttonB.GetComponent<Image>().color = Color.grey;
        buttonC.GetComponent<Image>().color = Color.grey;
        buttonD.GetComponent<Image>().color = Color.grey;

        p1ButtonA.SetActive(false);
        p1ButtonB.SetActive(false);
        p1ButtonC.SetActive(false);
        p1ButtonD.SetActive(false);

        p2ButtonA.SetActive(false);
        p2ButtonB.SetActive(false);
        p2ButtonC.SetActive(false);
        p2ButtonD.SetActive(false);
    }

    public void ButtonsWaitingForAnswer()
    {
        Color confirmedAnswerColor = Color.grey;
        confirmedAnswerColor.a = 0.25f;
        buttonA.image.color = confirmedAnswerColor;
        buttonB.image.color = confirmedAnswerColor;
        buttonC.image.color = confirmedAnswerColor;
        buttonD.image.color = confirmedAnswerColor;

        buttonA.GetComponentInChildren<TextMeshProUGUI>().alpha = confirmedAnswerColor.a;
        buttonB.GetComponentInChildren<TextMeshProUGUI>().alpha = confirmedAnswerColor.a;
        buttonC.GetComponentInChildren<TextMeshProUGUI>().alpha = confirmedAnswerColor.a;
        buttonD.GetComponentInChildren<TextMeshProUGUI>().alpha = confirmedAnswerColor.a;
    }
}
