using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttachablePiece : MonoBehaviour
{
    public GameObject AttachmentContainer;

    [HideInInspector]
    public AttachmentGroup AttachmentGroup;

    private Clickable[] _attachmentPoints;

    void Start()
    {
        _attachmentPoints = AttachmentContainer.GetComponentsInChildren<Clickable>();

        foreach (Clickable attachmentPoint in _attachmentPoints)
        {
            attachmentPoint.OnDown += OnAttachmentPointGrabbed;
            attachmentPoint.OnUp += OnAttachmentPointReleased;
            attachmentPoint.OnEnter += OnAttachmentPointEnter;
            attachmentPoint.OnExit += OnAttachmentPointExit;
        }
    }

    public void OnAttachmentPointGrabbed(Clickable attachmentPoint)
    {
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointGrabbed(this, attachmentPoint);
    }

    public void OnAttachmentPointReleased(Clickable attachmentPoint)
    {
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointReleased(this, attachmentPoint);
    }

    public void OnAttachmentPointEnter(Clickable attachmentPoint)
    {
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointEnter(this, attachmentPoint);
    }

    public void OnAttachmentPointExit(Clickable attachmentPoint)
    {
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointExit(this, attachmentPoint);
    }

    public void Attach(Clickable attachmentPoint, AttachablePiece otherAttachable, Clickable otherAttachmentPoint)
    {
        if(AttachmentGroup != null)
        {
            AttachmentGroup.AttachToGroup(attachmentPoint, otherAttachable, otherAttachmentPoint);
        }
        else
        {
            var newAttachmentGroup = new GameObject("AttachmentGroup").AddComponent<AttachmentGroup>();
            newAttachmentGroup.InitializeFromAttachablePiece(this);
            newAttachmentGroup.AttachToGroup(attachmentPoint, otherAttachable, otherAttachmentPoint);
        }
    }
}
