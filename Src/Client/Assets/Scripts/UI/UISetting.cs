﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UIWindow {

    public void ExitToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }
	
    public void ExitGame()
    {
        Services.UserService.Instance.SendGameLeave(true);
    }
}
