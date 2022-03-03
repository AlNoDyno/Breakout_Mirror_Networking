using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public RectTransform mainPanel;

    public RectTransform playersPanel;

    public RectTransform bricksPanel;

    // static instance that can be referenced directly from Player script
    public static CanvasUI instance;

    void Awake()
    {
        instance = this;
    }
}