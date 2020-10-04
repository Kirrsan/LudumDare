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
            _text.SetActive(false);
            _text.SetActive(true);
            _hasShownText = true;
        }
    }
    public void MakeTextDisappear()
    {
        print("kfsl,:dw;");
        if (_hasShownText)
        {
            _hasShownText = false;
            if (_text.GetComponent<Animation>())
                _text.GetComponent<Animation>().Play("A_TextDisappear");
            else
            {
                _text.GetComponent<Animator>().SetTrigger("mustDisppear");
            }
        }
    }

}
