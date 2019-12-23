using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public Transform rootSlot;

    private List<Slot> slots;

    // Use this for initialization
    void Start()
    {
        slots = new List<Slot>();

        int slotCnt = rootSlot.childCount;//슬롯들 갯수 세기

        for (int i = 0; i < slotCnt; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();

            slots.Add(slot);
        }
    }
}
