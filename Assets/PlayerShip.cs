using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawLine(transform.position, (transform.position + (GetForwardVector() * 10)), Color.red);
        Debug.Log(GetForwardVector());
    }

    public Vector3 GetForwardVector()
    {
        return transform.up;
    }

    public float GetRotationAngleFromMouse()
    {
        var mousePosition = Input.mousePosition;
        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var facingVector = transform.up;
        var mouseVector = mousePosition - screenPosition;

        var angle = Vector2.SignedAngle(facingVector, mouseVector);
        return angle;
    }
}
