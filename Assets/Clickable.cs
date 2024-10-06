using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public SpriteRenderer SpriteRenderer;

    public delegate void PointerAction(Clickable clickable);
    public PointerAction OnDown;
    public PointerAction OnUp;
    public PointerAction OnEnter;
    public PointerAction OnExit;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SpriteRenderer.color = Color.green;
        OnEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpriteRenderer.color = Color.red;
        OnExit?.Invoke(this);
    }
}
