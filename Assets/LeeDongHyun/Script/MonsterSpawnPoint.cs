using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour {
    public GameObject Train; // 자기 자신 담을 변수
    public GameObject[] Trains;
    public List<GameObject> MonsterSpawnPoints;
    public GameObject MonsterSpawnPrefab; //몬스터 스폰 프리팹 담을 변수
    public float TrainsStartPositionX;
    public float TrainsEndPositionX;
    public List<float> TrainsRandNumX;
    public float TrainsNumY;
    private bool check = false;
    public int Stage = 0;
    public int MonsterSpawnRule;

    const double eps = 2;

	void Awake () {

    }

    void Start()
    {
        MonsterSpawnRule = 10;
        TrainsRandNumX = new List<float>();
        MonsterSpawnPoints = new List<GameObject>();
        TrainsStartPositionX = Train.transform.position.x - (13 / 2);
        TrainsEndPositionX = Train.transform.position.x + (13 / 2);
        spawn();

    }

    void Update () {
        if (PlayerMinsu.PlayerInstance != null && PlayerMinsu.PlayerInstance.gameObject.GetComponent<MapSpawn>().preTrain.name == Train.name)
        {
            for (int i = 0; i < MonsterSpawnPoints.Count; i++)
            {
                Destroy(MonsterSpawnPoints[i]);
            }
        }
	}

    void spawn()
    {
        TrainsNumY = Random.Range(-1f, 1.8f);
        //if (check) return;
        for (int i = 0; i < MonsterSpawnRule; i++)
        {
            TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX));
            for (int j = 0; j <= i; j++)
            {
                if (Mathf.Abs(TrainsRandNumX[j] - TrainsRandNumX[i]) < eps)
                    TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX));
            }
         
            Vector2 SpawnPointPos = new Vector2(TrainsRandNumX[i], TrainsNumY);
            GameObject SpawnPoint = Instantiate(MonsterSpawnPrefab, SpawnPointPos, Quaternion.identity);
            MonsterSpawnPoints.Add(SpawnPoint);
            MonsterSpawnPoints[i].name = MonsterSpawnPrefab.name + i;
            Debug.Log("생성 + " + i);
        }

        
        //check = true;
    }
}
