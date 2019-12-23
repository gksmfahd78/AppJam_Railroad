using UnityEngine;
using System.Collections;

public class Minigun : Weapon
{

    void Start()
    {

        gun_Stat.Gun_State = Gun_State.NONE;

        Ammo_property = gun_Spec.maxAmmu;
    }


    private void OnDisable()
    {
        gun_Stat.isHeated = false;
    }
}
