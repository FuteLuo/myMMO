using System.Collections;
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
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }
    

}
