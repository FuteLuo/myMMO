using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UILogin : MonoBehaviour {

    public InputField username;
    public InputField password;
    public Button buttonLogin;

	// Use this for initialization
	void Start () {
        UserService.Instance.OnLogin = OnLogin;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickLogin()
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
        UserService.Instance.SendLogin(this.username.text, this.password.text);

    }


    void OnLogin(SkillBridge.Message.Result result, string msg)
    {
        if(result == SkillBridge.Message.Result.Success)
        {
            //MessageBox.Show(string.Format("登陆成功！请稍后... result: {0}, msg:{1}", result, msg));
            SceneManager.Instance.LoadScene("CharSelect");

        }
        else
        {
            MessageBox.Show(msg, "错误", MessageBoxType.Error);
        }
            

    }

}
