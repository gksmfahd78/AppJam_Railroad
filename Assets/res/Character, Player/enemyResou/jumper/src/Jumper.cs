using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Enemy {

    Rigidbody2D rigidbody;
    public bool isJump;
    public float jumpForceXMin;
    public float jumpForceXMax;
    public float jumpForceY;
    public float jumpChargeRate;

    void Start () {
        GetComponentInChildren<EnemyGun>().GetSpac(this);
        StartCoroutine("NonDetectAct");
        rigidbody = GetComponent<Rigidbody2D>();
        this.transform.GetChild(0).GetComponent<EnemyGun>().isChasing = true;
        ani.SetFloat("Blend", 0f);
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
            StopCoroutine("NonDetectAct");
            stat.isMoveing = false;
            if (!isJump)
            {
                ani.SetTrigger("Jump");
            }
            GetComponentInChildren<EnemyGun>().targetPlayer = PlayerMinsu.PlayerInstance.gameObject;
            GetComponentInChildren<EnemyGun>().PlayerChasing();
            // 자식의 EnemyGun 스크립트의 targetPlayer를 지정해줌
            Chasing();

            if (isJump && Mathf.Abs(rigidbody.velocity.y) < 0.2f && !stat.isUnderAttack)
            {
                stat.isUnderAttack = true;
                StartCoroutine("Shoot");
            }
        }
        if (stat.isMoveing)
        {
            ani.SetFloat("Blend", 1f);
            CliffCheck();
            transform.Translate(spac.moveSpeed * Time.deltaTime * stat.direction, 0, 0);
        }
        else
        {
            ani.SetFloat("Blend", 0f);
        }
    }

    void jumpReady()
    {
        jumpForceY = Random.Range(5f, 10f);
        ani.SetFloat("JumpSpeed", jumpChargeRate / jumpForceY);
        Debug.Log(jumpChargeRate / jumpForceY);
    }

    void Jump()
    {
        isJump = true;
        rigidbody.AddForce(new Vector2(Random.Range(jumpForceXMin,jumpForceXMax) * stat.direction,jumpForceY), ForceMode2D.Impulse);
        stat.isMoveing = false;
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            this.transform.GetChild(0).GetComponent<EnemyGun>().ShootBullet();
            yield return new WaitForSeconds(0.1f);
        }
        stat.isUnderAttack = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isJump && collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            isJump = false;
            ani.SetTrigger("Lending");
        }
    }
}
