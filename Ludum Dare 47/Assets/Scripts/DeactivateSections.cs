using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateSections : MonoBehaviour
{

    public GameObject[] sections;
    public GameObject[] doubleJumpSections;
    public GameObject[] wallJumpSections;

    public Text sectionTxt;
    
    public static int sectionIndex = 0;

    private void Start()
    {
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
        for (int i = 2; i < sections.Length; i++)
        {
            sections[i].SetActive(false);
        }
        for (int i = 1; i < doubleJumpSections.Length; i++)
        {
            doubleJumpSections[i].SetActive(false);
        }
        for (int i = 1; i < wallJumpSections.Length; i++)
        {
            wallJumpSections[i].SetActive(false);
        }
    }

    public IEnumerator DeactivateSection()
    {
        int index = sectionIndex;
        yield return  new WaitForSeconds(0.5f);
        sections[index].SetActive(false);
        if(index + 2 < sections.Length)
            sections[index + 2].SetActive(true);
        
        if(sectionIndex %2 == 0)
        {
            doubleJumpSections[index/2].SetActive(false);
            doubleJumpSections[index / 2 + 1].SetActive(true);
        }
        else
        {
            wallJumpSections[index / 2].SetActive(false);
            wallJumpSections[index / 2 + 1].SetActive(true);
        }

        sectionIndex++;
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
    }

    public void ResetSections()
    {
        for (int i = 0; i < 2; i++)
        {
            sections[i].SetActive(true);
        }
        for (int i = 2; i < sections.Length; i++)
        {
            sections[i].SetActive(false);
        }

        doubleJumpSections[0].SetActive(true);
        for (int i = 1; i < doubleJumpSections.Length; i++)
        {
            doubleJumpSections[i].SetActive(false);
        }

        wallJumpSections[0].SetActive(true);
        for (int i = 1; i < wallJumpSections.Length; i++)
        {
            wallJumpSections[i].SetActive(false);
        }

        sectionIndex = 0;
        sectionTxt.text = "Sector: " + (sectionIndex + 1).ToString();
    }
    
    

}
