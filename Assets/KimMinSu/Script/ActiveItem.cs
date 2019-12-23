using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Active_Spec
{
    public string name; // 아이템이름
    public float increaseMoveSpeed; //이동속도 증가량
    public float SlowScale; // 시간이 느려지는 정도
    public float InfluenceRange; // 영향범위
    public ActiveItemKind itemKind; // 아이템 종류
    public usingCondition usingCondition; // 작동조건
    public float ActiveTime; // 작동시간
    public float time;
    public float pusingForce;
}
[System.Serializable] 
public class Active_Stat
{
    [System.NonSerialized]
    public int killingEnemy; // 죽인 적들 체크하는 변수
    private int clearingTrainNum;
    public int clearedTrain
    {
        get
        {
            return clearingTrainNum;
        }
        set
        {
            if(!isFirstSetting && !canActive)
            {
                clearingTrainNum = value;
            }
        }
    }

    public bool isFirstSetting;
    public bool canActive;
    public float time;
}

public enum ActiveItemKind
{
    NONE,
    BLACKBALL,
    ARENALINE,
}

public enum usingCondition
{
    NONE,
    TIMER,
    CLEARTRAIN,
}

public interface IActive
{
    void ActiveAbility();
}

public class ActiveItem : MonoBehaviour
{
    
    public string text;
        

    [System.NonSerialized]
    public GameObject ActivePrefab;

    [Header("Spec")]
    public Active_Spec spec;

    [Header("Stat")]
    public Active_Stat stat;

    public LayerMask layer_Enemy;
    public LayerMask layer_Bullet;


   

    public void ActiveAbility_Item()
    {
        switch (spec.itemKind)
        {
            case ActiveItemKind.BLACKBALL:
                GetComponent<BlackBall>().ActiveAbility();
                break;
            case ActiveItemKind.ARENALINE:
                GetComponent<Adrenaline>().ActiveAbility();
                break;
        }
        
    }

    public void ClearTrainCount()
    {
        if(stat.clearedTrain == 3)
        {
            stat.clearedTrain = 0;
            stat.canActive = true;
        }
    }

    public void ActiveItemTimer()
    {
        stat.time -= Time.deltaTime;
        if(stat.time < 0f)
        {
            stat.canActive = true;
        }
    }

    public void DestroyOnGameObject()
    {
        GameManagerTaehyun.instance.SetTimeScale(1f);
    }

    private void OnDestroy()
    {
        DestroyOnGameObject();
    }

    /*protected IEnumerator ChargingActive()
    {
        yield return new WaitForSeconds(spec.time);
        stat.canActive = true;
    }*/
}
