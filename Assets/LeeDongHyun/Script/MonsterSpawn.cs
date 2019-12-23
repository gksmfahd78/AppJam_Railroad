using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject[] Monsters;
    [System.NonSerialized]
    public int monsterSpawnRule;
    public List<GameObject> monsterSpawnPoints;
    public List<GameObject> Completemonster;
    public int monsterCount;
    public int rule;
    public int groundindex;
     
    void Awake()
    {
        
    }

    void Start()
    {
        rule = 0;
        groundindex = GetComponentInChildren<Ground>().groundIndex;

        if (groundindex % 5 == 0)
        {
            rule = groundindex / 5;
        }

        StartCoroutine(TimeDelay());
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerMinsu.PlayerInstance != null && gameObject == PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain)
        {
            if (GetComponent<BigBossSpawn>() && gameObject.GetComponent<MonsterSpawnPoint>().Train == GetComponent<BigBossSpawn>().Train_Self)
            {
               GameManagerTaehyun.instance.MonsterSpawnRule = monsterCount + 2;
            }
            else
            {
               GameManagerTaehyun.instance.MonsterSpawnRule = monsterCount + 1;
            }
        }
    }

    IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(0.4f);
        monsterSpawnRule = gameObject.GetComponent<MonsterSpawnPoint>().MonsterSpawnRule;

        foreach (var monsterPoint in GetComponent<MonsterSpawnPoint>().MonsterSpawnPoints)
        {
            monsterSpawnPoints.Add(monsterPoint);
        }
        monsterSpawn();
    }

    void monsterSpawn()
    {
        int spawnrule = Random.Range(1, 2 + rule);
        monsterCount = spawnrule;

        for (int i = 0; i <= spawnrule; i++)
        {

            int spawnpoint = Random.Range(0, monsterSpawnRule-1);//랜덤 스폰포인트
            int monsterspawn = Random.Range(0, Monsters.Length);//랜덤 몬스터
            if(monsterSpawnPoints[spawnpoint].activeSelf)
            {
                GameObject completemonster = Instantiate(Monsters[monsterspawn], monsterSpawnPoints[spawnpoint].transform.position, Quaternion.identity);
                Completemonster.Add(completemonster);
                Completemonster[i].name = Monsters[monsterspawn].name;
                completemonster.GetComponent<Enemy>().stat.thisTrain = gameObject;

            }
        }
    }
}