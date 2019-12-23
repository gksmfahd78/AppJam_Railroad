using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

public class PlayerMinsu : MonoBehaviour, ICharactor, IPointerDownHandler,IPointerUpHandler{ // 플레이어 포시션 구하는 함수 넣었놨으니까 플레이어 인스턴스에 접근해서 PlayerPosition() 이 함수 쓰면 나

    private bool start_IndexOfWeapon = true;

    [Header("개발자 전용") , SerializeField]
    private GameObject developerGun;


    [Header("Weapon")]
    public Weapon[] weapons; // 자신이 획득한 무기배열
    public Weapon weapon; // 현재 사용중인 무기
    public Transform usingWeapon_Transform;// 사용중인 무기 컴포넌트
    public Transform WeaponPos; // 무기 포지션 > 무기가 생성될 포지션
    public Image[] UsingWeaponInterface_Image; //무기 슬롯의 무기 이미지
    public Image[] UsingWeaponInterface_BackGround; // 무기슬롯의 백그라운드 이미지
    public int weapons_Index = 0; // 무기배열에 저장하기 위한 인덱스
    public Image currWeaponEmphasisImgae;// 현재 사용하는 총을 강조시킬 이미지
    private bool check_WeaponSlot = true; // 1 = true, 2 = false


    [Header("ActiveItem")]
    public Image usingActiveItem_Image; // 사용중인 액티브 아이템 이미지를 저장할 변수
    public Image usingActiveItem_Background;
    public Image coolTimeImage; // 액티브 아이템의 쿨 타임 표시
    public ActiveItem activeItem; // 현재 사용중인 액티브 아이템
    public Transform activeItemPos;

    [Header("PassiveItem")]
    public List<PassiveItem> passiveItems; // 현재 사용중인 패시브 아이템
    public Transform passiveItemPos;

    [Header("playerStat")]
    public Stat playerStat; // 캐릭터 디버프 상태를 나타내는 열거형
    public float downJumpTime = 0.3f;


    public GameObject startingWeapon;
    public int groundIndex;
    [SerializeField]
    private HealthUI _healthPlayerUi;
    public ItemBox nearBox; // 가까운 아이템 박스가 저장될 변수
    public DropItem nearItem; // 가까운 아이템이 저장될 변수
    private int _scrollCount; // 얼마나 스크롤 됫는지 세줄 변수
    private int _scrollNumber = 3; //  scrollNumber만큼 마우스 휠을 돌려야 바뀜
    private bool _isDownJump = false;
    private float _betDownJump = 0f;

    private bool isMove = false;
    private int direction;
    
    public float AttackSpeed
    {
        get
        {
            return playerStat.shotDelayTime;
        }

        set
        {
            var shotDelayTime = playerStat.shotDelayTime;
            playerStat.shotDelayTime = value;
            if (playerStat.shotDelayTime != shotDelayTime)
            {
                switch (weapon.weaponKind)
                {
                    case Weapon_Kinds.GUN:
                        weapon.AttackDelay_Gun = playerStat.shotDelayTime;
                        break;
                    case Weapon_Kinds.MELEE:
                        weapon.AttackSpeed_Knife = playerStat.shotDelayTime;
                        break;
                    case Weapon_Kinds.GRANADE:
                        break;
                }
            }
        }
    }

    public float ReloadSpeed
    {
        get
        {
            return playerStat.reloadDelayTime;
        }

        set
        {
            playerStat.reloadDelayTime = value;
            weapon.ReloadSpeed = playerStat.reloadDelayTime;
        }
    }

    public float Additional_AttackForce
    {
        get
        {
            return playerStat.additionalDamage;
        }

        set
        {
            var firstAdditionalDamage = playerStat.additionalDamage;
            playerStat.additionalDamage = value;
            if (playerStat.additionalDamage != firstAdditionalDamage)
            {
                switch (weapon.weaponKind)
                {
                    case Weapon_Kinds.GUN:
                        weapon.AttackForce_Gun = playerStat.additionalDamage;
                        break;
                    case Weapon_Kinds.MELEE:
                        weapon.AttackForce_Knife = playerStat.additionalDamage;
                        break;
                    case Weapon_Kinds.GRANADE:
                        //weapon.AttackForce_Granade = playerStat.additionalDamage;
                        break;
                }
            }
        }
    }


    [Header("playerSpec")]
    public Spec playerSpec;

    [Header("state")]
    public CharState playerState;
    public bool isDeath = false; // 죽었는지 확인하는 변수
    public bool canJump = true; // 점프한 상태인가 체크하는 변수
    public float hitPostTime;
    public float hitPostAcount;
    private SpriteRenderer thisSprite_player; // 플레이어의 스프라이트를 제어할 변수
    private Dictionary<KeyCode, Vector3> playerAction_Move; // 플레이어의 움직임을 제어하는 딕셔너리변수
    private ChromaticAberration hitPost;
    private PostProcessVolume post;
    

    private Vector2 _startingCenterPos;

    //player
    private float _wheelInputTimer = 0f;
    [SerializeField]
    private float _wheelInputCooltime = 0.1f;
    public LayerMask layer_Jump;
    public Collider2D JumpCollider;
    private Collider2D currGround;
    private bool canChangeisGround = false;
    private Animator playerAni;
    private Rigidbody2D player; // 플레이어 오브젝트 변수

    public float jumpTime = 0.5f;
    private float _jumpTimer = 0f;
    private bool canSwapJump = true;

    public int Health // 프로퍼티를 이용하여 쉽게 코딩하기 위한 것(현재 체력을 설정함)
    {
        set
        {
            playerStat.health = Mathf.Clamp(value, 0, playerSpec.maxHealth);
            _healthPlayerUi.MyCurrentValue = playerStat.health;
            if (playerStat.health <= 0 && !isDeath)
            {
                Death();
            }
        }
        get
        {
            return playerStat.health;
        }
    }

    private static PlayerMinsu instance = null; //싱글턴문법을 이용하여 외부에서 얻어오기

    public static PlayerMinsu PlayerInstance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        playerStat.currHavingAmmo_Bullet = playerSpec.startAmmo_Bullet;
        playerStat.speed = playerSpec.speed;
        usingWeapon_Transform = GameObject.FindGameObjectWithTag("weaponPos").transform;
        weapons = new Weapon[2];

        if (PlayerInstance == null)
            instance = this;

        else if (PlayerInstance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        player = GetComponent<Rigidbody2D>();
        thisSprite_player = GetComponent<SpriteRenderer>();
        playerAni = GetComponent<Animator>();

        var installWeaponResult = InstallWeapon();

        weapon = GetComponentInChildren<Weapon>(true); //오브젝트가 꺼져있어도 true면 찾음 true가 아니면 꺼져있을 때 못 찾음 weapon이 이미 null이야
        weapons[0] = weapon.GetComponent<Weapon>();

        UsingWeaponInterface_Image[0].sprite = weapons[0].gameObject.GetComponent<SpriteRenderer>().sprite;
        UsingWeaponInterface_BackGround[0].color = Color.gray;
    }

    void Start()
    {
        _betDownJump = Time.time;
        _startingCenterPos = WeaponPos.parent.localPosition;
        _healthPlayerUi.Initialized(playerSpec.maxHealth, playerSpec.maxHealth);

        playerState = CharState.NONE;
        playerAction_Move = new Dictionary<KeyCode, Vector3>
        {
            { KeyCode.D, new Vector3(1,0,0) }, //오른쪽으로 이동
            { KeyCode.A, new Vector3(-1,0,0) }, //왼쪽으로 이동
        };

        Health = playerSpec.maxHealth;
        weapon.isUsedWeapon = true;
        post = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();
    }
    public bool InstallWeapon()
    {
        weapon = (Instantiate(startingWeapon, WeaponPos.position, Quaternion.identity, WeaponPos) as GameObject).GetComponent<Weapon>();
        return weapon != null;
    }

    void Update()
    {
        if (!isDeath)
        {

            AttackSpeed = playerStat.shotDelayTime == AttackSpeed ? AttackSpeed : playerStat.shotDelayTime;

            ReloadSpeed = playerStat.reloadDelayTime == ReloadSpeed ? ReloadSpeed : playerStat.reloadDelayTime;

            Additional_AttackForce = playerStat.additionalDamage == Additional_AttackForce ? Additional_AttackForce : playerStat.additionalDamage;

            UpdateWeaponCenter();
            foreach (var passive in passiveItems)
            {
                if (passive != null && passive.canUsePassiveSkill)
                {
                    UsingPassiveItem(passive);
                }
            }

            CountingDownJump();

            if (activeItem != null)
            {
                switch (activeItem.spec.usingCondition)
                {
                    case usingCondition.TIMER:
                        if (!activeItem.stat.isFirstSetting && !activeItem.stat.canActive) // 액티브 아이템의 쿨타임
                        {
                            if (activeItem.stat.time <= 0)
                            {
                                activeItem.stat.time = activeItem.spec.time;
                            }
                            activeItem.ActiveItemTimer();
                            coolTimeImage.fillAmount = activeItem.stat.time / activeItem.spec.time;
                        }
                        break;
                    case usingCondition.CLEARTRAIN:
                        float filled = 0f;
                        if (!activeItem.stat.canActive && !activeItem.stat.isFirstSetting)
                        {
                            activeItem.ClearTrainCount();
                            int clearTrainCount = (3 - activeItem.stat.clearedTrain);
                            filled = clearTrainCount == 3 ? 1f : clearTrainCount / 3f;
                        }
                        coolTimeImage.fillAmount = filled;

                        break;
                }
            }


            /*if (Input.GetKeyDown(KeyCode.H))
            {
                GameManager.instance.CreateDropItem(ItemType.Weapon, PlayerPosition());
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                GameManager.instance.CreateDropItem(ItemType.Heal, PlayerPosition());
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.instance.CreateDropItem(ItemType.Ammo, PlayerPosition());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameManager.instance.CreateDropItem(ItemType.ActiveItem, PlayerPosition());
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameManager.instance.CreateDropItem(ItemType.PassiveItem, PlayerPosition());
            }*/

            /*if (Input.GetKeyDown(KeyCode.F12))
            {
                GameObject.Instantiate(developerGun, PlayerPosition(), Quaternion.identity);
            }*/
            ChangeFlip();
            //CheckWeaponSwitch();
            /*if (Input.anyKey)
            {
                foreach (var action_move in playerAction_Move)
                {
                    if (Input.GetKey(action_move.Key))
                    {
                        Move(action_move.Value);
                    }
                    else if (Input.GetKeyUp(action_move.Key))
                    {
                    }
                }
            }
            else
            {
                playerAni.SetFloat("Walking", 0); // idle 애니메이션 실행
            }*/
            if (!canSwapJump)
            {
                CountingJumpTime();
            }

            if (!canJump)
            {
                FallingPlayer();
            }
        }
    }

    private void CountingJumpTime()
    {
        if(_jumpTimer <= 0f)
        {
            _jumpTimer = jumpTime;
            canSwapJump = true;
        }
        else
        {
            _jumpTimer -= Time.deltaTime;
        }
        
    }

    public void PushThePickUpButton()
    {
        if (nearItem != null)
        {
            PickupItem(nearItem);
        }
        if (nearBox != null)
        {
            nearBox.OpenItemBox();
        }
    }

    public void UsingActiveItemButton()
    {
        if (activeItem != null)
        {
            UsingActiveItem();
        }
    }

    private void CountingDownJump()
    {
        if(Time.time >= _betDownJump + downJumpTime)
        {
            _isDownJump = false;
        }
    }

    private IEnumerator DownJump()
    {
        yield return new WaitForSeconds(0.1f);
        JumpCollider.enabled = true;
    }

    private void UsingPassiveItem(PassiveItem item)
    {
        item.PassiveAbility_Item();
    }

    private void UpdateWeaponCenter()
    {
        Vector2 center = Vector2.zero;
        if (weapon.weaponKind == Weapon_Kinds.GUN)
        {
            center = weapon.gun_Spec.pivotOffset;
        }

        else if (weapon.weaponKind == Weapon_Kinds.MELEE)
        {
            center = weapon.knife_Spec.pivotOffset;
        }

        if (thisSprite_player.flipX)
        {
            center.x = -center.x;
        }
        WeaponPos.parent.localPosition = _startingCenterPos + center;
    }

    /*public void Move(Vector3 direction)
    {
        transform.position += Time.deltaTime * playerStat.speed * direction;
        playerAni.SetFloat("Walking", 1f); // walking 애니메이션 실행       
    }*/

    

    private void Jump(float force)
    {
        player.velocity = Vector2.zero;
        player.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    }
    
    /*public void IncreaseJumpForce() // 스페이스바를 누르고 있으면 점프를 더 많이 뜀
    {
        float increase = playerSpec.jumpIncrease - 0.4f;
        playerStat.jumpForce += increase;
        playerStat.jumpForce = Mathf.Clamp(playerStat.jumpForce, playerSpec.jumpForce, playerSpec.maxJumpForce);
    }*/

    private void FallingPlayer()
    {
        //스페이스바를 Up하거나 점프력이 최대점프력 이상이면 실행됨
        canJump = false;
        player.gravityScale = playerSpec.fallingGravity;
        playerStat.jumpForce = playerSpec.jumpForce;
    }

    private void FixedUpdate()
    {
        float rayDistance = 0.03f;
        Vector2 rayDirection = Vector2.down;
        Vector2 from = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit_Jump = Physics2D.Raycast(from, rayDirection, rayDistance, layer_Jump);

        if (hit_Jump.collider == null) {
            canJump = false;
            return;
        } 

        else
        {
            if (canSwapJump)
            {
                canJump = true;
            }
            currGround = hit_Jump.collider;
            if(hit_Jump.collider.tag == "Ground" && hit_Jump.collider.gameObject.GetComponent<Ground>() != null)
                groundIndex = hit_Jump.collider.gameObject.GetComponent<Ground>().groundIndex;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 a = new Vector2(0, -0.03f);
        Vector2 from = new Vector2(transform.position.x, transform.position.y);
        
        Gizmos.DrawRay(from, a);
    }
    

    public void ChangeFlip()
    {
        if (weapon.direction_Weapon)
        {
            thisSprite_player.flipX = true;
        }

        else
        {
            thisSprite_player.flipX = false;
            
        }
    }

    public void Death()
    {
        isDeath = true;
        
        player.gravityScale = 1;
        player.freezeRotation = false;
        player.AddForce(new Vector2(0, 4f), ForceMode2D.Impulse);
        player.AddTorque(5f, ForceMode2D.Impulse);
        Destroy(gameObject);
    }

    public void TakeDeBuff(CharState type, float time)
    {
        
    }

    public void DieEffect<T>(T name)
    {
        throw new System.NotImplementedException();
    }

    private void UsingActiveItem()
    {
        activeItem.ActiveAbility_Item();
    }

    private void PickupItem(DropItem nearItem) // E키를 눌렀을 때 작동하는 함수
    {
        bool canDrop = true;
        switch (nearItem.ItemType)
        {
            case ItemType.Weapon:
                var newWeapon = GameObject.Instantiate(nearItem.ItemPrefab).GetComponent<Weapon>();
                newWeapon.weaponPrefab = nearItem.ItemPrefab;
                var weaponName = "";

                if(newWeapon.weaponKind == Weapon_Kinds.GUN)
                {
                    weaponName = newWeapon.gun_Spec.gunName;
                }

                else if(newWeapon.weaponKind == Weapon_Kinds.MELEE)
                {
                    weaponName = newWeapon.knife_Spec.knifeName;
                }
                MessageText.Instance.Show(weaponName, PlayerPosition());

                weapons_Index = ReturnToWeaponArrayIndex(weapons, weapons_Index);

                if (weapons[weapons_Index] != null) //무기 슬롯이 남지 않았을때 작동하는 함수
                {
                    ChangeWeapon_E(newWeapon);
                }

                else // 무기 슬롯 다음칸에 무기가 없을 시에 작동됨
                {
                    weapons[weapons_Index] = newWeapon;
                    SetWeapon(newWeapon);
                    newWeapon.gameObject.SetActive(false);
                }
                UsingWeaponInterface_Image[weapons_Index].sprite = weapons[weapons_Index].gameObject.GetComponent<SpriteRenderer>().sprite;
                weapons[weapons_Index].GetComponent<SpriteRenderer>().flipY = !thisSprite_player.flipX;
                //무기를 변경한다.
                break;
            case ItemType.Ammo:
                //탄약을 충전시킨다.
                break;

            case ItemType.Heal:
                if(Health == playerSpec.maxHealth)
                {
                    canDrop = false;
                    MessageText.Instance.Show("이미 체력이 가득 차 있습니다.", transform.position);//position = 플레이어의 중앙위치
                }
                else
                {
                    Health += nearItem.ItemPrefab.GetComponent<HealItem>().heal_Amount;//힐맨
                }
                break;

            case ItemType.ActiveItem:
                var newItem_Active = GameObject.Instantiate(nearItem.ItemPrefab).GetComponent<ActiveItem>();
                newItem_Active.ActivePrefab = nearItem.ItemPrefab;
                SetActiveItem(newItem_Active);
                newItem_Active.transform.position = activeItemPos.position;
                usingActiveItem_Image.sprite = newItem_Active.GetComponent<SpriteRenderer>().sprite;
                usingActiveItem_Background.color = new Color(0.5f,0.5f,0.5f,0.5f);

                //액티브아이템을 변경한다.
                break;
            case ItemType.PassiveItem:
                var newItem_Passive = Instantiate(nearItem.ItemPrefab).GetComponent<PassiveItem>();
                SetPassiveItem(newItem_Passive);
                newItem_Passive.transform.position = passiveItemPos.position;
                //패시브효과를 적용한다.
                break;
        }
        
        //먹은 아이템을 지운다.
        if (canDrop)
        {
            GameObject.Destroy(nearItem.gameObject);
        }
        this.nearItem = null;

    }

    private void SetPassiveItem(PassiveItem _nearItem)
    { 
        passiveItems.Add(_nearItem);
    }

    private void SetActiveItem(ActiveItem _nearItem)
    {
        if (activeItem != null)
        {
            DropTheItem(activeItem);
        }
        activeItem = _nearItem.GetComponent<ActiveItem>();
    }


    private void DropTheItem(ActiveItem changeItem) //바꿀아이템을 넘겨줌
    {
        changeItem.gameObject.SetActive(true);
        changeItem.transform.position = PlayerPosition();
    }

    private void ChangeWeapon_E(Weapon changeWeapon)// 상호작용을 통한 무기 바꾸기 / 먹기
    {
        DropTheWeapon(weapon);
        SetWeapon(changeWeapon);
        weapons[weapons_Index] = changeWeapon;
        weapon = changeWeapon;
        weapon.gameObject.SetActive(true);
        weapon.isUsedWeapon = true;
    }
    /*public void CheckWeaponSwitch()
    {
        var isWheelMoving = Mathf.Abs(Input.mouseScrollDelta.y) > 0f;
        var canSwitchWeapon = weapons[0] != null && weapons[1] != null;

        //Debug.Log(string.Format("{0:0.0000}", Input.GetAxis("Mouse ScrollWheel")) + " / " + _wheelInputTimer);
        if (isWheelMoving && canSwitchWeapon && _wheelInputTimer <= 0f)
        {
            SwitchWeapon();
            _wheelInputTimer = _wheelInputCooltime;
            //Debug.Log("Switch");
        }
        if (_wheelInputTimer > 0f)
        {
            _wheelInputTimer -= Time.deltaTime;
        }
    }*/


    public void SetWeapon1()
    {
        if(weapons[0] != null)
        {
            weapon.isUsedWeapon = false;
            weapon.gameObject.SetActive(false);
            weapon.gun_Stat.Gun_State = Gun_State.NONE;

            weapon = weapons[0].GetComponent<Weapon>();
            UsingWeaponInterface_BackGround[0].color = Color.gray;
            UsingWeaponInterface_BackGround[1].color = Color.white;
            currWeaponEmphasisImgae.rectTransform.position = UsingWeaponInterface_BackGround[0].rectTransform.position;

            weapon.gameObject.SetActive(true);
            weapon.isUsedWeapon = true;
            SetWeapon(weapon);
        }
    }    

    public void SetWeapon2()
    {
        if (weapons[1] != null)
        {
            weapon.isUsedWeapon = false;
            weapon.gameObject.SetActive(false);
            weapon.gun_Stat.Gun_State = Gun_State.NONE;

            weapon = weapons[1].GetComponent<Weapon>();
            UsingWeaponInterface_BackGround[1].color = Color.gray;
            UsingWeaponInterface_BackGround[0].color = Color.white;
            currWeaponEmphasisImgae.rectTransform.position = UsingWeaponInterface_BackGround[1].rectTransform.position;

            weapon.gameObject.SetActive(true);
            weapon.isUsedWeapon = true;
            SetWeapon(weapon);
        }
    }
    public void Reload()
    {
        weapon.ReLoadBnt();
    }

    /*private void SwitchWeapon()
    {
        weapons_Index = 1 - weapons_Index;
        var notUsingWeaponIndex = 1 - weapons_Index;

        weapon.isUsedWeapon = false;
        weapon.gameObject.SetActive(false);
        weapon.gun_Stat.Gun_State = Gun_State.NONE;

        weapon = weapons[weapons_Index].GetComponent<Weapon>();
        UsingWeaponInterface_BackGround[weapons_Index].color = Color.gray;
        UsingWeaponInterface_BackGround[notUsingWeaponIndex].color = Color.white;
        currWeaponEmphasisImgae.rectTransform.position = UsingWeaponInterface_BackGround[weapons_Index].rectTransform.position;

        weapon.gameObject.SetActive(true);
        weapon.isUsedWeapon = true;
        SetWeapon(weapon);
    }*/
    /*
    public void OnChangeWeapon_Wheel()
    {
        int notUsingWeapon_Index = weapons_Index;
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0f)
        {
            _scrollCount++;
        }
        if (_scrollCount > _scrollNumber)
        {
            _scrollCount = 0;

            check_WeaponSlot = !check_WeaponSlot;
            weapons_Index = check_WeaponSlot ? 1 : 0;

            if (weapons[weapons_Index] == null || weapons_Index == notUsingWeapon_Index) return;

            weapon.isUsedWeapon = false;
            weapon.gameObject.SetActive(false);
            weapon.gun_Stat.Gun_State = Gun_State.NONE;

            weapon = weapons[weapons_Index].GetComponent<Weapon>();
            UsingWeaponInterface_BackGround[weapons_Index].color = Color.gray;
            UsingWeaponInterface_BackGround[notUsingWeapon_Index].color = Color.white;
            currWeaponEmphasisImgae.rectTransform.position = UsingWeaponInterface_BackGround[weapons_Index].rectTransform.position;

            weapon.gameObject.SetActive(true);
            weapon.isUsedWeapon = true;
            SetWeapon(weapon);
        }
    }*/

    public void SetWeapon(Weapon weapon_Setting)
    {
        if(weapon_Setting.weaponKind == Weapon_Kinds.GUN)
        {
            weapon_Setting.transform.parent = usingWeapon_Transform;
        }
        else if(weapon_Setting.weaponKind == Weapon_Kinds.MELEE)
        {
            weapon_Setting.transform.parent = weapon_Setting.WeaponCenter.transform;
            weapon_Setting.transform.position += (Vector3)weapon_Setting.knife_Spec.pivotOffset;
        }
        
        weapon_Setting.transform.localPosition = Vector3.zero;
        weapon_Setting.transform.localRotation = Quaternion.identity;
    }

    public void DropTheWeapon(Weapon weapon_Droped)
    {
        GameManagerTaehyun.instance.CreateDropItem(ItemType.Weapon , weapon_Droped.weaponPrefab, PlayerPosition());
        GameObject.Destroy(weapon_Droped.gameObject);
    }

    public int ReturnToWeaponArrayIndex(Weapon[] weapons, int index)
    {
        if (start_IndexOfWeapon)
        {
            start_IndexOfWeapon = false;
            return 1;
        }

        else
        {
            return weapon == weapons[index] ? index : 1 - index;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ammo")
        {
            PlayerAmmo ammo = other.gameObject.GetComponent<DropItem>().ItemPrefab.GetComponent<PlayerAmmo>();
            switch (ammo.ammoKind)
            {
                case Ammunition_Kinds.BULLET:
                    playerStat.currHavingAmmo_Bullet += ammo.ammoCharge;
                    playerStat.currHavingAmmo_Bullet = Mathf.Clamp(playerStat.currHavingAmmo_Bullet, 0, playerSpec.usingMaxAmmo_Bullet);
                    playerStat.currHavingAmmo = Mathf.Clamp(playerStat.currHavingAmmo, 0, playerSpec.usingMaxAmmo);
                    break;
                case Ammunition_Kinds.ENERGY:
                    playerStat.currHavingAmmo_Energy += ammo.ammoCharge;
                    playerStat.currHavingAmmo_Energy = Mathf.Clamp(playerStat.currHavingAmmo_Energy, 0, playerSpec.usingMaxAmmo_Energy);
                    playerStat.currHavingAmmo = Mathf.Clamp(playerStat.currHavingAmmo, 0, playerSpec.usingMaxAmmo);
                    break;
                case Ammunition_Kinds.EXPLOSIVE:
                    playerStat.currHavingAmmo_Explosion += ammo.ammoCharge;
                    playerStat.currHavingAmmo_Explosion = Mathf.Clamp(playerStat.currHavingAmmo_Explosion, 0, playerSpec.usingMaxAmmo_Explosion);
                    playerStat.currHavingAmmo = Mathf.Clamp(playerStat.currHavingAmmo, 0, playerSpec.usingMaxAmmo);
                    break;
                case Ammunition_Kinds.SHELL:
                    playerStat.currHavingAmmo_Shell += ammo.ammoCharge;
                    playerStat.currHavingAmmo_Shell = Mathf.Clamp(playerStat.currHavingAmmo_Shell, 0, playerSpec.usingMaxAmmo_Shell);
                    playerStat.currHavingAmmo = Mathf.Clamp(playerStat.currHavingAmmo, 0, playerSpec.usingMaxAmmo);
                    break;
            }

            switch (weapon.gun_Spec.ammoType)
            {
                case Ammunition_Kinds.BULLET:
                    playerStat.currHavingAmmo = playerStat.currHavingAmmo_Bullet;
                    break;
                case Ammunition_Kinds.ENERGY:
                    playerStat.currHavingAmmo = playerStat.currHavingAmmo_Energy;
                    break;
                case Ammunition_Kinds.EXPLOSIVE:
                    playerStat.currHavingAmmo = playerStat.currHavingAmmo_Explosion;
                    break;
                case Ammunition_Kinds.SHELL:
                    playerStat.currHavingAmmo = playerStat.currHavingAmmo_Shell;
                    break;
            }

            Destroy(other.gameObject);
        }

    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DropItem"))
        {
            nearItem = other.gameObject.GetComponent<DropItem>();
        }
        if (other.gameObject.CompareTag("ItemBox"))
        {
            nearBox = other.gameObject.GetComponent<ItemBox>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DropItem>() == nearItem)
        {
            nearItem = null;
        }
        if (other.gameObject.GetComponent<ItemBox>() == nearBox)
        {
            nearBox = null;
        }
    }

    public void TakeDamage(EnemyDamageInfo info)
    {
        var isAlive = Health > 0;
        if (isAlive)
        {
            Health -= info.Damage;
            post.profile.TryGetSettings(out hitPost);
            hitPost.intensity.value = hitPostAcount;
            StartCoroutine(BackToOriginallyIntensity());
            if (Health <= 0)
            {
                GameManagerTaehyun.instance.EndGame(info);
            }
        }
    }

    private IEnumerator BackToOriginallyIntensity()
    {
        yield return new WaitForSeconds(hitPostTime);
        hitPost.intensity.value = 0f;
    }

    public Vector2 PlayerPosition()
    {
        return new Vector2(transform.position.x, transform.position.y + 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void RightButton()
    {
        playerAni.SetFloat("Walking", 1f); // walking 애니메이션 실행    
        isMove = true;
        direction = 1;
        transform.position += Time.deltaTime * playerStat.speed * (transform.right * direction);
    }

    public void LeftButton()
    {
        playerAni.SetFloat("Walking", 1f); // walking 애니메이션 실행    
        isMove = true;
        direction = -1;
        transform.position += Time.deltaTime * playerStat.speed * (transform.right * direction);
    }

    public void ButtonUp()
    {
        playerAni.SetFloat("Walking", 0f); // walking 애니메이션 실행    
        isMove = false;
    }

    public void UpButton()
    {
        if (canJump)
        {
            player.gravityScale = 1f;
            Jump(playerStat.jumpForce);
            canSwapJump = false;
        }
    }

    public void DownButton()
    {
        if (currGround != null && !_isDownJump)
        {
            if (currGround.GetComponent<PlatformEffector2D>() != null && canJump)
            {
                JumpCollider.enabled = false;
                FallingPlayer();
                StartCoroutine(DownJump());
                _isDownJump = true;
            }
        }
    }
}