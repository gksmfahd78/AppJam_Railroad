using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageText : MonoBehaviour
{



    private float _timer;
    public static MessageText Instance;

    private void Awake()//모든 스타트보다 먼저 돌려주는 코드
    {
        Instance = this;//this는 메세지텍스트 개체 
    }

    [SerializeField]
    private Text _message;//프라이빗은 언더바랑 소문자

    void Start()
    {
        Hide();
        //1글씨보여주기
        //2글씨사라지게하기 2번은 1번이후에 자동으로 되야함
        //3글씨안보이게하기 
    }
    public void Hide()
    {
        _message.enabled = false;
    }
    public void Show(string messageString, Vector2 worldPosition, float duration = 2)//로컬 변수는 언더바 없이 소문자로 시작 두단어 이상일 때는 2번째꺼 대문자로 구분 맨끝의 인자는 값을 놓을수있음.
    {
        _message.enabled = true;// 표시되유
        _message.text = messageString;// 내용들어가유
        _message.rectTransform.anchoredPosition = GetCanvasPosition(worldPosition, UIManager.instance.GetComponent<RectTransform>());// UI의 포지션 위치들어가유
        _timer = duration;
    }

    public Vector2 GetCanvasPosition(Vector2 worldPosition, RectTransform canvasRectTransform)
    {
        var positionInCamera = Camera.main.WorldToViewportPoint(new Vector2(worldPosition.x , worldPosition.y ));    // 카메라 기준으로 (0∼1,0∼1)의 값을 제공
        var positionInCanvas = new Vector2((positionInCamera.x - 0.5f) * canvasRectTransform.sizeDelta.x, (positionInCamera.y - 0.5f) * canvasRectTransform.sizeDelta.y);    // 캔버스 크기에 맞춰 크기를 곱해준다. 캔버스는 중심값이 0, 0이므로 x,y 각각 0.5씩 뺀 후 계산한다
        return positionInCanvas;
    }
    private void Update()//글씨가 사라진다구?~~ 연출맨
    {
        
        if (_message.enabled)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                Hide();
            }
        }

        //다른 UI가 실행됬을 때 숨기기 실행
    }
}