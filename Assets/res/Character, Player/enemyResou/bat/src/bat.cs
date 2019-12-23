using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bat : Enemy {

    public ParticleSystem particle;
    public ParticleSystem particle2;
    Rigidbody2D rigidbody;
    Vector2 playerDir;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //particle = GetComponent<ParticleSystem>();
        //particle2 = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (healthSystem.isDead)
        {
            Delete();
            ani.SetTrigger("dead");
            return;
        }

        if (!stat.isDetect && PlayerMinsu.PlayerInstance != null && stat.thisTrain == PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            Detect();
        }

        if (PlayerMinsu.PlayerInstance != null && stat.isDetect) // 플레이어 감지중
        {
            ani.SetTrigger("detect");
            StopCoroutine("NonDetectAct");
            if (Vector2.Distance(this.transform.position, PlayerMinsu.PlayerInstance.gameObject.transform.position) <= 0.6f
                && stat.isUnderAttack == false) //플레이어와의 거리계산, 내가 공격중이 아닐 때
            {
                rigidbody.velocity = Vector2.zero;
                stat.isUnderAttack = true;
                ani.SetTrigger("BOOM");
            }
            else if (stat.isUnderAttack == false) // 내가 공격중이 아니면 플레이어 추격
            {
                Chasing();
                playerDir = PlayerMinsu.PlayerInstance.gameObject.transform.position - this.transform.position;
                rigidbody.AddForce(playerDir * Time.deltaTime * spac.chasingSpeed);
                rigidbody.velocity *= 0.95f;
            }
        }
    }

    void PlayEffect()
    {
        Instantiate(particle, transform.position, Quaternion.identity).Play();
        Instantiate(particle2, transform.position, Quaternion.identity).Play();
    }

    void BOOM()
    {
        PlayEffect();
        var player = Physics2D.OverlapCircle(transform.position, spac.explosionRadius, stat.playerLayer);
        if (player != null)
        {
            player.GetComponent<PlayerMinsu>().TakeDamage(spac.CreatDamageInfo());
        }
        if (PlayerMinsu.PlayerInstance.passiveItems != null)
        {
            foreach (var passive in PlayerMinsu.PlayerInstance.passiveItems)
            {
                if (passive != null && passive.canUsePassiveSkill)
                {
                    passive.killingEnemyCount += 1;
                }
            }
        }
    }

    public void dis()
    {
        healthSystem.TakeDamage(2);
        Destroy(gameObject);
    }
}
