using UnityEngine;
using System.Collections;

public class SpringShoes : PassiveItem,IPassive
{ 
    public void PassiveAbility()
    {
        PlayerMinsu.PlayerInstance.playerStat.jumpForce += spec.increaseJumpForce;
        PlayerMinsu.PlayerInstance.playerSpec.maxJumpForce += spec.increaseJumpForce;
        canUsePassiveSkill = false;
    }
}
