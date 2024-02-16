using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFunctionality : MonoBehaviour
{
    public float ballRange;
    public float ballWidth;
    public bool moving = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public BallFunctionality[] CheckNeighbours()
    {
        List<BallFunctionality> neighbourBalls = new();
        RaycastHit2D[] neighbours = Physics2D.CircleCastAll(this.transform.position, ballRange, new(0, 0));
        foreach (RaycastHit2D ray in neighbours)
        {
            if (ray.collider.gameObject.GetComponent<BallFunctionality>() == null) continue;
            neighbourBalls.Add(ray.collider.gameObject.GetComponent<BallFunctionality>());
        }
        return neighbourBalls.ToArray();
    }

    public Vector2 AttachPosition(Vector2 incomingBallPosition)
    {
        int northEast = 30;
        int east = 90;
        int southEast = 150;
        int southWest = 210;
        int west = 270;
        int northWest = 330;
        int closestAngle = int.MaxValue;
        int[] directions = { northEast, east, southEast, southWest, west, northWest};

        foreach(int direction in directions)
        {
            float difference = Mathf.Abs(direction - Vector2.Angle(incomingBallPosition, this.transform.position));
            if(closestAngle > difference)
            {
                closestAngle = (int)difference;
            }
        }


        var rotation = Quaternion.AngleAxis(closestAngle, Vector3.up);
        Debug.Log(rotation.ToString());
        Debug.Log((Vector2)this.transform.position + " + " + (Vector2)(rotation * new Vector3(0.0f, ballWidth)));
        return (Vector2)this.transform.position + (Vector2)(rotation * new Vector3(0.0f,ballWidth));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (moving == false) return;
        if (collision.gameObject.GetComponent<BallFunctionality>() == null) return;
        this.transform.position = AttachPosition((Vector2)collision.gameObject.transform.position);
        DisableGravity();
    }

    private void EnableGravity()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void DisableGravity()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
