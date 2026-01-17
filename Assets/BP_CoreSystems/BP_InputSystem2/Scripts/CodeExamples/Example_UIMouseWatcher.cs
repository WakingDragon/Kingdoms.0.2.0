using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this to a UI thing with a rect transform and, if it is in the UI mode, it will log the outcome.
/// extend it by introducing more event interfaces as needed.
/// </summary>
public class Example_UIMouseWatcher : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered " + name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited " + name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked " + name);
    }
}