using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject PauseUI;

    private bool paused = false;

    public GameObject InformationUI;

    private bool pushOn = false;

    public Image weapon_1;
    public Image weapon_2;

    public Image activeItem;


    public Image attack_Fill;
    public Image reload_Fill;
    public Image attackSpeed_Fill;

    private bool Paused
    {
        get
        {
            return paused;
        }

        set
        {
            paused = value;

            if (paused)
            {
                
                PauseUI.SetActive(true);
                GameManagerTaehyun.instance.SetTimeScale(0);
            }

            else
            {
                if (!InformationUI.activeSelf)
                {
                    PauseUI.SetActive(false);
                    GameManagerTaehyun.instance.SetTimeScale(1f);
                }
                else
                {
                    ActiveGameInformation();
                }
            }
        }
    }

    void Start()
    {
        PauseUI.SetActive(false);
        InformationUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!InformationUI.activeSelf)
            {
                Paused = !Paused;
            }
            else
            {
                InformationUI.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        PauseUI.SetActive(false);
        Paused = !Paused;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }

    public void ActiveGameInformation()
    {
        InformationUI.SetActive(true);

        weapon_1.sprite = PlayerMinsu.PlayerInstance.weapons[0].GetComponent<SpriteRenderer>().sprite;
        weapon_1.color = Color.white;
        if (PlayerMinsu.PlayerInstance.weapons[1] != null)
        {
            weapon_2.sprite = PlayerMinsu.PlayerInstance.weapons[1].GetComponent<SpriteRenderer>().sprite;
            weapon_2.color = Color.white;
        }
        if (PlayerMinsu.PlayerInstance.activeItem != null)
        {
            activeItem.sprite = PlayerMinsu.PlayerInstance.activeItem.GetComponent<SpriteRenderer>().sprite;
            activeItem.color = Color.white;
        }


        attack_Fill.fillAmount = PlayerMinsu.PlayerInstance.playerStat.additionalDamage / 10f;
        attackSpeed_Fill.fillAmount = PlayerMinsu.PlayerInstance.playerStat.shotDelayTime / 10f;
        reload_Fill.fillAmount = PlayerMinsu.PlayerInstance.playerStat.reloadDelayTime / 10f;
    }

    public void Back()
    {
        InformationUI.SetActive(false);
    }

    public void Resume_Information()
    {
        InformationUI.SetActive(false);
        Paused = false;
    }
}
