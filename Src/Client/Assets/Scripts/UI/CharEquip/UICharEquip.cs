using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow {
    public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;

    public List<Transform> slots;


	// Use this for initialization
	void Start () {
        RefreshUI();
        EquipManager.Instance.onEquipChanged += RefreshUI;
	}

    private void OnDestroy()
    {
        EquipManager.Instance.onEquipChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipedList();
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }
    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    void InitEquipedItems()
    {
        for(int i = 0; i < (int)EquipSlot.MaxSlot; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if(item != null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
        
    }
    /// <summary>
    /// 清空所有装备列表
    /// </summary>
    private void ClearEquipedList()
    {
        foreach(var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }
    /// <summary>
    /// 初始化已经装备的装备列表
    /// </summary>
    private void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            //显示类型是可以装备的并且是当前职业的
            if(kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                //已经装备的不显示在左边装备列表
                if(EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }
    /// <summary>
    /// 清空已经装备的装备列表
    /// </summary>
    private void ClearAllEquipList()
    {
        foreach(var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}
