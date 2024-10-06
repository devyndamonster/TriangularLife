using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [HideInInspector]
    public AttachablePiece AttachablePiece;

    private List<Thruster> _thrusters = new List<Thruster>();

    private void Start()
    {
        AttachablePiece = GetComponent<AttachablePiece>();
        AttachablePiece.OnOtherAttachedToGroup += OnOtherAttachedToGroup;
    }

    private void Update()
    {
        UpdateThrusters();
    }

    private void UpdateThrusters()
    {
        foreach (var thruster in _thrusters)
        {
            //Todo: we should probably just calculate the thrust logic here and then call with the force
            thruster.Thrust();
        }
    }

    private void OnOtherAttachedToGroup(AttachablePiece otherAttachable)
    {
        var otherParent = otherAttachable.transform;

        if(otherAttachable.AttachmentGroup != null)
        {
            otherParent = otherAttachable.AttachmentGroup.transform;
        }

        var newThrusters = otherParent.GetComponentsInChildren<Thruster>();
        _thrusters.AddRange(newThrusters);

        foreach (var thruster in newThrusters)
        {
            thruster.PlayerShip = this;
        }
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
