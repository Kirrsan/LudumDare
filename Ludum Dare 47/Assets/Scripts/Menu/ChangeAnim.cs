using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnim : MonoBehaviour
{
    private Animation _animation;
    private Animator _animator;
    private Vector3 startPosition;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        if(GetComponent<Animator>())
            _animator = GetComponent<Animator>();
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
    public void ChangeAnimationAnimator()
    {
        _animator.SetTrigger("mustIdle");
    }

}
