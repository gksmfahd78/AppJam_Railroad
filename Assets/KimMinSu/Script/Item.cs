using UnityEngine;

[System.Serializable]
public class Item_Spec
{
    public string name;
    public int heal_Account;
}

[System.Serializable]
public class Item_Stat
{

}

public class Item : MonoBehaviour
{
    Item_Spec spec;
    Item_Stat stat;

    private void Awake()
    {
        
    }
   
}
