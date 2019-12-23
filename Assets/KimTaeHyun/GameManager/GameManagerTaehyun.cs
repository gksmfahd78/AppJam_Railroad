using UnityEngine;
using UnityEngine.SceneManagement;
// 게임매니저
public class GameManagerTaehyun : MonoBehaviour
{
    public DropItem[] DropItemPrefabs_Weapon; // 칸에 맞춰서 프리팹 넣기
    public DropItem[] DropItemPrefabs_Ammo; // 칸에 맞춰서 프리팹 넣기
    public DropItem[] DropItemPrefabs_Heal; // 칸에 맞춰서 프리팹 넣기
    public DropItem[] DropItemPrefabs_Active; // 칸에 맞춰서 프리팹 넣기
    public DropItem[] DropItemPrefabs_Passive; // 칸에 맞춰서 프리팹 넣기

    //싱글톤 접근용 프로퍼티
    public static GameManagerTaehyun instance
    {
        get
        {
            //만약 싱글톤 변수에 아직 오브제트가 할당되지 않았다면
            if (GameManager_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                GameManager_instance = FindObjectOfType<GameManagerTaehyun>();
            }
            //싱글톤 오브젝트를 반환
            return GameManager_instance;
        }
    }

    private static GameManagerTaehyun GameManager_instance;//싱글톤이 할당될 static 변수
    
    public bool ClearResult { get; private set; }//클리어 상태

    public bool isGameover { get; private set; }//게임 오버 상태
    public bool isClear;
    private int monsterSpawnRule;
    public int MonsterSpawnRule
    {
        get
        {
            return monsterSpawnRule;
        }
        set
        {
            monsterSpawnRule = value;
            if (monsterSpawnRule <= 0)
            {
                PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<DoorMove>().frontDoorJudgment = 1;
                PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<DoorMove>().backDoorJudgment = 1;
                PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().fuTrain.GetComponent<DoorMove>().frontDoorJudgment = 1;
                if (isClear && PlayerMinsu.PlayerInstance.activeItem != null)
                {
                    PlayerMinsu.PlayerInstance.activeItem.stat.clearedTrain++;
                    isClear = false;
                }
            }
            else
            {
                PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<DoorMove>().frontDoorJudgment = -1;
                PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain.GetComponent<DoorMove>().backDoorJudgment = -1;
                isClear = true;
            }
        }
    }

    private void Awake()
    {
        isClear = true;
        SetTimeScale(1f);
        //씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            //자신을 파괴
            Destroy(gameObject);
        }
    }
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }


    public void MainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void EndGame(EnemyDamageInfo info)
    {
        //게임 오버 상태를 참으로 변경
        isGameover = true;
        //게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameOverUI(true, info);
    }
    

    public void CreateDropItem(ItemType dropItemType, GameObject targetPrefab, Vector2 position)
    {
        switch (dropItemType)
        {
            case ItemType.Weapon:
                foreach(var dropItemPrefab in DropItemPrefabs_Weapon)
                {
                    if(dropItemPrefab.ItemPrefab == targetPrefab)
                    {
                        GameObject.Instantiate(dropItemPrefab.gameObject, position, Quaternion.identity);
                    }
                }
                break;

            case ItemType.Ammo:
                
                break;

            case ItemType.Heal:
                
                break;

            case ItemType.ActiveItem:

                break;

            case ItemType.PassiveItem:
                
                break;
        }
    }

    public void CreateDropItem(ItemType dropItemType, Vector2 position)
    {
        switch (dropItemType)
        {
            case ItemType.Weapon:
                GameObject.Instantiate(DropItemPrefabs_Weapon[Random.Range(0, DropItemPrefabs_Weapon.Length)].gameObject, position, Quaternion.identity);
                break;

            case ItemType.Ammo:
                if(Random.value > 0.6f)
                {
                    GameObject.Instantiate(DropItemPrefabs_Ammo[(int)PlayerMinsu.PlayerInstance.weapon.gun_Spec.ammoType - 1].gameObject, position, Quaternion.identity);
                }
                else
                {
                    GameObject.Instantiate(DropItemPrefabs_Ammo[Random.Range(0, DropItemPrefabs_Ammo.Length)].gameObject, position, Quaternion.identity);
                }
                break;

            case ItemType.Heal:
                GameObject.Instantiate(DropItemPrefabs_Heal[Random.Range(0, DropItemPrefabs_Heal.Length)].gameObject, position, Quaternion.identity);
                break;

            case ItemType.ActiveItem:
                GameObject.Instantiate(DropItemPrefabs_Active[Random.Range(0, DropItemPrefabs_Active.Length)].gameObject, position, Quaternion.identity);
                break;

            case ItemType.PassiveItem:
                GameObject.Instantiate(DropItemPrefabs_Passive[Random.Range(0, DropItemPrefabs_Passive.Length)].gameObject, position, Quaternion.identity);
                break;
        }
        //열거형 하나마다 프리팹을 지정해놓고 꺼내씀
        //채움
    }
}



/*
public enum DropItemType
{
    None = 0,
    Magnum = 1, 
    Pistol = 2,
    Heal = 3, 
    //Heal2 = 11 이런식으로
    //Granade = 20 이런식
    //윤호가 종류를 만들자
}*/
