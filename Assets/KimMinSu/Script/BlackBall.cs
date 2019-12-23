using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBall : ActiveItem, IActive
{
    public void ActiveAbility()
    {
        if (stat.isFirstSetting || stat.canActive)
        {
            stat.canActive = false;
            Collider2D[] enemys = Physics2D.OverlapCircleAll(PlayerMinsu.PlayerInstance.PlayerPosition(), spec.InfluenceRange, layer_Enemy);
            Collider2D[] bullets = Physics2D.OverlapCircleAll(PlayerMinsu.PlayerInstance.PlayerPosition(), spec.InfluenceRange, layer_Bullet);

            if (enemys.Length > 0)
            {
                foreach (var enemy in enemys)
                {
                    
                    float x = PlayerMinsu.PlayerInstance.PlayerPosition().x - enemy.transform.position.x;
                    x = x > 0f ? -spec.pusingForce : spec.pusingForce;
                    enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, spec.pusingForce), ForceMode2D.Impulse);
                }
            }

            if (bullets.Length > 0)
            {
                foreach (var bullet in bullets)
                {
                    Destroy(bullet.gameObject);
                }
            }
            //StartCoroutine(ChargingActive());
            stat.isFirstSetting = false;
        }
        else
        {
            MessageText.Instance.Show(text, PlayerMinsu.PlayerInstance.PlayerPosition(), 1f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(PlayerMinsu.PlayerInstance.PlayerPosition() , spec.InfluenceRange );
    }
}
