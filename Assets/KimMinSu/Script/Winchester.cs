using UnityEngine;
using System.Collections;

public class Winchester : Weapon
{
    // Use this for initialization
    void Start()
    {

        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;

    }
}
