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
        float closestAngle = int.MaxValue;
        int chosenAngle = 0;
        int[] directions = { northEast, east, southEast, southWest, west, northWest};

        foreach(int direction in directions)
        {
            float difference = Mathf.Abs(direction - ((Vector2.SignedAngle(incomingBallPosition - (Vector2)transform.position, transform.up)+360) %360));
            if(closestAngle > difference)
            {
                closestAngle = difference;
                chosenAngle = direction;
            }
        }

        Debug.Log(chosenAngle);
        float closestRadAngle = Mathf.Deg2Rad * chosenAngle;
        Vector2 tempPosition = this.transform.position;
        Vector2 rotatedBallWidth = new(0, ballWidth);
        rotatedBallWidth = new(rotatedBallWidth.x * Mathf.Cos(closestRadAngle) - rotatedBallWidth.y * Mathf.Sin(closestRadAngle), rotatedBallWidth.x * Mathf.Sin(closestRadAngle) + rotatedBallWidth.y * Mathf.Cos(closestRadAngle));
        rotatedBallWidth.x = rotatedBallWidth.x * -1;
        tempPosition += rotatedBallWidth;

        return tempPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (moving == false) return;
        if (collision.gameObject.GetComponent<BallFunctionality>() == null) return;
        DisableGravity();
        transform.position = collision.gameObject.GetComponent<BallFunctionality>().AttachPosition((Vector2)transform.position);
    }

    private void EnableGravity()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        moving = true;
    }

    private void DisableGravity()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (gameObject.GetComponent<MoveComponent>() != null) gameObject.GetComponent<MoveComponent>().enabled = false;
        moving = false;
    }
}
