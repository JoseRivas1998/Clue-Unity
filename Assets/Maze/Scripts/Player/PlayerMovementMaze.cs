using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMaze : MonoBehaviour
{
    public float walkSpeed;
    public float rotationSpeed;

    public Transform rotationTransform;

    Vector2 direction = Vector2.zero;

    int targetX = 1;
    int targetY = 1;

    int currentX = 1;
    int currentY = 1;

    float currentAngle;
    float lastAngle;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool targetReached = transform.position.x == targetX && transform.position.y == targetY;

        currentX = Mathf.FloorToInt(transform.position.x);
        currentY = Mathf.FloorToInt(transform.position.y);

        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        bool isWalking = (Mathf.Abs(direction.x) + Mathf.Abs(direction.y)) > 0;

        anim.SetBool("isWalking", isWalking);

        float angle = 0;

        if (isWalking)
        {
            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);

            if (direction.x > 0 && isWalking)
            {
                angle = 270;

                if (MazeGenerator.instance.GetMazeGridCell(currentX + 1, currentY) && targetReached)
                {
                    targetX = currentX + 1;
                    targetY = currentY;
                }
            }
            else if (direction.x < 0 && isWalking)
            {
                angle = 90;

                if (MazeGenerator.instance.GetMazeGridCell(currentX - 1, currentY) && targetReached)
                {
                    targetX = currentX - 1;
                    targetY = currentY;
                }
            }
            else if (direction.y > 0 && isWalking)
            {
                angle = 0;

                if (MazeGenerator.instance.GetMazeGridCell(currentX, currentY + 1) && targetReached)
                {
                    targetX = currentX;
                    targetY = currentY + 1;
                }
            }
            else if (direction.y < 0 && isWalking)
            {
                angle = 180;

                if (MazeGenerator.instance.GetMazeGridCell(currentX, currentY - 1) && targetReached)
                {
                    targetX = currentX;
                    targetY = currentY - 1;
                }
            }
            else
            {
                angle = lastAngle;
            }

            currentAngle = Mathf.LerpAngle(currentAngle, angle, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, targetY), walkSpeed * Time.deltaTime);
            rotationTransform.eulerAngles = new Vector3(0, 0, currentAngle);
            lastAngle = angle;
        }
    }
}
