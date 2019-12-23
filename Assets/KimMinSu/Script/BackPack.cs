using UnityEngine;
using System.Collections;

public class BackPack : PassiveItem,IPassive
{

    public void PassiveAbility()
    {
        PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Bullet += spec.increaseAcount_Bullet;
        PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Energy += spec.increaseAcount_Energy;
        PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Explosion += spec.increaseAcount_Explosion;
        PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Shell += spec.increaseAcount_Shell;
        canUsePassiveSkill = false;
    }
}
