using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

    public bool isWeaponBox;
    public bool hasUsed;

    public AudioClip openBox;

    Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        hasUsed = false;
    }

    public void OpenItemBox()
    {
        if (hasUsed)
            return;
        else StartCoroutine("Open");
    }

    IEnumerator Open()
    {
        ani.SetTrigger("open");
        SoundManagerTaehyun.instance.PlayAudioClip_OneShot(openBox);
        yield return new WaitForSeconds(0.25f);

        if (isWeaponBox)
        {
            GameManagerTaehyun.instance.CreateDropItem(ItemType.Weapon, this.transform.position);
            hasUsed = true;
        }
        else
        {
            if (Random.value < 0.5f)
            {
                GameManagerTaehyun.instance.CreateDropItem(ItemType.PassiveItem, this.transform.position);
            }
            else
            {
                GameManagerTaehyun.instance.CreateDropItem(ItemType.ActiveItem, this.transform.position);
            }
            hasUsed = true;
        }
    }

}
