using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Weapon {
    new void OnEnable()
    {
        base.OnEnable();
        knife_Stat.isFirstAttak = true;
        if (knife_Stat.isFirstAttak)
        {
            knife_Stat.damage = knife_Spec.damage * 2f;
            
        }
    }
}
