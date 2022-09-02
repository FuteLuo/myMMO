﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatorName;
    public Text avatorLevel;
    public Text avatorId;

    public UITeam TeamWindow;

	// Use this for initialization
	protected override void OnStart () {
        this.UpdateAvator();
	}

    void UpdateAvator()
    {
        this.avatorName.text = string.Format("{0}", User.Instance.CurrentCharacter.Name);
        this.avatorLevel.text = User.Instance.CurrentCharacter.Level.ToString();
        this.avatorId.text = string.Format("ID: {0}",User.Instance.CurrentCharacter.Id);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle("这是一个标题栏");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框： " + result, "对话框响应结果", MessageBoxType.Information);
    }

    public void OnClickBag()
    {
        UIBag uiBag = UIManager.Instance.Show<UIBag>();
        uiBag.SetTitle(User.Instance.CurrentCharacter.Gold.ToString());
    }

    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }

    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }
    public void OnClickSkill()
    {
        
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
}

