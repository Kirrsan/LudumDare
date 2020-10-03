using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody _rb;
    
    private ICapacity _capacity;
    private Jump _jumpCapacity;
    private Hide _hideCapacity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _jumpCapacity = new Jump();
        _hideCapacity = new Hide();
        _capacity = _jumpCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput(_rb);
    }
    
    void CheckInput(Rigidbody rb)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _capacity.UseCapacity(rb);
        }

        //switch Test
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_capacity == _jumpCapacity)
                _capacity = _hideCapacity;
            else if (_capacity == _hideCapacity)
                _capacity = _jumpCapacity;
        }
    }

    public void SetCapacityDoubleJump()
    {
        //_capacity = doubleJumpScript;
    }
    
    public void SetCapacityWall()
    {
        //_capacity = WallRunScript;
    }
}
