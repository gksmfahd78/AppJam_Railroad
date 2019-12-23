using System.Collections;
using UnityEngine;

public enum CharState
{
    NONE,
    POISON,
    SLOWING,
    FAINT,
}

[System.Serializable]
public class Spec
{
    public float jumpIncrease;
    public int maxHealth;
    public float speed;
    public float jumpForce;
    public float maxJumpForce;
    public float minJumpForce;
    public float fallingGravity;
    public int usingMaxAmmo;
    public int usingMaxAmmo_Bullet;
    public int usingMaxAmmo_Shell;
    public int usingMaxAmmo_Explosion;
    public int usingMaxAmmo_Energy;
    public int startAmmo_Bullet;

    public AudioClip heatingSound; // 가열될때 재생될 사운드
    public AudioClip shootSound; // 총쏠때 재생될 사운드
    public AudioClip cancelShootSound; // 총쏘는게 멈추거나 끝나고 처리해줄 사운드
}

[System.Serializable]
public class Stat
{
    public float speed;
    public float jumpForce;
    public int health;
    public float shotDelayTime;
    public float reloadDelayTime;
    public float additionalDamage;
    public int currHavingAmmo_Bullet;
    public int currHavingAmmo_Energy;
    public int currHavingAmmo_Shell;
    public int currHavingAmmo_Explosion;
    public int currHavingAmmo;
}

interface ICharactor
{
    void Death();
    void TakeDeBuff(CharState type, float time);
    void DieEffect<T>(T name);
}