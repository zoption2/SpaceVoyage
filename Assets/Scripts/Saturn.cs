using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saturn : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(0f, 0f, 2f * Time.deltaTime);
    }
}
