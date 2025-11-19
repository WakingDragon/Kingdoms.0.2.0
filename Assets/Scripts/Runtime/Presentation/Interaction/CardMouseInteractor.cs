using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Attach this to a UI thing with a rect transform and, if it is in the UI mode, it will log the outcome.
/// </summary>
public class CardMouseInteractor : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private Action<int> _onClick;
    private int _cardId;

    public void SetClickAction(Action<int> onClick, int cardId)
    {
        _onClick = onClick;
        _cardId = cardId;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse entered " + name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Mouse exited " + name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke(_cardId);
    }
}