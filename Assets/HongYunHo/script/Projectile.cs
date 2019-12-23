using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public Rigidbody2D rigidbody; // 탄약오브젝트

    public GameObject shooter; //쏜 놈
    public string ShooterName;
    public Sprite ShooterSprite;

    public Vector2 targetDirection; //날아가는 방향
    public GameObject Target; // 피해를 줄 목표
    public GameObject weapon; // 발사한 총
    public float Damage; // 공격력
    public float explosionDamage; // 폭발 피해
    public float Speed; // 날아가는 속도
    public float accuracy; // 명중률
    public bool CanPenetrate; // 관통 가능 여부
    public float explosionRadius; // 폭발 범위
    public bool isSeeker; // 총알 유도 여부
    public bool isAffectedGravity; // 중력 작용 여부
    public float AimingDirection; // 정조준시 방향

    public bool isChasing;

    public LayerMask playerMask;
    public LayerMask EnemyMask;

    public void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void Shoot_Player()
    {
        rigidbody.AddForce(weapon.GetComponent<Weapon>().fire_Forward.right * Speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shooter != null && shooter.tag == collision.gameObject.tag || collision.gameObject.tag == "Bullet")
            return;
        else
        { 
            if (!CanPenetrate) // 관통이 가능 한 총알이아니라면 삭제
            {
                if(collision.tag == "Weapon" && PlayerMinsu.PlayerInstance.weapon.knife_Stat.reflectable)
                {
                    return;
                }
                if (collision.tag == "Enemy" && collision.gameObject.GetComponent<EnemyHealthSystem>().isDead)
                {
                    return;
                }
                Destroy(gameObject);
            }
            switch (collision.tag)
            {
                case "Enemy": collision.gameObject.GetComponent<EnemyHealthSystem>().TakeDamage(Damage);
                    break;
                case "Player":
                    var damageInfo = new EnemyDamageInfo();
                    damageInfo.Damage = (int)Damage;
                    damageInfo.ShooterName = ShooterName;
                    damageInfo.ShooterSprite = ShooterSprite;
                    collision.GetComponent<PlayerMinsu>().TakeDamage(damageInfo);
                    break;
            }

            if(explosionRadius > 0)
            {
                if(Target == null)
                {
                    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, EnemyMask);
                    foreach (Collider2D colls in hitColliders)
                    {
                        colls.gameObject.GetComponent<EnemyHealthSystem>().TakeDamage(explosionDamage);
                    }
                }
                else if (Target.gameObject.tag == "Player")
                {
                    Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, explosionRadius, playerMask);
                    var damageInfo = new EnemyDamageInfo();
                    damageInfo.Damage = (int)explosionDamage;
                    damageInfo.ShooterName = ShooterName;
                    damageInfo.ShooterSprite = ShooterSprite;
                    hitColliders.gameObject.GetComponent<PlayerMinsu>().TakeDamage(damageInfo);
                }
                Destroy(gameObject);
            }

        }
    }

    public void GetEnemySpec(EnemyGun enemygun) // 적의 스펙 가져오기
    {
        shooter = enemygun.thisChar;

        ShooterName = enemygun.ShooterName;
        ShooterSprite = enemygun.ShooterSprite;

        Target = enemygun.targetPlayer;
        Damage = enemygun.Damage;
        explosionDamage = enemygun.explosionDamage;
        Speed = enemygun.ShotSpeed;
        accuracy = enemygun.accuracy;
        isChasing = enemygun.isChasing;
        CanPenetrate = enemygun.CanPenetrate;
        explosionRadius = enemygun.explosionRadius;
        isSeeker = enemygun.isSeeker;
        isAffectedGravity = enemygun.isAffectedGravity;
        AimingDirection = enemygun.AimingDirection;
    }

    public void GetPlyerStats(Weapon playerGun) // 플레이어의 스텟 가져오기
    {
        shooter = PlayerMinsu.PlayerInstance.gameObject;
        weapon = playerGun.gameObject;
        Damage = playerGun.gun_Stat.attackDamage;
        Speed = playerGun.gun_Stat.shotSpeed;
        accuracy = playerGun.gun_Stat.accuracy;
        CanPenetrate = playerGun.gun_Stat.canPenetrate;
        explosionRadius = playerGun.gun_Stat.explosionRadius;
        Shoot_Player();
    }
}