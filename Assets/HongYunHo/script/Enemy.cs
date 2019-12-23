using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpac
{
    public bool deadIsOverride; //죽을 때의 함수가 오버라이딩 됨

    public float moveSpeed; // 기본 이동속도
    public float chasingSpeed; // 추격중 이동속도

    public string ShooterName; //공격자의 이름
    public Sprite ShooterSprite; // 공격자의 스프라이트

    public int Damage; // 공격력
    public float shotSpeed; // 투사체 속도
    public float gunAccuracy; //명중률
    public bool gunCanPenetrate; // 관통 가능 여부
    public float explosionRadius; // 폭발 범위
    public bool gunIsSeeker; // 총알 유도 여부
    public bool gunisAffectedGravity; // 총알의 중력 작용 여부
    public float explosionDamage; // 폭발 피해

    public bool canTakeKnockback;
    public float takeKnockbackValue;
    public bool takeDamageParticle;
    public float deathTorqueValue;
    public bool NotDeathTorque;

    public EnemyDamageInfo CreatDamageInfo()
    {
        var info = new EnemyDamageInfo();
        info.Damage = Damage;
        info.ShooterName = ShooterName;
        info.ShooterSprite = ShooterSprite;

        return info;
    }
}

[Serializable]
public class EnemyStat
{
    public Collider2D playercolls;
    public LayerMask playerLayer; // 플레이어 레이어
    public LayerMask groundLayer;
    public LayerMask trainLayer;

    public GameObject thisTrain;
    public bool isDetect; // 플레이어 감지 여부
    public bool isUnderAttack; // 공격중일 때 true
    public bool isMoveing; // 이동중일 때 true

    public int direction; // 나의 방향, -1은 왼쪽, 1은 오른쪽

    public int Damage; // 공격력

    public float detectTime;
}

public class Enemy : MonoBehaviour {

    protected Animator ani;

    public EnemySpac spac;
    public EnemyStat stat;

    protected EnemyHealthSystem healthSystem;

    protected void Awake()
    {
        ani = GetComponent<Animator>();
        healthSystem = this.GetComponent<EnemyHealthSystem>();
    }

    protected void Start()
    {
        GetComponentInChildren<EnemyGun>().GetSpac(this);
    }

    protected void Chasing() // 플레이어 방향 추격 메소드
    {
        if (PlayerMinsu.PlayerInstance == null)
            return;
        if (this.transform.position.x < PlayerMinsu.PlayerInstance.gameObject.transform.position.x)
        {
            stat.direction = 1;
            this.transform.localScale = new Vector3(stat.direction, 1, 1);
        }
        else
        {
            stat.direction = -1;
            this.transform.localScale = new Vector3(stat.direction, 1, 1);
        }
    }

    protected void CliffCheck()
    {
        Vector2 startPiont = new Vector2(transform.position.x + stat.direction, transform.position.y);
        Vector2 rayDirection = Vector2.down;
        float distance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(startPiont, rayDirection, distance, stat.groundLayer);

        if (hit.collider == null)
        {
            stat.isMoveing = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 startPiont = new Vector2(transform.position.x + stat.direction, transform.position.y);
        float distance = 1f;
        Vector2 to = new Vector2(startPiont.x, startPiont.y - distance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPiont, to);
    }

    protected void Detect()
    {
        stat.detectTime += Time.deltaTime;
        if (stat.detectTime >= 0.5f)
            stat.isDetect = true;
    }

    IEnumerator NonDetectAct() //플레이어를 발견하지 않았을 때 행동
    {
        float num;
        num = UnityEngine.Random.value;
        if (num >= 0.5f)
        {
            stat.direction = -1;
        }
        else
        {
            stat.direction = 1;
        }
        this.transform.localScale = new Vector3(stat.direction, 1, 1);
        num = UnityEngine.Random.value;
        if (num >= 0.5f)
        {
            stat.isMoveing = true;
        }
        else stat.isMoveing = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
        StartCoroutine("NonDetectAct");
    }

    public void Dead()
    {
        ani.SetTrigger("dead");
        StopAllCoroutines();

        GetComponent<Rigidbody2D>().gravityScale = 1;
        GetComponent<Rigidbody2D>().freezeRotation = false;
        if (spac.NotDeathTorque)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2f),ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(-spac.deathTorqueValue, spac.deathTorqueValue) , ForceMode2D.Impulse);
    }

    public void Delete()
    {
        if(transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }
}