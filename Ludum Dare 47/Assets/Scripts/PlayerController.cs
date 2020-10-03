using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private ICapacity _capacity;
    private Jump _jumpCapacity;
    private Hide _hideCapacity;

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
        CheckInput();
    }
    
    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _capacity.UseCapacity();
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
