using UnityEngine;
using System.Collections;

public class HealItem : Item
{
    
    public string itemName; // 힐아이템 이름
    public int heal_Amount; // 회복량

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && PlayerMinsu.PlayerInstance.Health != PlayerMinsu.PlayerInstance.playerSpec.maxHealth)
        {
            PlayerMinsu.PlayerInstance.Health += heal_Amount;
            gameObject.SetActive(false);
        }
    }
}
