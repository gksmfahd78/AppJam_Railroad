using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{

    public bool isDead;

    public float maxHP;
    public float Hp;

    public int dropAmmoQuantity;
    public float AmmodropPercentage;

    ParticleSystem particle;
    Enemy enemy;

    void Start()
    {
        //maxHp = maxHp * 진행도
        //dropAmmoQuantity = Random.Range(1,진행도)
        AmmodropPercentage = 0.5f;
        dropAmmoQuantity = 1;
        Hp = maxHP;
        particle = GetComponent<ParticleSystem>();
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        if (PlayerMinsu.PlayerInstance != null && enemy.stat.thisTrain == PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            this.Hp = this.Hp - damage;

            if (this.Hp <= 0)
            {
                if (!isDead)
                {
                    this.isDead = true;
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

                    if (PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain != null)
                    {
                        //GameManager.instance.MonsterSpawnRule--;
                        PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<MonsterSpawn>().monsterCount--;
                        //Debug.Log(Player.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<MonsterSpawn>().monsterSpawnRule);
                    }
                    //Player.PlayerInstance.monsterSpawn.monsterSpawnRule

                    if (Random.value <= AmmodropPercentage)
                    {
                        for (; dropAmmoQuantity > 0; dropAmmoQuantity--)
                        {
                            GameManagerTaehyun.instance.CreateDropItem(ItemType.Ammo, transform.position);
                        }
                    }
                }
                Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), PlayerMinsu.PlayerInstance.gameObject.GetComponent<Collider2D>());
                if (GetComponent<Enemy>().spac.deadIsOverride)
                    return;
                enemy.Dead();
            }
            else if(PlayerMinsu.PlayerInstance != null && enemy.spac.canTakeKnockback)
            {
                int dir;
                if (transform.position.x - PlayerMinsu.PlayerInstance.gameObject.transform.position.x > 0)
                {
                    dir = 1;
                }
                else dir = -1;
                transform.Translate(enemy.spac.takeKnockbackValue * dir, 0,0);
            }
            if (enemy.spac.takeDamageParticle)
            {
                particle.Play();
            }
        }
    }

    IEnumerator TakePoisonDamage()
    {
        yield return new WaitForSeconds(0.5f); // 독 뎀지 딜레이
    }
}