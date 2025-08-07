using Common.Data;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSystem : UIWindow {

	public Text title;
	public GameObject itemPrefab;
	public TabView tabs;
    public ListView listMain;
	public ListView listBranch;
	public UIQuestInfo questInfo;

	private bool showAvailableList = false;

	// Use this for initialization
	void Start () {
		this.listMain.onItemSelected += this.OnQuestSelected;
		this.listBranch.onItemSelected += this.OnQuestSelected;
		this.tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
        QuestManager.Instance.onQuestStatusChanged += RefreshUI;
    }

    void OnSelectTab(int idx)
    {
		showAvailableList = idx == 1;
		RefreshUI();
    }
	private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= RefreshUI;
    }
    void RefreshUI()
    {
		ClearAllQuestList();
		InitAllQuestItems();
    }

	void ClearAllQuestList()
	{
		this.listMain.RemoveAll();
		this.listBranch.RemoveAll();
	}
	void InitAllQuestItems()
    {
		foreach(var kv in QuestManager.Instance.allQuests)
        {
            //如果任务已完成，则不显示
            if (kv.Value.Info != null && kv.Value.Info.Status == SkillBridge.Message.QuestStatus.Finished)
                continue;
            if (showAvailableList)
            {
				if (kv.Value.Info != null)
					continue;
            }
            else
            {
				if (kv.Value.Info == null)
					continue;
            }
			GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? listMain.transform : listBranch.transform);
			UIQuestItem ui = go.GetComponent<UIQuestItem>();
			ui.SetQuestInfo(kv.Value);
			if (kv.Value.Define.Type == QuestType.Main)
				this.listMain.AddItem(ui as ListView.ListViewItem);
			else
				this.listBranch.AddItem(ui as ListView.ListViewItem);
        }
    }
	public void OnQuestSelected(ListView.ListViewItem item)
    {
		UIQuestItem questItem = item as UIQuestItem;
		this.questInfo.SetQuestInfo(questItem.quest);
    }
}
