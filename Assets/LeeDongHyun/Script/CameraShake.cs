using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Vector3 originPos;

    [Header("Shake")]
    public float time;
    public float amount;
    public float _timer;

    void Start()
    {
        originPos = transform.localPosition;
        Shake();
    }

    void Update()
    {
        if(_timer < time)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * amount + originPos;
            _timer += Time.deltaTime;
            if(_timer >= time)
            {
                transform.localPosition = originPos;
            }
        }
    }

    public void Shake()
    {
        float _timer = 0;
        //transform.localPosition = originPos;
    } 
}