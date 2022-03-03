using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public RectTransform mainPanel;

    public RectTransform playersPanel;

    public RectTransform bricksPanel;

    public static CanvasUI instance;

    void Awake()
    {
        instance = this;
    }
}