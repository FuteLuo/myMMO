using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow {

    public Text description;

    public GameObject itemPrefab;

    public ListView listMain;

    private UIRideItem selectedItem;


    // Use this for initialization
    void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnDestroy()
    {
        
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.description.text = this.selectedItem.item.Define.Description;
    }

    void RefreshUI()
    {
        ClearItems();
        InitItems();
        
    }
    /// <summary>
    /// 清空所有装备列表
    /// </summary>
    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }
    /// <summary>
    /// 初始化已经装备的装备列表
    /// </summary>
    private void InitItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            //显示类型是可以装备的并且是当前职业的
            if (kv.Value.Define.Type == ItemType.Ride && kv.Value.Define.LimitClass == CharacterClass.None || kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;

                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetEquipItem(kv.Value, this, false);
                this.listMain.AddItem(ui);
            }
        }
    }

    public void DoRide()
    {
        if(this.selectedItem == null)
        {
            MessageBox.Show("请选择要召唤的坐骑", "提示");
        }
        User.Instance.Ride(this.selectedItem.item.Id);
    }

}
