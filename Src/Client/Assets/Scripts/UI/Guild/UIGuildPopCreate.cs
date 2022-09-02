using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate : UIWindow {

    public InputField inputName;
    public InputField inputNotice;

    // Use this for initialization
    void Start () {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
	}

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }

    public override void OnYesClick()
    {
        if(string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
            return;
        }
        if (inputName.text.Length < 2 || inputName.text.Length > 5)
        {
            MessageBox.Show("公会名称2-5个字符", "错误", MessageBoxType.Error);
            return;
        }

        if (string.IsNullOrEmpty(inputNotice.text))
        {
            MessageBox.Show("请输入公会宣言", "错误", MessageBoxType.Error);
            return;
        }
        if (inputNotice.text.Length < 2 || inputNotice.text.Length > 50)
        {
            MessageBox.Show("公会宣言为2-50个字符", "错误", MessageBoxType.Error);
            return;
        }

        GuildService.Instance.SendGuildCreate(inputName.text, inputNotice.text);

    }

    void OnGuildCreated(bool result)
    {
        if (result)
            this.Close(WindowResult.Yes);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
