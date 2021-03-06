﻿using UnityEngine;

public class MovePiece : MonoBehaviour
{
    public MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private RigidbodyConstraints originalConstraints;

    private bool pieceGrabbed;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool xDown, yDown, zDown;

    private float maxVelocity = 10f;

    public JengaManager jengaManager;
            
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        originalConstraints = _rigidbody.constraints;

        pieceGrabbed = false;
        xDown = false;
        yDown = false;
        zDown = false;

        jengaManager.canMove = true;
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > maxVelocity)
        {
            var v = _rigidbody.velocity;
            _rigidbody.velocity = v.normalized * maxVelocity;
        }

        if (jengaManager.canMove && pieceGrabbed)
        {
            if (Input.GetKeyDown("x"))
            {
                xDown = true;
            }
            else if (Input.GetKeyUp("x"))
            {
                xDown = false;

                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }

            if (Input.GetKeyDown("y"))
            {
                yDown = true;
            }
            else if (Input.GetKeyUp("y"))
            {
                yDown = false;

                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }

            if (Input.GetKeyDown("z"))
            {
                zDown = true;
            }
            else if (Input.GetKeyUp("z"))
            {
                zDown = false;

                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }


            DragPiece();
        }
    }

    private void OnMouseEnter()
    {
        if (jengaManager.canMove && !pieceGrabbed)
        {
            _renderer.material.color = Color.yellow;
        }
    }
    private void OnMouseExit()
    {
        _renderer.material.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (!jengaManager.pieceSelected && jengaManager.canMove)
        {
            pieceGrabbed = true;
            _rigidbody.freezeRotation = true;
            _rigidbody.useGravity = false;
            jengaManager.pieceSelected = true;

            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }
    private void OnMouseUp()
    {
        if (pieceGrabbed)
        {
            pieceGrabbed = false;
            _rigidbody.freezeRotation = false;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = originalConstraints;
            jengaManager.pieceSelected = false;

        }
    }

    private void DragPiece()
    {

        _rigidbody.Sleep();

        if ( Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
        }
        

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 translatedPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        Vector3 newPosition = transform.position;

        if (xDown || yDown || zDown)
        {
            if (xDown)
            {
                newPosition.x = translatedPosition.x;
            }

            if (yDown)
            {
                newPosition.y = translatedPosition.y;
            }

            if (zDown)
            {
                newPosition.z = translatedPosition.z;
            }

        }
        else
        {
            newPosition = translatedPosition;
        }

        transform.position = newPosition;

    }
    

}
