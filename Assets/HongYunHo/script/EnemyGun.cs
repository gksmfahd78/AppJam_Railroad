using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public GameObject thisChar;

    public string ShooterName;
    public Sprite ShooterSprite;

    public int Damage; // 받아온 공격력을 저장하는 변수
    public float explosionDamage; // 폭발 피해
    public float ShotSpeed; // 날아가는 속도
    public float accuracy; // 명중률
    public bool CanPenetrate; // 관통 가능 여부
    public float explosionRadius; // 폭발 범위
    public bool isSeeker; // 총알 유도 여부
    public bool isAffectedGravity; // 중력 작용 여부
    public float AimingDirection; // 정조준시 방향
    public GameObject targetPlayer; // 받아온 플레이어 오브젝트를 저장하는 변수
    public Vector2 dir;
    float angle;

    public bool isChasing;

    public GameObject bullet; // 발사할 총알 프리팹

    public void Awake()
    {
        thisChar = transform.parent.gameObject; // 총의 사용자를 지정해줌
    }

    public void GetSpac(Enemy enemy) // 적의 스팩을 가져와 적용
    {
        ShooterName = enemy.spac.ShooterName;
        ShooterSprite = enemy.spac.ShooterSprite;
        Damage = enemy.spac.Damage;
        explosionDamage = enemy.spac.explosionDamage;
        ShotSpeed = enemy.spac.shotSpeed;
        accuracy = enemy.spac.gunAccuracy;
        CanPenetrate = enemy.spac.gunCanPenetrate;
        explosionRadius = enemy.spac.explosionRadius;
        isSeeker = enemy.spac.gunIsSeeker;
        isAffectedGravity = enemy.spac.gunisAffectedGravity;
    }

    public void ShootBullet()
    {
        GameObject.Instantiate(bullet, this.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().GetEnemySpec(this);
    }

    public void ShootShotGun(int Quantity)
    {
        for (; Quantity>0; Quantity--)
        {
            GameObject.Instantiate(bullet, this.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().GetEnemySpec(this);
        }
    }

    public void PlayerChasing() // 플레이어 방향 추격 메소드
    {
        if (targetPlayer != null) {
            dir = targetPlayer.transform.position - this.transform.position;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (this.transform.position.x < targetPlayer.gameObject.transform.position.x)
            {
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                this.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                this.transform.localScale = new Vector3(-1, -1, 1);
            }
        }
    }
}
