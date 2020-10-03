using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : ICapacity
{
    public void UseCapacity(Rigidbody rb,bool onGround)
    {
        Debug.Log("jump");
    }
}
