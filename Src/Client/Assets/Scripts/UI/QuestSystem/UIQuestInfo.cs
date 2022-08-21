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


	// Use this for initialization
	void Start () {
		
	}
	
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if(quest.Info == null)
        {
            this.description.text = quest.Define.Dialog;
;       }
        else
        {
            if(quest.Info.Status == SkillBridge.Message.QuestStatus.Finished)
            {
                this.description.text = quest.Define.DialogFinish;
            }
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
            {
                this.description.text = quest.Define.DialogIncomplete;
            }

        }

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

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
}
