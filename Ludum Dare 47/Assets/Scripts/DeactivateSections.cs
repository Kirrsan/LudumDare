using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSections : MonoBehaviour
{

    public GameObject[] sections;
    
    public static int sectionIndex = 0;

    private void Start()
    {
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
    }
    
    

}
