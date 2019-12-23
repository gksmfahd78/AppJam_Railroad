using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostomerStuffSpawn : MonoBehaviour //사물 생성하는 스크립트 - 고객층 관련
{
    //public GameObject[] Trainss;
    public GameObject Train_Self;
    public List<GameObject> StuffSpawnPoints;
    public List<GameObject> StuffPrefab;
    private int StuffSpawnRule;
    public float TrainsStartPositionX;
    public float TrainsEndPositionX;
    public List<float> TrainsRandNumX;
    public float TrainsNumY;
    private int randNum;
    const double eps = 2;

    public bool isSpawnedObject = false;

    void Awake()
    {
        StuffSpawnRule = Random.Range(1, 5);
    }

    void Start()
    {
        //Trainss = gameObject.GetComponent<MapSpawn>().Trains;
        TrainsNumY = 0.18f;
        StuffSpawnPoints = new List<GameObject>();
        TrainsStartPositionX = Train_Self.transform.position.x - 4f;
        TrainsEndPositionX = Train_Self.transform.position.x + 4f;
        if(!isSpawnedObject){
            Spawn();
        }
        
    }
    // Use this for initialization


    // Update is called once per frame
    void Update () 
    {
        /*if (Player.PlayerInstance != null && Player.PlayerInstance.gameObject.GetComponent<MapSpawn>().currTrain.name == Train_Self.name)
        {
            for (int i = 0; i < StuffSpawnPoints.Count; i++)
            {
                StuffSpawnPoints[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < StuffSpawnPoints.Count; i++)
            {
                StuffSpawnPoints[i].SetActive(false);
            }
        }*/
	}

    void Spawn()
    {
        
        for (int i = 0; i < StuffSpawnRule; i++)
        {
            TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX));
            randNum = Random.Range(0, 2);

            for (int j = 0; j <= i; j++)
            {
                if (Mathf.Abs(TrainsRandNumX[j] - TrainsRandNumX[i]) < eps)
                    TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX));
                
            }
            Vector2 SpawnPointPos = new Vector2(TrainsRandNumX[i], TrainsNumY);
            GameObject SpawnPoint = Instantiate(StuffPrefab[randNum], SpawnPointPos, Quaternion.identity);
            StuffSpawnPoints.Add(SpawnPoint);
            StuffSpawnPoints[i].name = StuffPrefab[randNum].name + i;
        }
        isSpawnedObject = true;
    }
}
