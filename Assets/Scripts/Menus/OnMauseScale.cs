using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMauseScale : MonoBehaviour
{
    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.3f, 1.3f);
    }
    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }
}
