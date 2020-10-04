using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{

    [SerializeField] private GameObject _text;
    private bool _hasShownText = false;


    public void MakeTextAppear()
    {
        if (!_hasShownText)
        {
            _hasShownText = true;
            _text.SetActive(false);
            _text.SetActive(true);
        }
    }
    public void MakeTextDisappear()
    {
        _hasShownText = false;
        _text.GetComponent<Animation>().Play("A_TextDisappear");
    }

}
