using UnityEngine;
using System.Collections;

public class Pistol : Weapon
{
    void Start()
    {
        gun_Spec.gunType = Gun_Kinds.GUNS;

        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;
    }
}
