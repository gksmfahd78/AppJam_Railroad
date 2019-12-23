using UnityEngine;
using UnityEditor;

public class VampireTeeth : PassiveItem, IPassive
{
    public void PassiveAbility()
    {
        if(killingEnemyCount == 10)
        {
            PlayerMinsu.PlayerInstance.Health += spec.healAcount;
            killingEnemyCount = 0;
        }
    }
}