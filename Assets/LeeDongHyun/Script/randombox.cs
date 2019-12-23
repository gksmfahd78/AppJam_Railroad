using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randombox : MonoBehaviour
{
    private float BoxY;
    public GameObject Train_Self;
    public float TrainsStartPositionX;
    public float TrainsEndPositionX;
    public List<GameObject> Boxs;

    public int rand1;
    public int rand2;
    public float rand3;
    public float randx;
    public List<float> randxs;
    const double eps = 2;

    /*
     0 노란색박스
     1 파란색박스
     */

    void Start()
    {
        BoxY = -1.75f;
        TrainsStartPositionX = Train_Self.transform.position.x - 4f;
        TrainsEndPositionX = Train_Self.transform.position.x + 4f;
        preducebox();
    }

    void Update()
    {
        
    }

    void preducebox()
    {
        rand1 = Random.Range(0, 2);
        rand2 = Random.Range(0, 1);
        rand3 = Random.Range(TrainsStartPositionX + 1, TrainsEndPositionX - 1);

        if (rand1 == 1)
        {
            GameObject box = Instantiate(Boxs[rand2], new Vector2(Train_Self.transform.position.x, BoxY), Quaternion.identity);
        }

        else if (rand1 == 2) 
        {
           

            for (int i = 0; i <= 1; i++)
            {
                Object box = Instantiate(Boxs[i], new Vector2(rand3, BoxY), Quaternion.identity);
            }
        }
    }
}
