using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerTaehyun : MonoBehaviour
{
    GameObject mainCamera;

    //어디서나 접근할수 있는 정적 변수
    public static SoundManagerTaehyun instance;

    AudioSource myAudio;

    //오디오클립추가
    public AudioClip doubleBarrel;
    public AudioClip katana;
    public AudioClip knife;
    public AudioClip magnum;
    public AudioClip rifle;
    public AudioClip smg;
    public AudioClip winchester;
    public AudioClip DieSlime;
    public AudioClip MainBG;
    public AudioClip BossBGM;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    //사운드 추가
    //가져갈때는 SoundManager.instance.PlayAudioClip_OneShot(SoundManager.instance.sndEnemyAttack); 요런식으로 사용


    public void PlayAudioClip_OneShot(AudioClip clip)
    {
        myAudio.PlayOneShot(clip);
    }

    public void CancelAudioClip()
    {
        myAudio.Stop();
    }

    public void PlayTheBossBGM() // 보스 등장 타이밍에 호출 시켜주셈
    {
        var cameraSource = mainCamera.GetComponent<AudioSource>();
        cameraSource.clip = BossBGM;
        cameraSource.Play(); 
    }
}
