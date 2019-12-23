using UnityEngine;
using UnityEngine.SceneManagement;//씬 관리자 관련 코드
using UnityEngine.UI;//UI 관련 코드

//필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject currTrain;

    //싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (GameManager_instance == null)
            {
                GameManager_instance = FindObjectOfType<UIManager>();
            }
            return GameManager_instance;
        }
    }

    private static UIManager GameManager_instance;//싱글톤이 할당될 변수

    //public Text ammoText;//탄약 표시용 텍스트
    // public Text leftEnemy;//남은 적 표시용 텍스트
    //public GameObject Map;//맵 정보 표시할 때 활성화할 UI
    public GameObject GameOver;//게임 오버시 활성화할 UI
    public Text killedByName;
    public Image KilledBySprite;
    public Text playTime;
    private string _playTime;
    //맵 표시용 텍스트
    [SerializeField]
    private Text MapText;

    //탄약 텍스트 갱신
    /*
    public void UpdateAmmoText(Weapon weapon)
    {
        ammoText.text = weapon.gun_Stat.ammo_Valume + "/" + weapon.gun_Spec.maxAmmo;
    }
    
    //남은 적 표시
    public void UpdateleftEnemy(int magEnemy, int remainEnemy)
    {
        
    }
    */
    //맵정보 UI 활성화
    /*public void SetActiveMapUI(bool active)
    {
        Map.SetActive(active);
    }*/
    /*
     currTrain = sdfljkasd;fjasjdflk
        mapinfor(currtrain);
        */


    private void Update()
    {
        Mapinform();
    }

    public void Mapinform()
    {
        if (this.currTrain != null)
        {
            MapText.text = currTrain.name + "/" + PlayerMinsu.PlayerInstance.GetComponent<MapSpawn>().Trains.Length;

        }
        else
        {
            MapText.text = "현재 밟고 있는 기차가 없습니다.";
        }
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameOverUI(bool active, EnemyDamageInfo info)
    {
        GameOver.SetActive(active);
        killedByName.text = info.ShooterName;
        KilledBySprite.sprite = info.ShooterSprite;
        playTime.text = _playTime;
    }

    public void SetPlayTime(string time)
    {
        _playTime = time;
    }

    //게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}