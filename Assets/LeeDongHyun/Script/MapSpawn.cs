using UnityEngine;
using System.Collections;
using System.Linq;

public class MapSpawn : MonoBehaviour
{
    public GameObject[] Trains; //기차 
    public GameObject[] MonsterSpwans; //몬스터 스폰 포인트폰  
    public GameObject[] TrainsBackground;
    public GameObject[] MonsterSpwanPrefab; //몬스터 스폰 포인트폰 프리팹
    public GameObject[] TrainPrefab; //기차 프리팹

    /*
     
        TrainPrefab[0] 기차 시작칸
        TrainPrefab[1] 기차 조종칸
        TrainPrefab[2] 기차 고객칸 1
        TrainPrefab[3] 기차 고객칸 2
        TrainPrefab[4] 기차 식당칸
        TrainPrefab[5] 기차 침실칸
        TrainPrefab[6] 기차 화물칸
        TrainPrefab[7] 기차 폭탄

     */

    private int TrainCount; //기차 횟수
    private int MonsterSpwanCount; //몬스터 스폰 포인트 생성 횟수
    private float TrainPrefabX = 0f; //기차 프리팹 x 좌표
    private float BridgePrefabX = 0f;
    private float ScrollSpeed; //스크롤 속도
    private Vector2 CameraAfterPos; // 이후 카메라 좌표

    public Transform CameraScroll; // 카메라 
    public float CameraSpeed = 3f; // 카메라 이동 속도

    Vector3 Target;

    GameObject Floor;
    GameObject FoodFloor;

    public bool foodfloor = true;

    int RandNum = 0; //랜섬 생성

    Vector3 BeforeTrainPrefabPos;
    Vector3 Velocity;

    int cnt = 0;
  
    public GameObject preTrain; // 뒤에 기차
    public GameObject currTrain; // 현재칸
    public GameObject fuTrain; // 다음칸
    public int index_Train = 0; // 기차칸 할당을 위한 인덱스 변수

    public int StuffSpawnRule = 2;
    public float trainsStartPositionX;
    public float trainsEndPositionX;

    //float BeforeTrainPrefabPos = 0f;

    public bool start = true;
    public bool end = true;
    public int acount_Train;

    public GameObject middle_Bridge;


    void Awake()
    {
        Trains = new GameObject[acount_Train];
        _PrefabProduce();
        if(Trains[0].GetComponent<CostomerStuffSpawn>() != null){
            Trains[0].GetComponent<OtherStuffSpawn>().isSpawnedObject = true;
            
        }

        if(Trains[0].GetComponent<CostomerStuffSpawn>() != null)
        {
            Trains[0].GetComponent<CostomerStuffSpawn>().isSpawnedObject = true;
        }
    }

    void LateUpdate()
    {
        Vector3 TargetPos = new Vector3(Trains[PlayerMinsu.PlayerInstance.groundIndex].transform.position.x, Trains[PlayerMinsu.PlayerInstance.groundIndex].transform.position.y, CameraScroll.position.z);
        CameraScroll.position = Vector3.SmoothDamp(CameraScroll.position, TargetPos, ref Velocity, 2f, 2f, 2f);


        if (currTrain.name == fuTrain.name)
        { // 지금 밟고 있는 칸이 다음 칸일 경우 전 칸을 끄고 index++
            preTrain.SetActive(false);
            index_Train++;
        }
        else if (currTrain.name == preTrain.name)
        {//지금 밟고 있는 칸이 전 칸일 경우 다음칸을 끄고 index--
            //fuTrain.SetActive(false);
            index_Train--;
        }

        try
        { // 배열의 길이를 초과하는 인덱스에 접근할경우의 예외를 잡음
            preTrain = Trains[PlayerMinsu.PlayerInstance.groundIndex - 1];
            fuTrain = Trains[PlayerMinsu.PlayerInstance.groundIndex + 1];
        }
        catch (System.IndexOutOfRangeException) // 예외가 발생시 처리하는 코드
        {
            if (PlayerMinsu.PlayerInstance.groundIndex - 1 < 0)
            { // 처음칸을 밟고 있는경우
                preTrain = Trains[PlayerMinsu.PlayerInstance.groundIndex]; //전칸을 처음칸으로 두면서 null은 만들지 않음
            }
            else if (PlayerMinsu.PlayerInstance.groundIndex + 1 == Trains.Length)
            {//마지막칸을 밟고 있는 경우
                fuTrain = Trains[PlayerMinsu.PlayerInstance.groundIndex];
                //다음칸을 마지막칸으로 두면서 null을 만들지 않음
            }

            //다른 조건으로 만들어
                //보스 나오기

        }

        currTrain = Trains[PlayerMinsu.PlayerInstance.groundIndex]; // 현재칸을 넣음

        UIManager.instance.currTrain = currTrain;
        Active_Train(); // 다음칸 , 현재칸 , 전 칸을 setactive true 시킴
        
    }
    
    void Active_Train(){
        fuTrain.SetActive(true);
        //gameObject.GetComponent<MonsterSpawn>().monsterSpawn();
        currTrain.SetActive(true);
        preTrain.SetActive(true);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void _PrefabProduce() // 랜덤 Prefab 및 생성
    {
        for (int i = 0; i < Trains.Length; i++)// 30만큼 반복
        {
            RandNum = Random.Range(2, TrainPrefab.Length - 1); //열차 프리팹배열 에서 랜덤값을 가져옴
            end = i == Trains.Length - 1 ? true : false;
            if (start) // 첫번째 칸은 무조건 시작칸의 인덱스를 가져옴
            {
                RandNum = 0;
                start = false;
            }
            else if (end) // 마지막에 무조건 조종실의 인덱스를 가져옴 
            {
                RandNum = 1;
                end = false;
            }
            else if (i == Trains.Length - 2) // 마지막의 전칸엔 무조건 보스룸의 인덱스를 가져옴
                RandNum = 12;
    
            else
            {
                if (!foodfloor && RandNum == 5)
                {
                    while (RandNum == 5)
                        RandNum = Random.Range(2, TrainPrefab.Length - 1);
                    cnt++;
                }
            }

            GameObject map = Instantiate(TrainPrefab[RandNum], new Vector2(TrainPrefabX, 0), Quaternion.identity);
            Trains[i] = map;
            Trains[i].name = (i+1).ToString();
            Trains[i].GetComponentInChildren<Ground>().groundIndex = i;
            Trains[i].SetActive(false);            

            if(RandNum == 5)
            {
                foodfloor = false;
                cnt = 0;
            }

            if (cnt == 5)
                foodfloor = true;

            TrainPrefabX += 14.2f;
            BridgePrefabX += 7.1f;
            GameObject bridge = Instantiate(middle_Bridge, new Vector2(BridgePrefabX, -1.75f), Quaternion.identity);
            BridgePrefabX += 7.1f;
        }
        //처음 만들어 질 때 실행되는 함수이기 때문에 전칸은 현재칸으로 만듬
        currTrain = Trains[index_Train];
        preTrain = Trains[index_Train];
        fuTrain = Trains[index_Train + 1];

        Active_Train(); // 할당후에 setactive를 true로 만듬
        fuTrain.GetComponent<DoorMove>().frontDoorJudgment = 1;

    }

    public int gizmosX;
    public int gizmosY;
    public int gizmosZ;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(gizmosX, gizmosY, gizmosZ));
    }
}