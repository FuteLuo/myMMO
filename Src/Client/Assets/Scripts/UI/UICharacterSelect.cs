using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;

public class UICharacterSelect : MonoBehaviour {

    public GameObject panelCreate;
    public GameObject panelSelect;

    public GameObject btnCreateCancel;

    public InputField charName;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    private int selectCharacterIdx = -1;

	public UICharacterView characterView;
	// Use this for initialization
	void Start () {
		
	}
	
    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if(init)
        {
            foreach(var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            for(int i=0; i<User.Instance.Info.Player.Characters.Count;i++)
            {
                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    //OnSele
                });

                uiChars.Add(go);
                go.SetActive(true);
            }
        }
    }

    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
    }


	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickCreate()
    {

    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;
    }
}
