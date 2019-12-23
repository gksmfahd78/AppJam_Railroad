using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image content_Health;
    private float currentValue;

    //체력 텍스트
    [SerializeField]
    private Text statText;

    [SerializeField]
    private float lerpSpeed;

    public float currentFill;
    public float MyMaxValue { get; set; }


    //체력과 마나의 현재 값 설정
    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            currentValue = value;
            currentFill = currentValue / MyMaxValue;
            // 체력 표시
            //statText.text = currentValue + " / " + MyMaxValue;
        }
    }

    void Update()
    {
        // 체력 or 마나의 값이 변경될 경우
        if (currentFill != content_Health.fillAmount)
        {
            // Mathf.Lerp(시작값, 끝값, 기준) => 부드럽게 값을 변경 가능
            content_Health.fillAmount = Mathf.Lerp(content_Health.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }
    // 체력과 마나 값을 셋팅(현재 값, 최대값)
    public void Initialized(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
