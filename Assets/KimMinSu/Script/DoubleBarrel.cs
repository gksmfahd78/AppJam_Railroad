using UnityEngine;
using System.Collections;

public class DoubleBarrel : Weapon  
{
    // Use this for initialization
    void Start()
    {
        gun_Spec.gunType = Gun_Kinds.SHOTGUN;
        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;
    }
}
