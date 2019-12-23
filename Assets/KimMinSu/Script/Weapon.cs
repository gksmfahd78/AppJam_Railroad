using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

public enum Gun_State
{
    NONE,
    FIRE,
    REROAD,
}

public enum Gun_Kinds
{
    NONE,
    GUNS,
    EXPLOSION,
    SEEKER,
    SHOTGUN,
}

public enum Ammunition_Kinds
{
    NONE,
    BULLET,
    SHELL,
    ENERGY,
    EXPLOSIVE,
}

public enum Weapon_Kinds
{
    NONE,
    GUN,
    MELEE,
    GRANADE,
}

[System.Serializable]
public class Gun_Stat
{
    public int ammu_Volume; // 탄약량
    public Gun_State Gun_State; // 무기의 현재 상태
    public float attackDamage; // 공격력
    public float attackDelay; // 공격딜레이
    public float reloadSpeed; // 재장전 스피드

    public float shotSpeed; // 발사속도
    public float accuracy;//명중률
    public float explosionRadius;//폭발범위
    public bool canPenetrate;//관통가능여부
    public bool isHeated;// 가열 여부
    public float explosionDamage;

    public bool isPlayedCancelSound;//취소 사운드를 플레이 했는가?
}

[System.Serializable]
public class Gun_Spec
{
    public float heatingTime; //가열시간 - 미니건 적용
    public float explosionRadius;//폭발범위
    public bool canPenetrate;//관통가능여부
    public float accuracy;//명중률
    public float shotSpeed;// 발사속도
    public int quantity;// 1회당 총알 발사량
    public float attackDamage; // 공격력
    public float attackDelay; // 공격딜레이
    public float reloadSpeed;// 재장전 스피드
    public int maxAmmu; // 최대 탄약량
    public Ammunition_Kinds ammoType; // 탄약 종류
    public string gunName; // 무기 이름
    public Gun_Kinds gunType; // 총 타입
    public GameObject ammu_ForSpawn; // 스폰을 위한 탄약 오브젝트
    public float explosionDamage;//폭발데미지
    public float decrease_MoveSpeed; //무기 장착시에 이동속도 감소량;
    public Vector2 pivotOffset; // 센터에서 얼마나 떨어져서 위치시킬지 오프셋변수
    public bool canOnePerOneAmmo;// 한발씩 충전하는 총인지 구분하는 변수
    public bool canAutoFire; // 자동총인지 자동총이 아닌지 구분하는 변순
    public AudioClip shootSound; // 쏘는 사운드
    public AudioClip heatingSound; // 히팅 사운드
    public AudioClip cancelShootSound; // 공격이 취소되거나 공격이 끝났을 때 사용하는 사운드
}


public enum Knife_State
{
    NONE,
    ATTAKE,
}

public enum Attack_Form
{
    NONE,
    SWING,
    STING,
    STAY,
}

public enum Knife_Kinds
{
    NONE,
    KNIFE,
    HAMMER,
}

[System.Serializable]
public class Knife_Spec
{
    public Knife_Kinds knifeKind; // 칼의 종류
    public float damage; // 공격력
    public float attackSpeed; // 공격 속도
    public Attack_Form attack_Form; // 공격 형태
    public Vector2 pivotOffset;
    public string knifeName;
    public Collider2D attackRange;
    public AudioClip[] audioClips_Attack; // 공격 소리
}

[System.Serializable]
public class Knife_Stat
{
    public Knife_State knife_State; // 근접무기의 상태
    public float damage; // 공격력
    public float attackSpeed; //공격속도
    public Collider2D attackRange; //공격범위
    public bool isFirstAttak;
    public bool reflectable; // 총알 반사 가능한가
}

public class Weapon : MonoBehaviour{

    
    [Header("Knife_Spec")]
    public Knife_Spec knife_Spec;

    [Header("Knife_Stat")]
    public Knife_Stat knife_Stat;

    [Header("GUN_SPEC")]
    public Gun_Spec gun_Spec;

    [Header("GUN_STAT")]
    public Gun_Stat gun_Stat;

    [Header("State")]
    public bool direction_Weapon; // 무기의 방향을 나타내는 bool 형 변수

    [Header("USING_WEAPON")]
    public bool isUsedWeapon = false;

    [System.NonSerialized]
    public GameObject weaponPrefab;

    public Weapon_Kinds weaponKind; //무기 종류
    [System.NonSerialized]
    public SpriteRenderer WeaponCenter;

    // 원거리 무기
    [Header("발싸 위치")]
    public Transform fire_Forward;

    private Animator _weapon_Anit;
    private float _heating_Timer = 0f; // 가열 시간을 잴 타이머
    private int _currAmmo;//한발씩 충전할 때 남은 장탄수를 표현할 변수
    private bool _isPlayHeatingSound; // 가열사운드가 플레이 되고 있는지 확인할 변수
    private ParticleSystem shootEffect;
    private Light shootLight;

    public LayerMask layer_Enemy;
    public LayerMask layer_Bullet;



    public int Ammo_property
    {
        get
        {
            return gun_Stat.ammu_Volume;
        }
        set
        {
            gun_Stat.ammu_Volume = Mathf.Clamp(value, 0, gun_Spec.maxAmmu);
            if(gun_Stat.ammu_Volume <= 0 && gun_Stat.Gun_State == Gun_State.NONE)
            {
                if(gun_Spec.cancelShootSound != null)
                {
                    SoundManagerTaehyun.instance.PlayAudioClip_OneShot(gun_Spec.cancelShootSound);
                    gun_Stat.isPlayedCancelSound = true;
                }
                if (PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo > 0)
                {
                    Reload(gun_Stat.reloadSpeed);
                }
            }
        }
    }
    
    public float AttackForce_Gun
    {
        get
        {
            return gun_Stat.attackDamage;
        }

        set
        {
            float atkFc = value;
            atkFc = atkFc <= 0f ? 1f : atkFc;
            gun_Stat.attackDamage = gun_Spec.attackDamage * atkFc;
        }
    }

    public float AttackDelay_Gun
    {
        get
        {
            return gun_Stat.attackDelay;
        }

        set
        {

            float atkSp = value;
            atkSp = (atkSp / 100f) <= 0f ? gun_Spec.attackDelay : (atkSp / 100f) * atkSp;
            gun_Stat.attackDelay = gun_Spec.attackDelay;
        }
    }


    public float ReloadSpeed
    {
        get
        {
            return gun_Stat.reloadSpeed;
        }

        set
        {
            float reloadSp = value;
            reloadSp = (reloadSp / 100f) <= 0f ? gun_Spec.reloadSpeed : (reloadSp / 100f) * gun_Spec.reloadSpeed* reloadSp;
            gun_Stat.reloadSpeed = reloadSp;
        }
    }


    public float AttackForce_Knife
    {
        get
        {
            return knife_Stat.damage;
        }

        set
        {
            float atkFc = value;
            atkFc = atkFc <= 0f ? 1f : atkFc;
            knife_Stat.damage = knife_Spec.damage * atkFc;
        }
    }

    public float AttackSpeed_Knife
    {
        get
        {
            return knife_Stat.attackSpeed;
        }

        set
        {

            float atkSp = value;
            atkSp = (atkSp / 100f) <= 0f ? knife_Spec.attackSpeed : (atkSp / 100f) * atkSp;
            knife_Stat.attackSpeed = knife_Spec.attackSpeed;
        }
    }

    public void Awake()
    {
        WeaponCenter = GameObject.FindGameObjectWithTag("WeaponCenter").GetComponent<SpriteRenderer>();
        _weapon_Anit = GetComponent<Animator>();
        shootEffect = GetComponentInChildren<ParticleSystem>();
        shootLight = GetComponentInChildren<Light>();
   
        if (shootLight != null)
        {
            shootLight.enabled = false;
        }
        switch (weaponKind)
        {
            case Weapon_Kinds.GUN:
                gun_Stat.attackDamage = gun_Spec.attackDamage;
                gun_Stat.attackDelay = gun_Spec.attackDelay;
                gun_Stat.reloadSpeed = gun_Spec.reloadSpeed;
                gun_Stat.accuracy = gun_Spec.accuracy;
                gun_Stat.canPenetrate = gun_Spec.canPenetrate;
                gun_Stat.explosionRadius = gun_Spec.explosionRadius;
                gun_Stat.shotSpeed = gun_Spec.shotSpeed;
                gun_Stat.explosionDamage = gun_Spec.explosionDamage;
                break;
            case Weapon_Kinds.MELEE:
                knife_Spec.attackRange = GetComponent<Collider2D>();
                knife_Stat.attackRange = knife_Spec.attackRange;
                knife_Stat.attackRange.enabled = false;
                knife_Stat.attackSpeed = knife_Spec.attackSpeed;
                knife_Stat.damage = knife_Spec.damage;
                knife_Stat.isFirstAttak = true;

                break;
            case Weapon_Kinds.GRANADE:
                break;
        }
    }

    protected void OnEnable()
    {
        gun_Stat.Gun_State = Gun_State.NONE;
        knife_Stat.knife_State = Knife_State.NONE;


        _isPlayHeatingSound = false;
    }

    void UpdateMaxAmmo()
    {
        switch (gun_Spec.ammoType)
        {
            case Ammunition_Kinds.BULLET:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Bullet;
                PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Bullet;
                break;
            case Ammunition_Kinds.ENERGY:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Energy;
                PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Energy;
                break;
            case Ammunition_Kinds.EXPLOSIVE:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Explosion;
                PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Explosion;
                break;
            case Ammunition_Kinds.SHELL:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Shell;
                PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo = PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Shell;
                break;
        }
    }
    public void Update()
    {
        if (isUsedWeapon)
        {
            UpdateMaxAmmo();
            PlayerMinsu.PlayerInstance.playerStat.speed = PlayerMinsu.PlayerInstance.playerSpec.speed - gun_Spec.decrease_MoveSpeed;
            FllowingMouse();
            switch (weaponKind)
            {
                case Weapon_Kinds.GUN:
                    Gun_Active();
                    break;

                case Weapon_Kinds.MELEE:
                    Knife_Active();
                    break;

                case Weapon_Kinds.GRANADE:
                    break;
            }
            
        }
    }

    public void Gun_Active()
    {
        
        if (Input.GetKeyDown(KeyCode.R) && 
            gun_Stat.Gun_State == Gun_State.NONE && 
            Ammo_property < gun_Spec.maxAmmu)
        {
            if(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo <= 0)
            {
                MessageText.Instance.Show("사용 가능한 총알이 부족합니다.",PlayerMinsu.PlayerInstance.PlayerPosition());
            }
            else
            {

                Reload(ReloadSpeed);
            }
        }


        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (shootLight != null)
            {
                shootLight.enabled = false;
            }
            gun_Stat.isHeated = false;
            _heating_Timer = 0f;
            _isPlayHeatingSound = false;
            if(gun_Spec.cancelShootSound != null && !gun_Stat.isPlayedCancelSound)
            {
                SoundManagerTaehyun.instance.CancelAudioClip();
                SoundManagerTaehyun.instance.PlayAudioClip_OneShot(gun_Spec.cancelShootSound);
            }
        } 

        if (gun_Spec.canAutoFire)
        {

            if (Input.GetKey(KeyCode.Mouse0)&& gun_Stat.Gun_State == Gun_State.NONE && Ammo_property > 0)
            {
                if (_weapon_Anit != null)
                {
                    _weapon_Anit.SetBool("fire", true);
                }

                gun_Stat.isPlayedCancelSound = false;
                if (gun_Stat.isHeated)
                {
                    if (shootLight != null)
                    {
                        shootLight.enabled = true;
                    }
                    Ammo_property--;
                    Debug.Log(string.Format("발싸!  남은 총알 : {0}", Ammo_property));
                    if (gun_Spec.shootSound != null)
                    {
                        SoundManagerTaehyun.instance.PlayAudioClip_OneShot(gun_Spec.shootSound);
                    }
                    switch (gun_Spec.gunType)
                    {
                        case Gun_Kinds.GUNS:
                            ShootAmmo();
                            break;

                        case Gun_Kinds.SHOTGUN:
                            ShootShotGun(gun_Spec.quantity);
                            break;

                        case Gun_Kinds.SEEKER:
                            ShootAmmo();
                            break;

                        case Gun_Kinds.EXPLOSION:
                            ShootAmmo();
                            break;
                    }
                    if (shootEffect != null && !shootEffect.isPlaying)
                    {
                        shootEffect.Play();
                    }
                    gun_Stat.Gun_State = Gun_State.FIRE;
                    Fire(AttackDelay_Gun);
                }
                else
                {
                    if(gun_Spec.heatingSound != null && !_isPlayHeatingSound)
                    {
                        SoundManagerTaehyun.instance.PlayAudioClip_OneShot(gun_Spec.heatingSound);
                        _isPlayHeatingSound = true;
                    }
                    if (_heating_Timer <= gun_Spec.heatingTime)
                    {
                        _heating_Timer += Time.deltaTime;
                        return;
                    }
                    SoundManagerTaehyun.instance.CancelAudioClip();
                    gun_Stat.isHeated = true;
                }
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)&& gun_Stat.Gun_State == Gun_State.NONE && Ammo_property > 0)
            {
                if (_weapon_Anit != null)
                {
                    _weapon_Anit.SetBool("fire", true);
                }
                if (shootLight != null)
                {
                    shootLight.enabled = true;
                }
                Ammo_property--;
                Debug.Log(string.Format("발싸!  남은 총알 : {0}", Ammo_property));
                if (gun_Spec.shootSound != null)
                {
                    SoundManagerTaehyun.instance.PlayAudioClip_OneShot(gun_Spec.shootSound);
                }
                switch (gun_Spec.gunType)
                {
                    case Gun_Kinds.GUNS:
                        ShootAmmo();
                        break;

                    case Gun_Kinds.SHOTGUN:
                        ShootShotGun(gun_Spec.quantity);
                        break;

                    case Gun_Kinds.SEEKER:
                        ShootAmmo();
                        break;

                    case Gun_Kinds.EXPLOSION:
                        ShootAmmo();
                        break;
                }
                shootEffect.Play();
                gun_Stat.Gun_State = Gun_State.FIRE;
                Fire(AttackDelay_Gun);
            }
        }
        

    }

    public void Knife_Active()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) &&
            knife_Stat.knife_State == Knife_State.NONE)
        {
            if (_weapon_Anit != null)
            {
                _weapon_Anit.SetBool("fire", true);
            }

            int randNum = Random.Range(0 , knife_Spec.audioClips_Attack.Length);
            SoundManagerTaehyun.instance.PlayAudioClip_OneShot(knife_Spec.audioClips_Attack[randNum]);
            switch (knife_Spec.knifeKind)
            {
                case Knife_Kinds.KNIFE:
                    knife_Stat.isFirstAttak = false;
                    break;
                case Knife_Kinds.HAMMER:
                    break;
            }
            Fire(AttackSpeed_Knife);
            OnOffCollider_Knife();
            knife_Stat.damage = knife_Spec.damage;
            knife_Stat.knife_State = Knife_State.ATTAKE;
        }
    }

    public void OnOffCollider_Knife()
    {
        knife_Stat.attackRange.enabled = !knife_Stat.attackRange.enabled;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealthSystem>().TakeDamage(AttackForce_Knife);
        }
        if(other.CompareTag("Bullet"))
        {
            other.GetComponent<EnemyBullet>().TakePlayerAttack(knife_Stat.reflectable);
        }
    }



    IEnumerator Timer_ReloadAtOnceAndFire(float delay, string name)
    {
        yield return new WaitForSeconds(delay);
        if (gun_Stat.Gun_State != Gun_State.NONE || knife_Stat.knife_State != Knife_State.NONE)
        {
            yield return null;
        }
        
        switch (weaponKind)
        {
            case Weapon_Kinds.GUN:
                if (name == "Fire")
                {
                    if (_weapon_Anit != null)
                    {
                        _weapon_Anit.SetBool("fire", false);
                    }
                    if (shootLight != null)
                    {
                        shootLight.enabled = false;
                    }
                }

                else if (name == "Reload" && Ammo_property != gun_Spec.maxAmmu)
                {
                    int amount_Ammo = gun_Spec.maxAmmu;

                    MessageText.Instance.Show("재장전 완료",new Vector2(PlayerMinsu.PlayerInstance.PlayerPosition().x,PlayerMinsu.PlayerInstance.PlayerPosition().y + 0.5f), 0.5f);
                    if (gun_Spec.maxAmmu - Ammo_property != 0)
                    {
                        amount_Ammo = gun_Spec.maxAmmu - Ammo_property;
                        if (PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo < amount_Ammo)
                        {
                            amount_Ammo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo;
                        }
                    }
                    

                    amount_Ammo = Mathf.Clamp(amount_Ammo, 0, gun_Spec.maxAmmu);

                    PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo -= amount_Ammo;
                    PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo, 
                                                                                            0, 
                                                                                            PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo);
                    Ammo_property += amount_Ammo;

                    MinusAmmo_Situation(gun_Spec.ammoType, amount_Ammo);
                }
                gun_Stat.Gun_State = Gun_State.NONE;     
                break;

            case Weapon_Kinds.MELEE:
                if (name == "Fire")
                {
                    if (_weapon_Anit != null)
                    {
                        _weapon_Anit.SetBool("fire", false);
                    }
                }
                OnOffCollider_Knife();
                knife_Stat.knife_State = Knife_State.NONE;
                break;

            case Weapon_Kinds.GRANADE:
                break;
        }
    }

    private void Timer_ReloadOnceAtTime()
    {
        if (gun_Stat.Gun_State == Gun_State.FIRE)
        {
            CancelInvoke("Timer_ReloadOnceAtTime");
            return;
        }
        gun_Stat.Gun_State = Gun_State.REROAD;
        _currAmmo--;
        
        PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo -= 1;
        PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo, 0, PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo);
        Ammo_property += 1;
        MinusAmmo_Situation(gun_Spec.ammoType, 1);
        gun_Stat.Gun_State = Gun_State.NONE;

        if (_currAmmo == 0)
        {
            CancelInvoke("Timer_ReloadOnceAtTime");
            MessageText.Instance.Show("재장전 완료", new Vector2(PlayerMinsu.PlayerInstance.PlayerPosition().x, PlayerMinsu.PlayerInstance.PlayerPosition().y + 0.5f) , 0.5f);
        }
    }

    public void MinusAmmo_Situation(Ammunition_Kinds kind_Ammo, int amount)
    {
        switch (kind_Ammo)
        {
            case Ammunition_Kinds.BULLET:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Bullet -= amount;
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Bullet = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Bullet, 0, PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Bullet);
                break;
            case Ammunition_Kinds.ENERGY:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Energy -= amount;
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Energy = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Energy, 0, PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Energy);
                break;
            case Ammunition_Kinds.EXPLOSIVE:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Explosion -= amount;
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Explosion = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Explosion, 0, PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Explosion);
                break;
            case Ammunition_Kinds.SHELL:
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Shell -= amount;
                PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Shell = Mathf.Clamp(PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo_Shell, 0, PlayerMinsu.PlayerInstance.playerSpec.usingMaxAmmo_Shell);
                break;
        }
    }

    protected void Fire(float delay_fire)
    {
        StartCoroutine(Timer_ReloadAtOnceAndFire(delay_fire, "Fire"));
    }

    protected void Reload(float delay_reload)
    {
        gun_Stat.Gun_State = Gun_State.REROAD;
        MessageText.Instance.Show("재장전 중", new Vector2(PlayerMinsu.PlayerInstance.PlayerPosition().x, PlayerMinsu.PlayerInstance.PlayerPosition().y + 0.5f) , delay_reload);
        if (!gun_Spec.canOnePerOneAmmo)
        {
            StartCoroutine(Timer_ReloadAtOnceAndFire(delay_reload, "Reload"));
        }
        else
        {
            _currAmmo = (gun_Spec.maxAmmu - Ammo_property);
            if (_currAmmo > PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo)
            {
                _currAmmo = PlayerMinsu.PlayerInstance.playerStat.currHavingAmmo;
            }
            InvokeRepeating("Timer_ReloadOnceAtTime", delay_reload,delay_reload);
        } 
    }


    private IEnumerator Timer_ReloadOnceAtTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
    private void FllowingMouse()
    {
        
        //--------------------------------------------------------------------
        //플레이어의 총을 마우스의 위치를 따라오도록 만드는 코드
        Vector3 mouesPos = Input.mousePosition;
        Vector3 centerPos = PlayerMinsu.PlayerInstance.usingWeapon_Transform.parent.position;

        mouesPos.z = centerPos.z - Camera.main.transform.position.z;

        Vector3 target = Camera.main.ScreenToWorldPoint(mouesPos);

        float dy = target.y - centerPos.y;
        float dx = target.x - centerPos.x;

        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        PlayerMinsu.PlayerInstance.usingWeapon_Transform.parent.rotation = Quaternion.Euler(0f, 0f, rotateDegree);
        //--------------------------------------------------------------------

        //HideGun_HigherCenter(target.y);

        if(PlayerMinsu.PlayerInstance.transform.position.x < target.x)
        {
            WeaponCenter.gameObject.transform.localScale = new Vector3(1, 1, 1);
            direction_Weapon = true;
        }
        else
        {
            WeaponCenter.gameObject.transform.localScale = new Vector3(1, -1, 1);
            direction_Weapon = false;
        }
    }


    public void ShootAmmo()
    {
        GameObject.Instantiate(gun_Spec.ammu_ForSpawn, fire_Forward.position, transform.parent.rotation).GetComponent<Bullet>().GetPlyerStats(this);
        //직접 연결해서 사용할 것
    }

    public void ShootShotGun(int Quantity) // 샷건은이걸 이용
    {        
        for (; Quantity > 0; Quantity--)
        {
            float target = Random.Range(fire_Forward.localPosition.y - gun_Stat.accuracy, fire_Forward.localPosition.y + gun_Stat.accuracy);
            fire_Forward.localRotation = Quaternion.Euler(0f, 0f, target);
            GameObject.Instantiate(gun_Spec.ammu_ForSpawn, fire_Forward.position, Quaternion.identity).GetComponent<Bullet>().GetPlyerStats(this);
            fire_Forward.localRotation = Quaternion.identity;
        }
    }
    /*private void HideGun_HigherCenter(float targety)
    {
        if (transform.parent.transform.position.y < targety)
        {
            sprite_Weapon.sortingOrder = 0;
        }
        else
        {
            sprite_Weapon.sortingOrder = 1;
        }
    }*/
}

