using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Rigidbody2D rb;
    public float individualCoefficient;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate ()
    {
        Gravity[] attractors = FindObjectsOfType<Gravity>();
        foreach (var item in attractors)
        {
            if (item != this)
            {
               Attract(item);
            }
            
        }
    }
    private void Attract (Gravity objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;
        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;
        var distanceToTheEdge = direction.magnitude - rb.transform.localScale.x - rbToAttract.transform.localScale.x;
        if (distanceToTheEdge < 4 && distanceToTheEdge >= 2)
        {
            float forceMagnitude = Constants.G * individualCoefficient * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            Vector2 force = direction.normalized * forceMagnitude;
            rbToAttract.AddForce(force);
        }

        else if (distanceToTheEdge < 2 )
        {
            float forceMagnitude = Constants.G * 6f * individualCoefficient * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
            Vector2 force = direction.normalized * forceMagnitude;
            rbToAttract.AddForce(force);
        }

    }
}
