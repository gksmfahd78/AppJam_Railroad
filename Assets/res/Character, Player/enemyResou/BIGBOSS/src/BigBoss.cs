using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoss : Enemy {

    bool isShooting;
    float moveDir;

    public float jumpForceX;
    public float jumpForceY;

    bool isJump;
    public int jumpDamage;
    public float KnockbackForceX;
    public float KnockbackForceY;

    Rigidbody2D rigidbody;

    void Start()
    {

        stat.thisTrain = transform.parent.gameObject;
        this.transform.GetChild(0).GetComponent<EnemyGun>().GetSpac(this);
        this.transform.GetChild(1).GetComponent<EnemyGun>().GetSpac(this);
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update () {

        if (healthSystem.isDead)
        {
            stat.isMoveing = false;
            stat.isDetect = false;
            isShooting = false;
            isJump = false;
            stat.isDetect = false;
            moveDir = 0;
            Delete();
            StopAllCoroutines();
            ani.SetTrigger("Dead");
            return;
        }

        if (!stat.isDetect && PlayerMinsu.PlayerInstance != null && stat.thisTrain == PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            SoundManagerTaehyun.instance.PlayTheBossBGM();
            Detect();
        }

        if (stat.isDetect && !stat.isUnderAttack)
        {
            stat.playercolls = Physics2D.OverlapCircle(transform.position, 10f, stat.playerLayer);
            this.transform.GetChild(0).GetComponent<EnemyGun>().isChasing = false;
            this.transform.GetChild(0).GetComponent<EnemyGun>().targetPlayer = stat.playercolls.gameObject;
            this.transform.GetChild(1).GetComponent<EnemyGun>().targetPlayer = stat.playercolls.gameObject;
            ani.SetTrigger("Detect");

            if (ani.GetCurrentAnimatorStateInfo(0).IsName("attack1_BlendTree"))
            {
                stat.isUnderAttack = true;
                StartCoroutine("Attack");
                StartCoroutine("AttackMove");
            }
        }
        if (stat.isDetect && !isShooting)
        {
            Chasing();
        }
        if (stat.isMoveing)
        {
            ani.SetFloat("Blend",1f);
            transform.Translate(new Vector2(moveDir,0) * Time.deltaTime);
        }else ani.SetFloat("Blend", 0);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        float num;
        num = UnityEngine.Random.value;
        if(num >= 0.45f && !isShooting && !isJump)
        {
            StartCoroutine("Shoot");
        }
        else if(num < 0.45f && !isJump)
        {
            ani.SetFloat("JumpBlend", 0f);
            StopCoroutine("Shoot");
            isShooting = false;
            ani.SetTrigger("Jump");
        }else yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        StartCoroutine("Attack");
    }

    IEnumerator Shoot()
    {
        this.transform.GetChild(0).GetComponent<EnemyGun>().AimingDirection = stat.direction;
        this.transform.GetChild(1).GetComponent<EnemyGun>().AimingDirection = stat.direction;
        isShooting = true;
        for (int i = 0; i < 20 && isShooting; i++)
        {
            this.transform.GetChild(0).GetComponent<EnemyGun>().ShootBullet();
            yield return new WaitForSeconds(0.15f);
            this.transform.GetChild(1).GetComponent<EnemyGun>().ShootBullet();
            yield return new WaitForSeconds(0.1f);
        }
        isShooting = false;
    }

    IEnumerator AttackMove() //발사중 이동
    {
        float num;
        num = UnityEngine.Random.value;
        if (num >= 0.5f)
            moveDir = 1;
        else
            moveDir = -1;

        num = UnityEngine.Random.value;
        if (num >= 0.5f)
        {
            stat.isMoveing = true;
        }
        else stat.isMoveing = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
        StartCoroutine("AttackMove");
    }

    void Jump()
    {
        rigidbody.AddForce(new Vector2(jumpForceX * stat.direction, jumpForceY), ForceMode2D.Impulse);
        isJump = true;
        stat.isMoveing = false;
        moveDir = 0;
        StopAllCoroutines();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isJump && collision.gameObject.tag == "Ground")
        {
            ani.SetFloat("JumpBlend", 1f);
            ani.SetTrigger("Lending");
            isJump = false;
            StartCoroutine("Attack");
            StartCoroutine("AttackMove");
            if(Random.value >= 0.45f)
            {
                StartCoroutine("Shoot");
            }
            PlatformEffector2D platformEffector2D = collision.gameObject.GetComponent<PlatformEffector2D>();
            if (platformEffector2D != null)
            {
                Destroy(platformEffector2D.gameObject);
            }
        }
        if (isJump && collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.Translate(new Vector2(0.5f* stat.direction, 0));
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(KnockbackForceX * stat.direction, KnockbackForceY), ForceMode2D.Impulse);
            var damageInfo = spac.CreatDamageInfo();
            damageInfo.Damage = jumpDamage;
            collision.gameObject.GetComponent<PlayerMinsu>().TakeDamage(damageInfo);
        }
    }

    void Dead()
    {
        GetComponent<Collider2D>().isTrigger = true; ;
    }
}
