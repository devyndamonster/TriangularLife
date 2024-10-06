using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject TargetObject;
    public float Zoom = 10;
    public float MinZoom = 5;

    private Clickable _grabbedAttachmentPoint;
    private AttachablePiece _grabbedAttachable;
    private Clickable _hoveredAttachmentPoint;
    private AttachablePiece _hoveredAttachable;
    private LineRenderer _lineRenderer;

    private void Update()
    {
        transform.position = new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, -10);

        if (Input.mouseScrollDelta.y > 0)
        {
            Zoom += 1;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Zoom -= 1;
        }

        if (Zoom < MinZoom)
        {
            Zoom = MinZoom;
        }

        Camera.main.orthographicSize = Zoom;

        UpdateGrabLine();
    }

    private void UpdateGrabLine()
    {
        if(_lineRenderer != null && _grabbedAttachmentPoint != null)
        {
            _lineRenderer.SetPosition(0, _grabbedAttachmentPoint.transform.position);
            _lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void OnAttachmentPointGrabbed(AttachablePiece attachable, Clickable clickable)
    {
        _grabbedAttachable = attachable;
        _grabbedAttachmentPoint = clickable;

        if(_lineRenderer != null)
        {
            Destroy(_lineRenderer);
        }

        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.white;
        _lineRenderer.endColor = Color.white;
        _lineRenderer.alignment = LineAlignment.TransformZ;
    }

    public void OnAttachmentPointReleased(AttachablePiece attachable, Clickable clickable)
    {
        if (_grabbedAttachable != null && _grabbedAttachmentPoint != null)
        {
            if (_hoveredAttachable != null && _hoveredAttachmentPoint != null)
            {
                _hoveredAttachable.Attach(_hoveredAttachmentPoint, _grabbedAttachable, _grabbedAttachmentPoint);
            }
        }

        _grabbedAttachable = null;
        _grabbedAttachmentPoint = null;
        
        if(_lineRenderer != null)
        {
            Destroy(_lineRenderer);
        }
    }

    public void OnAttachmentPointEnter(AttachablePiece attachable, Clickable clickable)
    {
        _hoveredAttachable = attachable;
        _hoveredAttachmentPoint = clickable;
    }

    public void OnAttachmentPointExit(AttachablePiece attachable, Clickable clickable)
    {
        _hoveredAttachable = null;
        _hoveredAttachmentPoint = null;
    }
}
