using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherStuffSpawn : MonoBehaviour//사물생성하는 스크립트 - 고객층 아닌 나머지 관련
{
    public GameObject Train_Self;// 자기 프리팹을 담을 변수
    private List<GameObject> StuffSpawnPoints; //사물 스폰 포인트<게임오브젝>형 리스트
    public GameObject[] StuffPrefab; //사물 프리팹 
    private int StuffSpawnRule;//사물 스폰 개수
    private float TrainsStartPositionX; // 기차의 시작 포시션 x축
    private float TrainsEndPositionX;// 기차의 마지막 포시션 x축
    private List<float> TrainsRandNumX; // 기자의 랜덤 x 축

    const double eps = 2; // 포지션값이 겹치는 것을 방지하는 더블형 상수
    float TrainsNumY = 0;


    public bool isSpawnedObject = false;
    // Use this for initialization


    void Start()
    {
        StuffSpawnPoints = new List<GameObject>();
        TrainsRandNumX = new List<float>();
        TrainsStartPositionX = Train_Self.transform.position.x - 4f;
        TrainsEndPositionX = Train_Self.transform.position.x + 4f;
        if (!isSpawnedObject){
            pointSpawn();
        }
    }

    void Awake()
    {
        Train_Self = this.gameObject;
        StuffSpawnRule = Random.Range(1, 5); // 사물 개수를 랜덤으로 뽑아냄
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Player.PlayerInstance.gameObject.GetComponent<MapSpawn>().preTrain.name == Train_Self.name && Player.PlayerInstance != null)
       /* if (Player.PlayerInstance != null && Player.PlayerInstance.gameObject.GetComponent<MapSpawn>().currTrain.name == Train_Self.name)
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

    void pointSpawn()
    {
        for (int i = 0; i < StuffSpawnRule-1; i++)
        {
            TrainsNumY = Random.Range(-1, 1);
            TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX)); // 리스트는 배열의 크기를 지정해주지 않았으니까 인덱스로 접근 할 수 없음 때문에 Add함수를 이용해

            for (int j = 0; j <= i; j++)
            {
                if (Mathf.Abs(TrainsRandNumX[j] - TrainsRandNumX[i]) < eps)
                    TrainsRandNumX.Add(Random.Range(TrainsStartPositionX, TrainsEndPositionX));
            }
            Vector3 SpawnPointPos = new Vector3(TrainsRandNumX[i], TrainsNumY, -1);
            GameObject SpawnPoint = Instantiate(StuffPrefab[i], SpawnPointPos, Quaternion.identity);
            StuffSpawnPoints.Add(SpawnPoint);
            StuffSpawnPoints[i].name = StuffPrefab[i].name + i;
        }
        isSpawnedObject = true;
    }
}
