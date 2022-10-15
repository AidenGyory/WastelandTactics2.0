using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public bool canUseArrows;


    public float moveSpeed;

    bool hoverEnterMoveUp;
    bool hoverEnterMoveDown;
    bool hoverEnterMoveLeft;
    bool hoverEnterMoveRight;

    public bool moveFollowTarget;
    public Vector3 targetPosition;
    public float movementSpeed; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hoverEnterMoveUp)
        {
            MoveUp();
        }
        if (hoverEnterMoveDown)
        {
            MoveDown();
        }
        if (hoverEnterMoveLeft)
        {
            MoveLeft();
        }
        if (hoverEnterMoveRight)
        {
            MoveRight();
        }

        if (canUseArrows)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                MoveUp();
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                MoveDown();
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }
        }

        if (moveFollowTarget)
        {
            Vector3 _currentPosition = transform.position;
            Vector3 _targetPosition = targetPosition;

            _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, movementSpeed * Time.deltaTime);

            transform.position = _currentPosition;
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                moveFollowTarget = false;
            }
        }
    }

    public void SetDestination(Vector3 target)
    {
        targetPosition = target; 
        moveFollowTarget = true;
    }

    public void SetMoveDirection(int direction)
    {
        switch (direction)
        {
            case 0: hoverEnterMoveUp = true; break;
            case 1: hoverEnterMoveRight = true; break;
            case 2: hoverEnterMoveDown = true; break;
            case 3: hoverEnterMoveLeft = true; break;
        }
    }

    public void stopMoving()
    {
        hoverEnterMoveUp = false;
        hoverEnterMoveDown = false;
        hoverEnterMoveLeft = false;
        hoverEnterMoveRight = false;
    }

    void MoveUp()
    {
        transform.position = transform.position + Vector3.forward * moveSpeed * Time.deltaTime;
    }
    void MoveDown()
    {
        transform.position = transform.position + Vector3.back * moveSpeed * Time.deltaTime;
    }
    void MoveRight()
    {
        transform.position = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
    }
    void MoveLeft()
    {
        transform.position = transform.position + Vector3.left * moveSpeed * Time.deltaTime;
    }

}
