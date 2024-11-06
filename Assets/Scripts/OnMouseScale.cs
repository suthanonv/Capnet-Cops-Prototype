using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseScale : MonoBehaviour
{

    public List<Color> colors = new List<Color>();
    public Image button;

    public void OnEnable()
    {
        transform.localScale = new Vector2(1, 1);
        button.color = colors[0];
    }

    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.25f, 1.25f);
        button.color = colors[1];
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1, 1);
        button.color = colors[0];
    }
}
