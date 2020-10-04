using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnim : MonoBehaviour
{
    private Animation _animation;
    private Vector3 startPosition;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        startPosition = transform.position;
    }

    public void ChangeAnimation()
    {
        _animation.Play("A_TextIdle");
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
        transform.position = startPosition;
    }

}
