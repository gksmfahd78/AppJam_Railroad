using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {
    [System.NonSerialized]
    public Text _text;
    float timer = 0;
    int min = 0;

    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMinsu.PlayerInstance.isDeath)
        {
            _Timer();
            UIManager.instance.SetPlayTime(_text.text);
        }
    }

    void _Timer()
    {
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            min++;
            timer -= 60;
        }

        _text.text = string.Format("{0:D2} : {1:D2}", min, (int)timer);
    }
}
