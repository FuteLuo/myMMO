using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestInfo : MonoBehaviour {
    public Text title;

    public Text[] targets;

    public Text description;

    public UIIconItem rewardItems;

    public Text rewardMoney;
    public Text rewardExp;

    public Button navButton;
    private int npc = 0;


	// Use this for initialization
	void Start () {
		
	}
	
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if(this.description != null)
        {
            if (quest.Info == null)
            {
                this.description.text = quest.Define.Dialog;
                ;
            }
            else
            {
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.Finished)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
                if (quest.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
                {
                    this.description.text = quest.Define.DialogIncomplete;
                }

            }
        }
        

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        if(quest.Info == null)
        {
            this.npc = quest.Define.AcceptNPC;
        }
        else if(quest.Info.Status == SkillBridge.Message.QuestStatus.Completed)
        {
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(this.npc > 0);

        foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickAbandon()
    {

    }

    public void OnClickNav()
    {
        Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StartNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }
}
