using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem {

    public Image icon;
    public Text title;
    public Text level;
   

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;


    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public Item item;
    // Use this for initialization
    void Start () {
		
	}

    public void SetEquipItem(Item item, UIRide owner, bool equiped)
    {
        
        this.item = item;

        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}
