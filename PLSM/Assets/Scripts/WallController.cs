using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class WallController : MonoBehaviour
{
    public bool vertical;

    [SerializeField]
    private GameObject Parent;

    private RectTransform ParentRectTransform;
    private BoxCollider2D box;

    void OnEnable()
    {
        box = GetComponent<BoxCollider2D>();
        ParentRectTransform = Parent.GetComponent<RectTransform>();

        float parentW = ParentRectTransform.rect.width;
        float parentH = ParentRectTransform.rect.height;

        if (vertical)
        {
            box.size = new Vector2(GetComponent<RectTransform>().rect.width, parentH);
        }
        else
        {
            box.size = new Vector2(parentW, GetComponent<RectTransform>().rect.height);
        }
    }
}
