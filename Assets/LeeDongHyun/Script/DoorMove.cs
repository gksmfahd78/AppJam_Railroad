using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour 
{
    
    public int frontDoorJudgment;
    /*
     1 올라가는거
     0 멈춤
     -1 내려가는거
     */
    public int backDoorJudgment;
    /*
    1 올라가는거
    0 멈춤
    -1 내려가는거
    */
    public GameObject frontDoor;
    public GameObject backDoor;

    Vector3 frontDoorEndPos;
    Vector3 backDoorEndPos;

    Vector3 frontDoorStartPos;
    Vector3 backDoorStartPos;

    public float doorMoveSpeed;

    public float max = 5f;

    const double eps = 0.01;

    void Awake()
    {
        frontDoorStartPos = frontDoor.transform.position;
        backDoorStartPos = backDoor.transform.position;
        frontDoorEndPos = new Vector3(frontDoor.transform.position.x, 0, frontDoor.transform.position.z);
        backDoorEndPos = new Vector3(backDoor.transform.position.x, 0, frontDoor.transform.position.z);
    }

    void Start () 
    {
        doorMoveSpeed = 10f;
       // frontDoorJudgment = 1;
       //backDoorJudgment = 1;
    }
	
	void Update () 
    {
        if (frontDoorJudgment == 1)
            frontDoor.transform.position = Vector3.Lerp(frontDoor.transform.position, frontDoorEndPos, Time.deltaTime * doorMoveSpeed);
        else if (frontDoorJudgment == -1) 
            frontDoor.transform.position = Vector3.Lerp(frontDoor.transform.position, frontDoorStartPos, Time.deltaTime * doorMoveSpeed);

        if (backDoorJudgment == 1)
            backDoor.transform.position = Vector3.Lerp(backDoor.transform.position, backDoorEndPos, Time.deltaTime * doorMoveSpeed);
        else if (backDoorJudgment == -1)
            backDoor.transform.position = Vector3.Lerp(backDoor.transform.position, backDoorStartPos, Time.deltaTime * doorMoveSpeed);
	}
}
