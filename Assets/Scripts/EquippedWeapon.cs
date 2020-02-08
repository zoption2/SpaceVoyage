using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{
    public Transform shotDirection;

    private enum Weapons
    {
        Hook,
        Blaster
    };
    Weapons activeWeapon = Weapons.Hook;

    void Start()
    {
        
    }

    void Update()
    {
        switch (activeWeapon)
        {
            case Weapons.Hook:
                break;
            case Weapons.Blaster:
                break;
            default:
                break;
        }
    }
}
