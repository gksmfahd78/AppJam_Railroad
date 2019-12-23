using UnityEngine;
using System.Collections;

public class Enemys_Count : MonoBehaviour
{
    public GameObject[] enemys;


    // Use this for initialization
    void Start()
    {
        //UI = text => 0; 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Count()
    {
        // 적 스폰을 실행
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //UI = text => enemys.length; 
    }

}
