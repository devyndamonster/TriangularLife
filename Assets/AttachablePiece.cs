using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttachablePiece : MonoBehaviour
{
    public GameObject AttachmentContainer;

    private Clickable[] _attachmentPoints;
    private Clickable _grabbedAttachmentPoint;
    private LineRenderer _lineRenderer;

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
        Debug.Log("Attachment point grabbed");
        _grabbedAttachmentPoint = attachmentPoint;

        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointGrabbed(this, attachmentPoint);
    }

    public void OnAttachmentPointReleased(Clickable attachmentPoint)
    {
        Debug.Log("Attachment point released");
        _grabbedAttachmentPoint = null;

        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointReleased(this, attachmentPoint);
    }

    public void OnAttachmentPointEnter(Clickable attachmentPoint)
    {
        Debug.Log("Attachment point entered");
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointEnter(this, attachmentPoint);
    }

    public void OnAttachmentPointExit(Clickable attachmentPoint)
    {
        Debug.Log("Attachment point exited");
        PlayerCamera playerCamera = Camera.main.GetComponent<PlayerCamera>();
        playerCamera.OnAttachmentPointExit(this, attachmentPoint);
    }
}
