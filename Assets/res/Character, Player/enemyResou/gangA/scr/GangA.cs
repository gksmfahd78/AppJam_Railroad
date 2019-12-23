using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangA : Enemy {

    void Start()
    {
        GetComponentInChildren<EnemyGun>().GetSpac(this);
        StartCoroutine("NonDetectAct");
    }

    void Update () {

        if (healthSystem.isDead) // 살아있을 때
        {
            Delete();
            return;
        }

        if (!stat.isDetect && PlayerMinsu.PlayerInstance != null && stat.thisTrain == PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            Detect();
        }

        if (PlayerMinsu.PlayerInstance != null && stat.isDetect) // 플레이어 감지중
        {
            this.transform.GetChild(0).GetComponent<EnemyGun>().isChasing = false;
            GetComponentInChildren<EnemyGun>().targetPlayer = PlayerMinsu.PlayerInstance.gameObject;
            if (stat.isUnderAttack == false)
            {
                StartCoroutine("Attack");
            }
            else if (stat.isUnderAttack == false) // 내가 공격중이 아니면 플레이어 추격
            {
                Chasing();
            }
        }
        if (stat.isMoveing)
        {
            ani.SetFloat("Blend", 1f);
            CliffCheck();
            transform.Translate(spac.moveSpeed * Time.deltaTime * stat.direction, 0, 0);
        }
        else ani.SetFloat("Blend", 0f);
    }

    IEnumerator Attack() // 공격 코루틴
    {
        Chasing();
        ani.SetTrigger("attack");
        stat.isMoveing = false;
        stat.isUnderAttack = true;
        this.transform.GetChild(0).GetComponent<EnemyGun>().AimingDirection = stat.direction;
        yield return new WaitForSeconds(0.5f);
        Chasing();
        this.transform.GetChild(0).GetComponent<EnemyGun>().ShootBullet();
        yield return new WaitForSeconds(Random.Range(0.5f,1f));
        stat.isMoveing = true;
        stat.isUnderAttack = false;
    }
}
