using UnityEngine;
using System.Collections;

//아이템마다 각각의 프리팹으로 만듬 그래픽은 코드가 아니라 프리팹에서 지정함
public class DropItem : MonoBehaviour
{
    public ItemType ItemType; // 아이템 타입 변수
    public GameObject ItemPrefab; // 아이템 프리팹 > 이 변수에 있는 프리팹을 생성시킴

    //플레이어가 먹으려고 할 때 조작이 필요한지 필요없는지 속성

    //어떤 아이템인지 알려주는 속성


    //아이템은 등장했다가 먹으면 사라짐
    //아이템 등장 = GameObject.Instantiate()
    //아이템 사라짐 = GameObject.Destroy()

    //무기탄약(자동으로 먹기), 회복, 무기, 액티브아이템(직접 사용하는 아이템) => 나머지는 E눌러서

    //타입만 알려주고 타입에 필요한 데이터나 프리팹 등을 들고있다가 플레이어한테 알려줌(O < 이게 좋음 매우 좋음)
    //무기중에 매그넘이다 => 프리팹으로 직접 플레이어한테 쥐어줌
}

public enum ItemType
{
    None,
    Weapon,
    Ammo,
    Heal,
    ActiveItem,
    PassiveItem
}