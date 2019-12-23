using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile {

    float gravity;
    float v;
    float h;

    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        EnemyTargetSet();
        Shoot_Enemy();
    }

    void Update()
    {
        if (isSeeker) // 유도탄이면 플레이어를 향해 유도
        {
            targetDirection = Target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rigidbody.velocity = targetDirection.normalized * Speed;
        }
    }

    public void Shoot_Enemy()
    {
        if (isAffectedGravity)
        {
            rigidbody.velocity = Vector2.zero;    // 다른 힘이 적용되면 방향이 흐트러지므로 초기화
            rigidbody.AddForce(new Vector2(v, h), ForceMode2D.Impulse);    // 힘 적용
        } else
            rigidbody.AddForce(targetDirection * Speed, ForceMode2D.Impulse); // 날아갈 방향 * 속도 만큼 힘을 줌
    }

    public void EnemyTargetSet()
    {
        if (isChasing && Target !=null) // 플레이어를 조준
        {
            targetDirection = (Target.transform.position - this.transform.position).normalized;
            targetDirection.x += Random.Range(-accuracy, accuracy);
            targetDirection.y += Random.Range(-accuracy, accuracy);
            targetDirection = targetDirection.normalized;
        }
        else if (isAffectedGravity) // 중력작용시 곡사포로 발사
        {
            rigidbody.gravityScale = 1;
            gravity = Mathf.Abs(rigidbody.gravityScale * Physics2D.gravity.y);    // g : 중력 (양수값으로)
            targetDirection = Target.transform.position - this.transform.position;
            v = targetDirection.x;    // v : 가로속도
            h = 0.5f * gravity + targetDirection.y;    // h : 세로속도
        }
        else // 좌 우 정조준
        {
            targetDirection = new Vector2(AimingDirection, 0);
            targetDirection.y += Random.Range(-accuracy, accuracy);
            targetDirection = targetDirection.normalized;
        }
    }

    public void TakePlayerAttack(bool Reflectable)
    {
        if(Reflectable)
        {
            rigidbody.gravityScale = 0;
            rigidbody.velocity = Vector2.zero;
            // 반사를 위한 준비

            Target = shooter;
            shooter = PlayerMinsu.PlayerInstance.gameObject;
            // 타겟과 슈터를 서로 바꾼다

            targetDirection = (Target.transform.position - this.transform.position).normalized;
            rigidbody.AddForce(targetDirection * Speed, ForceMode2D.Impulse);
            // 반사시켜 돌려보낸다
        }
        else // 반사가 불가능일 때는 삭제시킴
        {
            Destroy(this.gameObject);
        }
    }
}
