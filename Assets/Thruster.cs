using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public TextMeshProUGUI DebugText;
    public float MaxThrustForce = 1000;

    [HideInInspector]
    public PlayerShip PlayerShip;

    private AttachablePiece _attachablePiece;
    private float _thrustAmount = 0;

    private void Start()
    {
        _attachablePiece = GetComponent<AttachablePiece>();
        var emmision = ParticleSystem.emission;
        emmision.rateOverTime = 0;
    }

    public void Thrust()
    {
        DebugText.text = "";

        var thrusterDirection = transform.up;

        var movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var movementDirectionNormalized = movementDirection.normalized;
        var movementDotProduct = Vector2.Dot(movementDirectionNormalized, thrusterDirection);

        var targetRotation = PlayerShip.GetRotationAngleFromMouse();
        var parentRigidbody = PlayerShip.AttachablePiece.GetParentRigidbody();
        var parentCenter = parentRigidbody.transform.position;
        var parentForward = parentRigidbody.transform.up;

        //If the thruster is perpendicular to its position on parent, then the thruster will have a leverage effect
        var relativePosition = transform.position - parentCenter;
        var relativePositionNormalized = relativePosition.normalized;
        var thrusterTorque = relativePosition.x * thrusterDirection.y - relativePosition.y * thrusterDirection.x;
        DebugText.text += $"Torque: {thrusterTorque}";
        Debug.DrawLine(parentCenter, parentCenter + relativePosition, Color.green);

        var thrusterRotationForce = Mathf.Clamp((targetRotation * thrusterTorque) / 100, 0, 1);
        DebugText.text += $"\nRot Thr Amount: {thrusterRotationForce}";

        _thrustAmount = Mathf.Clamp(movementDotProduct + thrusterRotationForce, 0, 1);

        DebugText.text += $"\nMov Thr Amount: {_thrustAmount}";

        var emission = ParticleSystem.emission;
        emission.rateOverTime = Mathf.Lerp(0, 100, _thrustAmount);

        if (_thrustAmount > 0.01f)
        {
            var thrustForce = Mathf.Lerp(0, MaxThrustForce, _thrustAmount);
            var forceVector = transform.up * thrustForce * Time.deltaTime;
            parentRigidbody.AddForceAtPosition(forceVector, transform.position, ForceMode2D.Force);
        }
    }
}
