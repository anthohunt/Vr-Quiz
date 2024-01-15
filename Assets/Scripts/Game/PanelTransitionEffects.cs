using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransitionEffects : MonoBehaviour
{
    public GameObject coverPanel;
    public GameObject panel;
    
    // Update is called once per frame
    void Update()
    {
        if (panel != null)
        {
            if (this.transform.localRotation.y < -90)
            {
                coverPanel.SetActive(true);
                panel.SetActive(false);
            }
            else
            {
                coverPanel.SetActive(false);
                panel.SetActive(true);
            }
        }
    }
}
