using UnityEngine;
using System.Collections;

public class MP5 : Weapon
{
    // Use this for initialization
    void Start()
    {
        gun_Spec.gunType = Gun_Kinds.GUNS;

        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;
    }

}
