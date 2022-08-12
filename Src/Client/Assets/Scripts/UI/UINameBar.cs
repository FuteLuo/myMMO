using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{

    public Text avaterName;

    //public Image avaterIcon;

    public Character character;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateInfo();

        this.transform.forward = Camera.main.transform.forward;
    }

    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + " Lv." + this.character.Info.Level;
            if(name != this.avaterName.text)
            {
                this.avaterName.text = name;
            }
        }
    }
}
