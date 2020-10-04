using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour
{

    [SerializeField] private GameObject _text;
    private bool _hasShownText = false;
    private Quaternion clogStartRotation;

    private void Start()
    {
        clogStartRotation = transform.GetChild(0).rotation;
    }

    public void ResetRotation()
    {
        transform.GetChild(0).rotation = clogStartRotation;
    }


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
