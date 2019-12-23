using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Magnum : Weapon
{
   

    void Start()
    {
        //  gun_Spec.weaponName = "Magnum";
        //
        //  gun_Spec.ammuType = Ammunition_Kinds.BULLET;
        //  gun_Spec.maxAmmu = 6;
        //  gun_Spec.ammu_ForSpawn = Resources.Load("bullet") as GameObject;

        Ammo_property = 6;

        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;

    }
}
