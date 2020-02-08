using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform playerFollow;
    private void LateUpdate()
    {
        PlayerFollow();
    }

    private void PlayerFollow()
    {
        transform.position = playerFollow.position;
    }
       
}
