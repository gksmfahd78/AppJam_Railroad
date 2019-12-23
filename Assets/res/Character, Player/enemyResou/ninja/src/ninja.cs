using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninja : Enemy {
    /*
     * 플레이어가 일정 거리 안으로 오면 등장준비상태가 된다
     * 등장준비상태에서 플레이어가 일정 거리 이상 떨어지면 모습을 드러내 추격상태가 된다
     * 추격상태일 때는 플레이어를 향해 돌진하며 이 때 플레이어와 일정 거리 이상 가까워지면 공격 준비상태가 된다.
     * 추격상태일 때 만약 플레이어가 공중에 있다면 점프하여 추격한다.
     * 공격준비 상태일 때 플레이어 앞에 도달하면 공격한다.
     */

    public enum State {Stealth, ChasingReady, Chasing, None};
    public State state = State.Stealth;

    Rigidbody2D rigidbody;
    Collider2D collider;
    Collider2D playercolls;
    Vector2 ChasingVelocity;
    bool isGround;
    bool AttackReady;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (healthSystem.isDead)
        {
            Delete();
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0;
            ani.SetTrigger("dead");
            return;
        }

        if (PlayerMinsu.PlayerInstance == null || stat.thisTrain != PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            return;
        }

        switch (state)
        {
            case State.Stealth:
                {
                    collider.enabled = false;
                    playercolls = Physics2D.OverlapCircle(transform.position, 10f, stat.playerLayer);
                    if (playercolls  != null && Mathf.Abs(this.transform.position.x - playercolls.gameObject.transform.position.x) < 2)
                    {
                        state = State.ChasingReady;
                    }
                    break;
                }
            case State.ChasingReady:
                {
                    playercolls = Physics2D.OverlapCircle(transform.position, 10f, stat.playerLayer);
                    if (Vector2.Distance(this.transform.position, playercolls.gameObject.transform.position) > 4 && state != State.None)
                    {
                        collider.enabled = true;
                        state = State.None;
                        rigidbody.gravityScale = 1;
                        ani.SetTrigger("Appear");
                        StartCoroutine("Appear");
                    }
                    break;
                }
            case State.Chasing:
                {
                    playercolls = Physics2D.OverlapCircle(transform.position, 20f, stat.playerLayer);

                    if (PlayerMinsu.PlayerInstance != null && PlayerMinsu.PlayerInstance.isDeath)
                    {
                        ani.SetTrigger("Idle");
                        return;
                    }

                    if (isGround)
                    {
                        ani.SetFloat("Blend", 0);
                        Chasing();
                        ChasingVelocity = rigidbody.velocity;
                        if (playercolls != null && Mathf.Abs(this.transform.position.x - playercolls.gameObject.transform.position.x) > 0.5f)
                        {
                            ChasingVelocity.x = spac.chasingSpeed * stat.direction;
                        }
                        else
                        {
                            ChasingVelocity.x = 0f;
                        }
                        rigidbody.velocity = ChasingVelocity;
                    }

                    if (playercolls != null && Mathf.Abs(this.transform.position.x - playercolls.gameObject.transform.position.x) < 4)
                    {
                        AttackReady = true;
                    } else AttackReady = false;

                    if (AttackReady &&isGround &&  this.transform.position.y + 0.5f <= playercolls.gameObject.transform.position.y)
                    {
                        Jump();
                    }

                    if (AttackReady && !stat.isUnderAttack &&
                        Vector2.Distance(this.transform.position, playercolls.gameObject.transform.position) < 1.5
                        && Mathf.Abs(this.transform.position.x - playercolls.gameObject.transform.position.x) < 1)
                    {
                        StartCoroutine("Attack");
                    }
                    break;
                }
        }
    }

    private void Jump()
    {
        ani.SetFloat("Blend", 1);
        isGround = false;
        float g = Mathf.Abs(rigidbody.gravityScale * Physics2D.gravity.y);
        Vector2 d = playercolls.gameObject.transform.position - transform.position;
        float h = Vector2.Distance(this.transform.position, playercolls.gameObject.transform.position) * 0.13f * g + d.y;    // h : 세로속도
        rigidbody.AddForce(new Vector2(h/7, h), ForceMode2D.Impulse);
        collider.isTrigger = true;
    }

    IEnumerator DownJump()
    {
        collider.isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        collider.isTrigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                {
                    PlatformEffector2D platformEffector2D = collision.gameObject.GetComponent<PlatformEffector2D>();
                    if (platformEffector2D != null)
                    {
                        return;
                    }
                    ani.SetFloat("Blend", 0);
                    isGround = true;
                    collider.isTrigger = false;
                    break;
                }
            case "Wall":
                {
                    collider.isTrigger = false;
                    Chasing();
                    if (transform.position.y > 0.6f)
                    {
                        rigidbody.AddForce(new Vector2(13f * stat.direction, 1f), ForceMode2D.Impulse);
                    }
                    break;
                }
            case "Ceiling":
                {
                    collider.isTrigger = false;
                    break;
                }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y - 0.5f >= playercolls.gameObject.transform.position.y && this.transform.position.y - PlayerMinsu.PlayerInstance.gameObject.transform.position.y > 0.5f)
            {
                PlatformEffector2D platformEffector2D = collision.gameObject.GetComponent<PlatformEffector2D>();
                if(platformEffector2D != null)
                {
                    StartCoroutine("DownJump");
                }
            }
            ani.SetFloat("Blend", 1);
            isGround = true;
        }
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(1f);
        ani.SetTrigger("Chasing");
        state = State.Chasing;
    }

    IEnumerator Attack()
    {
        stat.isUnderAttack = true;
        ani.SetTrigger("Attack");
        Collider2D playercoll = Physics2D.OverlapArea(this.transform.position - new Vector3(0f, 0.5f, 0), this.transform.position + new Vector3(2f * stat.direction, 0.5f, 0),stat.playerLayer);
        if (playercoll == enabled)
        {
            playercoll.gameObject.GetComponent<PlayerMinsu>().TakeDamage(spac.CreatDamageInfo());
        }
        state = State.None;
        if (isGround)
        {
            rigidbody.velocity = Vector2.zero;
        }
        yield return new WaitForSeconds(0.5f);
        ani.SetTrigger("Chasing");
        state = State.Chasing;
        stat.isUnderAttack = false;
    }

    void disappear()
    {
        Destroy(gameObject);
    }
}
