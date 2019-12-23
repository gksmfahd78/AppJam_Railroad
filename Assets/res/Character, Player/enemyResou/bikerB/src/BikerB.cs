using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikerB : Enemy {

    public int ShotGunQuantity; //한번에 발사할 총알의 수

    void Start()
    {
        GetComponentInChildren<EnemyGun>().GetSpac(this);
        StartCoroutine("NonDetectAct");
        this.transform.GetChild(0).GetComponent<EnemyGun>().isChasing = true;
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
            if (stat.isUnderAttack == false)
            {
                StartCoroutine("Attack");
            }
            GetComponentInChildren<EnemyGun>().targetPlayer = PlayerMinsu.PlayerInstance.gameObject;
            GetComponentInChildren<EnemyGun>().PlayerChasing();
            // 자식의 EnemyGun 스크립트의 targetPlayer를 지정해줌
            Chasing();
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
        stat.isUnderAttack = true;

        yield return new WaitForSeconds(0.5f);
        this.transform.GetChild(0).GetComponent<EnemyGun>().ShootShotGun(ShotGunQuantity);
        yield return new WaitForSeconds(Random.Range(0.5f,2f));

        stat.isUnderAttack = false;
    }
}
