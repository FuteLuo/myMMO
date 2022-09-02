using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildItem : ListView.ListViewItem {

    public Text GuildID;
    public Text GuildName;
    public Text MemberNum;
    public Text Leader;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public NGuildInfo Info;
    // Use this for initialization
    void Start()
    {

    }

    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.GuildID != null) this.GuildID.text = this.Info.Id.ToString();
        if (this.GuildName != null) this.GuildName.text = this.Info.GuildName;
        if (this.MemberNum != null) this.MemberNum.text = this.Info.memberCount.ToString();
        if (this.Leader != null) this.Leader.text = this.Info.leaderName;

    }
}
