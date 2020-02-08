using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookCollision : MonoBehaviour
{
    public bool canPool;
    public bool canGrab;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        canGrab = false;
        canPool = false;
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }
  public void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.collider == true)
        {
            if(collision.gameObject.tag == "Staff")
            {
                canGrab = false;
                canPool = true;
                particleSystem.Stop();
            }
            canGrab = true;
            particleSystem.Play();
        }
        else
        {
            canGrab = false;
            canPool = false;
            particleSystem.Stop();
        }
       
    }
}
