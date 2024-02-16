using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFunctionality : MonoBehaviour
{
    public float ballRange;
    public float ballWidth;
    public bool moving = false;
    public BallColor ballColor = BallColor.RED;

    public Color red;
    public Color green;
    public Color blue;
    public Color yellow;
    public Color purple;
    public Color black;

    public bool ballChecked = false;

    private void Start()
    {
        SetColor();
    }

    void SetColor()
    {
        switch (ballColor)
        {
            case BallColor.RED:
                GetComponent<Renderer>().material.color = red;
                break;
            case BallColor.GREEN:
                GetComponent<Renderer>().material.color = green;
                break;
            case BallColor.BLUE:
                GetComponent<Renderer>().material.color = blue;
                break;
            case BallColor.YELLOW:
                GetComponent<Renderer>().material.color = yellow;
                break;
            case BallColor.PURPLE:
                GetComponent<Renderer>().material.color = purple;
                break;
            case BallColor.BLACK:
                GetComponent<Renderer>().material.color = black;
                break;
            case BallColor.RANDOMIZE:
                RandomColor();
                SetColor();
                break;

        }

    }

    void RandomColor()
    {
        int randomColorNumber = UnityEngine.Random.Range(0, 5);
        ballColor = (BallColor)randomColorNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfFalling()
    {
        BallFunctionality[] neighbours = CheckNeighbours();
        if(neighbours.Length == 0)
        {
            Debug.Log("Fall");
            EnableGravity();
        }
    }

    public void NeighboursToPop()
    {
        if (ballChecked) return;
        ballChecked = true;
        foreach (BallFunctionality neighbour in CheckNeighbours())
        {
            if(neighbour.ballColor == this.ballColor)
            {
                Debug.Log(ballChecked);
                PopBalls.Instance.newBallsToCheck.Add(neighbour);
            }
        }
    }

    public BallFunctionality[] CheckNeighbours()
    {
        List<BallFunctionality> neighbourBalls = new();
        RaycastHit2D[] neighbours = Physics2D.CircleCastAll(this.transform.position, ballRange, new(0, 0));
        foreach (RaycastHit2D ray in neighbours)
        {
            if (ray.collider.gameObject.GetComponent<BallFunctionality>() == null) continue;
            if (ray.collider.gameObject == this.gameObject) continue;
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
        if (collision.gameObject.GetComponent<BallFunctionality>() != null)
        {
            DisableGravity();
            transform.position = collision.gameObject.GetComponent<BallFunctionality>().AttachPosition((Vector2)transform.position);
            PopBalls.Instance.StartBallChecking(this);
        }
        if(collision.gameObject.tag == "wall")
        {
            BounceOnWall();
        }
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
        if (GetComponent<Rigidbody2D>() == null) return;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (gameObject.GetComponent<MoveComponent>() != null) gameObject.GetComponent<MoveComponent>().enabled = false;
        moving = false;
    }
    private void BounceOnWall()
    {
        Vector3 newMoveDirection = GetComponent<MoveComponent>().moveDirection;
        newMoveDirection.x *= -1;
        GetComponent<MoveComponent>().moveDirection = newMoveDirection;
    }

}

public enum BallColor
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    PURPLE,
    BLACK,
    RANDOMIZE
}
