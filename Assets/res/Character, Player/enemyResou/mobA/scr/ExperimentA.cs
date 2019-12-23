using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentA : Enemy {

    Rigidbody2D rigidbody;
    Collider2D collider;

    public bool isJump;
    public float jumpForceX;
    public float jumpForceY;

    void Start()
    {
        StartCoroutine("NonDetectAct");
        ani.SetFloat("Blend",0f);
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
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
            StopCoroutine("NonDetectAct");
            ani.SetFloat("Blend", 1f);
            stat.isMoveing = false;
            if (Vector2.Distance(this.transform.position, PlayerMinsu.PlayerInstance.gameObject.transform.position) <= 1f
                && stat.isUnderAttack == false) //플레이어와의 거리계산, 내가 공격중이 아닐 때
            {
                stat.isUnderAttack = true;
                ani.SetTrigger("attack");
            }
            if (stat.isUnderAttack == false) // 내가 공격중이 아니면 플레이어 추격
            {
                Chasing();
                transform.Translate(spac.chasingSpeed * Time.deltaTime * stat.direction, 0, 0);
            }
        }
        else if (stat.isMoveing)
        {
            transform.Translate(spac.moveSpeed * Time.deltaTime * stat.direction, 0, 0);
            CliffCheck();
        }
    }


    void Attack() // 공격 코루틴
    {
        Chasing();
        Collider2D attackColl = Physics2D.OverlapCircle(transform.position + new Vector3(0.5f, 0, 0) * stat.direction, 0.1f, stat.playerLayer);
        if ( attackColl != null)
        {
            attackColl.gameObject.GetComponent<PlayerMinsu>().TakeDamage(spac.CreatDamageInfo()); // 범위 체크로 플레이어에게 피해를 줌
        }
        stat.isUnderAttack = false;
    }

    void Jump()
    {
        isJump = true;
        float g = Mathf.Abs(rigidbody.gravityScale * Physics2D.gravity.y);
        Vector2 d = PlayerMinsu.PlayerInstance.gameObject.transform.position - transform.position;
        float h = 0.2f * g + d.y;    // h : 세로속도
        rigidbody.AddForce(new Vector2(0, h), ForceMode2D.Impulse);
    }

    IEnumerator DownJump()
    {
        collider.isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        collider.isTrigger = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            PlatformEffector2D platformEffector2D = collision.gameObject.GetComponent<PlatformEffector2D>();
            if (platformEffector2D != null)
            {
                if (!stat.isUnderAttack && this.transform.position.y - 1f >= PlayerMinsu.PlayerInstance.gameObject.transform.position.y)
                {
                    StartCoroutine("DownJump");
                }
            }
            else
            {
                isJump = false;
                if (!isJump && this.transform.position.y + 0.5f <= PlayerMinsu.PlayerInstance.gameObject.transform.position.y && this.transform.position.x - PlayerMinsu.PlayerInstance.gameObject.transform.position.x < 2f)
                {
                    Jump();
                }
            }
        }
        if (collision.gameObject.tag == "Ceiling")
        {
            collider.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            PlatformEffector2D platformEffector2D = collision.gameObject.GetComponent<PlatformEffector2D>();
            if (platformEffector2D == null)
            {
                collider.isTrigger = false;
            }
        } 
    }

    void disappear()
    {
        Destroy(gameObject);
    }
}
