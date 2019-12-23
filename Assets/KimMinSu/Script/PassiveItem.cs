using UnityEngine;
using System.Collections;

[System.Serializable]
public class PassiveItemSpec
{
    public float increaseJumpForce;//점프력 증가량
    public int healAcount;// 힐링량
    public int increaseAcount_Bullet; // 가질수 있는 불렛량의 증가량
    public int increaseAcount_Energy; // 가질수 있는 에너지량의 증가량
    public int increaseAcount_Explosion; // 가질수 있는 폭발물량의 증가량
    public int increaseAcount_Shell; // 가질수 있는 셀의양의 증가량
    public PassiveItemKinds item; 
}

public enum PassiveItemKinds
{
    NONE,
    BACKPACK,
    VAMPIRETEETH,
    SPRINGSHOES,
}

public interface IPassive
{
    void PassiveAbility();
}

public class PassiveItem : MonoBehaviour
{
    [Header("Spec")]
    public PassiveItemSpec spec;

    public int killingEnemyCount; // 죽인 적의 수
    public bool canUsePassiveSkill; // 스킬을 사용할 수 있나 확인하는 변수
    

    public LayerMask layer_Enemy;
    public LayerMask layer_Bullet;

    private void Start()
    {
        canUsePassiveSkill = true;
        killingEnemyCount = 0;
        spec.increaseAcount_Shell = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Shell / 2;
        spec.increaseAcount_Bullet = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Bullet / 2;
        spec.increaseAcount_Energy = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Energy / 2; 
        spec.increaseAcount_Explosion = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Explosion / 2; 
    }

    //아이템 사용 후에 변수를 false로 바꿔주고 killing도 초기화시켜주기
    public void PassiveAbility_Item()
    {
        switch (spec.item)
        {
            case PassiveItemKinds.BACKPACK:
                GetComponent<BackPack>().PassiveAbility();
                break;
            case PassiveItemKinds.SPRINGSHOES:
                GetComponent<SpringShoes>().PassiveAbility();
                break;
            case PassiveItemKinds.VAMPIRETEETH:
                GetComponent<VampireTeeth>().PassiveAbility();
                break;
        }
    }
}
