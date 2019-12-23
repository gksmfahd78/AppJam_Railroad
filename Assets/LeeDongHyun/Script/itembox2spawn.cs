using UnityEngine;
using System.Collections;

public class itembox2spawn : MonoBehaviour
{
    public GameObject Train;
    public GameObject Itembox;
    public float itemBoxSpawnPosY;

    void Start()
    {
        itemBoxSpawnPosY = -1.75f;
        for (int i = 0; i < 3; i++)
        {
            GameObject box = Instantiate(Itembox, new Vector2(Train.transform.position.x + 4, itemBoxSpawnPosY), Quaternion.identity);
        }
    }
}
