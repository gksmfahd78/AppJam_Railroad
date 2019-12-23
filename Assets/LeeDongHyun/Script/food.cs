using UnityEngine;
using System.Collections;

public class food : MonoBehaviour
{

    public GameObject Train;
    public GameObject Itembox;
    public float itemBoxSpawnPosY;
    public int Rand;

    void Start()
    {
        itemBoxSpawnPosY = -1.15f;
        Rand = Random.Range(1, 10);

        for (int i = 0; i < Rand; i++)
        {
            int RandX = Random.Range(1,7);
            GameManagerTaehyun.instance.CreateDropItem(ItemType.Heal, new Vector2(Train.transform.position.x + RandX, itemBoxSpawnPosY));
        }
    }

}
