using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject Ball;

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject newBall = Instantiate(Ball, this.transform.position, Quaternion.identity);
            newBall.AddComponent<MoveComponent>();
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0;
            Vector3 targetDir = worldMousePosition - gameObject.transform.position;
            float angleToMouse = (Vector3.SignedAngle(targetDir, transform.up, Vector3.forward) + 360) % 360;
            newBall.GetComponent<MoveComponent>().SetMoveDirection(angleToMouse);
            newBall.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            newBall.GetComponent<Rigidbody2D>().gravityScale = 0;
            newBall.GetComponent<BallFunctionality>().moving = true;
        }
    }
}
