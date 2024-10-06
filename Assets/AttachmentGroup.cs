using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentGroup : MonoBehaviour
{
    [HideInInspector]
    public List<AttachablePiece> ChildAttachmentPieces = new List<AttachablePiece>();

    [HideInInspector]
    public Rigidbody2D Rigidbody;

    public void InitializeFromAttachablePiece(AttachablePiece attachablePiece)
    {
        Rigidbody = gameObject.AddComponent<Rigidbody2D>();
        var otherRigidbody = attachablePiece.GetComponent<Rigidbody2D>();

        Rigidbody.mass = otherRigidbody.mass;
        Rigidbody.drag = otherRigidbody.drag;
        Rigidbody.angularDrag = otherRigidbody.angularDrag;
        Rigidbody.gravityScale = otherRigidbody.gravityScale;
        Rigidbody.collisionDetectionMode = otherRigidbody.collisionDetectionMode;
        Rigidbody.sleepMode = otherRigidbody.sleepMode;
        Rigidbody.interpolation = otherRigidbody.interpolation;
        Rigidbody.constraints = otherRigidbody.constraints;
        Rigidbody.simulated = otherRigidbody.simulated;

        Destroy(otherRigidbody);

        transform.position = attachablePiece.transform.position;
        transform.rotation = attachablePiece.transform.rotation;

        attachablePiece.transform.SetParent(transform);
        attachablePiece.AttachmentGroup = this;
        ChildAttachmentPieces.Add(attachablePiece);
    }

    public void AttachToGroup(Clickable targetAttachmentPoint, AttachablePiece attachablePiece, Clickable otherAttachmentPoint)
    {
        foreach (AttachablePiece childAttachmentPiece in ChildAttachmentPieces)
        {
            childAttachmentPiece.OnOtherAttachedToGroup?.Invoke(attachablePiece);
        }

        if (attachablePiece.AttachmentGroup is AttachmentGroup attachmentGroup)
        {
            if(attachmentGroup.Rigidbody is Rigidbody2D otherRigidBody)
            {
                Destroy(otherRigidBody);
            }

            //Align the attachment group to the attachment point first
            AllignToAttachmentPoint(targetAttachmentPoint, attachmentGroup.transform, otherAttachmentPoint);

            //Now move over all the pieces from the other attachment group
            foreach (AttachablePiece childAttachmentPiece in attachmentGroup.ChildAttachmentPieces)
            {
                childAttachmentPiece.transform.SetParent(transform);
                childAttachmentPiece.AttachmentGroup = this;
                ChildAttachmentPieces.Add(childAttachmentPiece);
            }

            Destroy(attachmentGroup);
        }
        else
        {
            if(attachablePiece.GetComponent<Rigidbody2D>() is Rigidbody2D otherRigidBody)
            {
                Destroy(otherRigidBody);
            }

            AllignToAttachmentPoint(targetAttachmentPoint, attachablePiece.transform, otherAttachmentPoint);
            attachablePiece.transform.SetParent(transform);
            attachablePiece.AttachmentGroup = this;
            ChildAttachmentPieces.Add(attachablePiece);
        }
    }

    private void AllignToAttachmentPoint(Clickable targetAttachmentPoint, Transform elementToAllign, Clickable otherAttachmentPoint)
    {
        var ourForward = targetAttachmentPoint.transform.up;
        var otherForward = otherAttachmentPoint.transform.up;
        var angle = Vector3.SignedAngle(ourForward, -otherForward, Vector3.forward);

        elementToAllign.RotateAround(otherAttachmentPoint.transform.position, Vector3.forward, -angle);
        elementToAllign.position += (targetAttachmentPoint.transform.position - otherAttachmentPoint.transform.position);
    }
}
