using UnityEngine;
using System.Collections;

public class itemBoxSpawn : MonoBehaviour
{
    public GameObject Train;
    public float itemBoxSpawnPosY;
    public float TrainsStartPositionX;
    public float TrainsEndPositionX;

    public int Rand;

    public bool boxon = false;

    private Vector2 itemBoxSpawnPosition;

    public GameObject[] Itembox;

    void Start()
    {
        itemBoxSpawnPosY = -1.75f;
        itemBoxSpawnPos();
    }

    void Update()
    {

    }

    void itemBoxSpawnPos()
    {
        Rand = Random.Range(0, 3);

        if(Rand == 0)
        {
            GameObject box = Instantiate(Itembox[0], new Vector2(Train.transform.position.x, itemBoxSpawnPosY), Quaternion.identity);
        }

        else if (Rand == 1)
        {
            GameObject box = Instantiate(Itembox[1], new Vector2(Train.transform.position.x, itemBoxSpawnPosY), Quaternion.identity);
        }

        else if (Rand == 2)
        {
            GameObject box1 = Instantiate(Itembox[0], new Vector2(Train.transform.position.x + 4, itemBoxSpawnPosY), Quaternion.identity);
            GameObject box2 = Instantiate(Itembox[1], new Vector2(Train.transform.position.x - 4, itemBoxSpawnPosY), Quaternion.identity);
        }
    }
}