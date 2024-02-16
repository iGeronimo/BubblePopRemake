using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    public Vector3 moveDirection = new(0,1,0);
    public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        this.transform.position += moveDirection * speed;
    }

    public void SetMoveDirection(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        moveDirection = new Vector3(moveDirection.x * Mathf.Cos(radians) - moveDirection.y * Mathf.Sin(radians), moveDirection.x * Mathf.Sin(radians) + moveDirection.y * Mathf.Cos(radians));
        moveDirection.x = moveDirection.x * -1;
    }
}
