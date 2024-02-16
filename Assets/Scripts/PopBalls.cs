using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopBalls : MonoBehaviour
{
    public static PopBalls Instance;
    private void OnEnable()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<BallFunctionality> ballsToPop;
    public List<BallFunctionality> ballsToCheck;
    public List<BallFunctionality> newBallsToCheck;
    private List<BallFunctionality> tempList;

    private void Start()
    {
        ballsToPop = new List<BallFunctionality>();
        ballsToCheck = new List<BallFunctionality>();
        newBallsToCheck = new List<BallFunctionality>();
        tempList = new List<BallFunctionality>();
    }

    public void StartBallChecking(BallFunctionality firstBall)
    {
        newBallsToCheck.Clear();
        firstBall.NeighboursToPop();
        ballsToPop.Add(firstBall);
        while (newBallsToCheck.Count > 0)
        {
            tempList = newBallsToCheck.Distinct().ToList();
            foreach (BallFunctionality ball in tempList)
            {
                ballsToCheck.Add(ball);
            }
            tempList.Clear();
            newBallsToCheck.Clear();
            Debug.Log(ballsToCheck.Count);
            foreach (BallFunctionality ball in ballsToCheck)
            {
                ball.NeighboursToPop();
                ballsToPop.Add(ball);
            }
            ballsToCheck.Clear();
        }
        tempList.Clear();
        newBallsToCheck.Clear(); 
        DestroyBallsToPop();
    }

    public void DestroyBallsToPop()
    {
        tempList = ballsToPop.Distinct().ToList();
        if (tempList.Count < 3)
        {
            foreach (BallFunctionality ball in tempList)
            {
                ball.ballChecked = false;
            }
            tempList.Clear();
            ballsToPop.Clear();
            return;
        }
        
        BallFunctionality[] ballsArray = tempList.ToArray();
        foreach (BallFunctionality ball in ballsArray)
        {
            Destroy(ball.gameObject);
        }

        tempList.Clear();
        ballsToPop.Clear();

        BallFunctionality[] allBalls = FindObjectsOfType<BallFunctionality>();
        foreach (BallFunctionality ball in allBalls)
        {
            ball.CheckIfFalling();
        }

    }
}
