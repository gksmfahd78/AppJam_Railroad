using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : ActiveItem, IActive {
    public void ActiveAbility()
    {
        if (stat.canActive || stat.isFirstSetting)
        {
            PlayerMinsu.PlayerInstance.playerStat.speed = PlayerMinsu.PlayerInstance.playerSpec.speed + spec.increaseMoveSpeed;
            GameManagerTaehyun.instance.SetTimeScale(spec.SlowScale);
            StartCoroutine(SlowTimer());
            stat.isFirstSetting = false;
            stat.canActive = false;
        }
        else
        {
            MessageText.Instance.Show(text , PlayerMinsu.PlayerInstance.PlayerPosition(), 1f);
        }
    }



    public IEnumerator SlowTimer()
    {
        yield return new WaitForSeconds(spec.ActiveTime);
        GameManagerTaehyun.instance.SetTimeScale(1f);
        PlayerMinsu.PlayerInstance.playerStat.speed = PlayerMinsu.PlayerInstance.playerSpec.speed;
    }
}
