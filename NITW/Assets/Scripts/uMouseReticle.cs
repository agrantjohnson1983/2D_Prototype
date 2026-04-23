using UnityEngine;

public class uMouseReticle : MonoBehaviour
{
    private RectTransform rect;
    private Canvas parentCanvas;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        // Optional: Hide the system cursor
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePos,
            parentCanvas.worldCamera,
            out Vector2 localPoint);

        rect.anchoredPosition = localPoint;
    }
}
