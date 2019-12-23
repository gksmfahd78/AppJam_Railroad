using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerabg : MonoBehaviour
{

    public float MountainSpeed = 5f;
    public float SkySpeed = 5f;
    public float RailSpeed = 5f;

    public Transform MountainPos;
    public Transform SkyPos;
    public Transform RailPos;

    Vector3 MountainStartPos;
    Vector3 SkyStartPos;
    Vector3 RailStartPos;

    public Transform cameraScroll;
    public GameObject CurrentTrain;

    public float block = 63.7f; 

    void Start()
    {
        MountainStartPos = MountainPos.transform.position;
        SkyStartPos = SkyPos.transform.position;
        RailStartPos = RailPos.transform.position;

        
        cameraScroll = gameObject.GetComponent<MapSpawn>().CameraScroll;
    }

    void Update()
    {
        CurrentTrain = PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().currTrain;
        float NewMountainPos = Mathf.Repeat(Time.time * MountainSpeed, block);
        float NewSkyPos = Mathf.Repeat(Time.time * SkySpeed, block);
        float NewRailPos = Mathf.Repeat(Time.time * RailSpeed, block);

        MountainPos.transform.position = MountainStartPos + Vector3.right * NewMountainPos;
        SkyPos.transform.position = SkyStartPos + Vector3.right * NewSkyPos;
        RailPos.transform.position = RailStartPos + Vector3.right * NewRailPos;

        //if(CurrentTrain.transform.position != cameraScroll.transform.position)
        //{
        //    MountainStartPos = new Vector3(CurrentTrain.transform.position.x - 20, MountainPos.transform.position.y, MountainPos.transform.position.z);
        //    SkyStartPos = new Vector3(CurrentTrain.transform.position.x - 20, SkyPos.transform.position.y, SkyPos.transform.position.z);
        //    RailStartPos = new Vector3(CurrentTrain.transform.position.x - 20, RailPos.transform.position.y, RailPos.transform.position.z);
        //}
    }
}