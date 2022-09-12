﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UIRegister : MonoBehaviour {

    public InputField username;
    public InputField password;
    public InputField ConfirmPW;
    public Button buttonRegister;

    // Use this for initialization
    void Start () {
        UserService.Instance.OnRegister = this.OnRegister;
	}
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickRegister()
    {
        if(string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.ConfirmPW.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.password.text != this.ConfirmPW.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }

    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        MessageBox.Show(string.Format("注册成功！result: {0} msg:{1}",result,msg));
    }


}
