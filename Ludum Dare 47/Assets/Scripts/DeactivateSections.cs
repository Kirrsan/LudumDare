using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateSections : MonoBehaviour
{

    public GameObject[] sections;

    public Text sectionTxt;
    
    public static int sectionIndex = 0;

    private void Start()
    {
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
        for (int i = 3; i < sections.Length; i++)
        {
            sections[i].SetActive(false);
        }
    }

    public IEnumerator DeactivateSection()
    {
        int index = sectionIndex;
        yield return  new WaitForSeconds(0.5f);
        sections[index].SetActive(false);
        if(index + 3 < sections.Length)
            sections[index + 3].SetActive(true);
        
        sectionIndex++;
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
    }

    public void ResetSections()
    {
        for (int i = 0; i < 3; i++)
        {
            sections[i].SetActive(true);
        }
        for (int i = 3; i < sections.Length; i++)
        {
            sections[i].SetActive(false);
        }

        sectionIndex = 0;
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
    }
    
    

}
