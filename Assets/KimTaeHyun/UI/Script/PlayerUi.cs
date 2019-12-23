using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour {

    public Text AmmoText;

    public Image content_Bullet;
    public Image content_Energy;
    public Image content_Explosion;
    public Image content_Shell;


    [Header("Bullet"), SerializeField]
    public float currAmmo_Bullet;
    public float maxAmmo_Bullet;

    [Header("Shell") ,SerializeField]
    public float currAmmo_Shell;
    public float maxAmmo_Shell;

    [Header("Energy"), SerializeField]
    public float currAmmo_Energy;
    public float maxAmmo_Energy;

    [Header("Explsion"), SerializeField]
    public float currAmmo_Explosion;
    public float maxAmmo_Explosion;



    //남은 탄약 텍스트
    [SerializeField]
    private Text Bullet_UI;
    [SerializeField]
    private Text Shotgun_UI;
    [SerializeField]
    private Text Energy_UI;
    [SerializeField]
    private Text Explosion_UI;

	void Update ()
    {

        SetAmmo();
        UpdateAmmoUI();
        AmmoText.text = PlayerMinsu.PlayerInstance.weapon.gun_Stat.ammu_Volume + "/" + PlayerMinsu.PlayerInstance.weapon.gun_Spec.maxAmmu;

    }

    private void UpdateAmmoUI()
    {
        content_Bullet.fillAmount = currAmmo_Bullet / maxAmmo_Bullet;
        content_Energy.fillAmount = currAmmo_Energy / maxAmmo_Energy;
        content_Explosion.fillAmount = currAmmo_Explosion / maxAmmo_Explosion;
        content_Shell.fillAmount = currAmmo_Shell / maxAmmo_Shell;
    }

    public void SetAmmo()
    {
        this.currAmmo_Bullet = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Bullet;
        this.maxAmmo_Bullet = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Bullet;

        this.currAmmo_Energy = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Energy;
        this.maxAmmo_Energy = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Energy;

        this.currAmmo_Explosion = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Explosion;
        this.maxAmmo_Explosion = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Explosion;

        this.currAmmo_Shell = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Shell;
        this.maxAmmo_Shell = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Shell;

        //UI 탄약 텍스트
        Bullet_UI.text = "-" + currAmmo_Bullet;
        Shotgun_UI.text = "-" + currAmmo_Shell;
        Energy_UI.text = "-" + currAmmo_Energy;
        Explosion_UI.text = "-" + currAmmo_Explosion;
    }

}
