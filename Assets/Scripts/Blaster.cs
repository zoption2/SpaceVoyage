using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{

    Transform player;
    private SpriteRenderer spriteRenderer;
    Sprite blaster;
    void Start()
    {
        player = transform.Find("Player");
        spriteRenderer = player.Find("hands").GetComponentInChildren<SpriteRenderer>();
        blaster = Resources.Load("Blaster") as Sprite;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Yes");
            spriteRenderer.sprite = blaster;
        }
    }
}
