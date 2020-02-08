using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    Vector2 mousePos;
    private void Start()
    {
       Cursor.visible = false;
        
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 60) * Time.deltaTime);
    }
    private void LateUpdate()
    {
        CursorRules();
    }

    private void CursorRules()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distancePlayerMouse = Vector2.Distance(transform.parent.position, mousePos );

        if (distancePlayerMouse <= 9)
        {
            transform.position = mousePos;
        }
    }
}
