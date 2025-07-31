using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Services;
using Models;

public class UICharCreate : MonoBehaviour {

	public GameObject selectPanel;
	public GameObject createPanel;
	public InputField charName;
	public UICharView charView;
	public GameObject[] titles;
	public Text desc;
	public UICharSelect uSelect;
	CharacterClass charClass = CharacterClass.Archer;


    // Use this for initialization
    void Start()
    {
		UserService.Instance.OnCreate = this.OnCreate;
    }

	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSelectChar(int charClass)
    {
		this.charClass = (CharacterClass)charClass;
		charView.CurChar = charClass - 1;
		for(int i = 0; i < 3; i++)
        {
			titles[i].SetActive(i == charClass - 1);
        }
		desc.text = DataManager.Instance.Characters[charClass].Description;
    }
	void OnCreate(Result result, string msg)
	{
		if (result == SkillBridge.Message.Result.Success)
		{
			Debug.Log("OnCreate");
			uSelect.InitSelect();
        }
		else
			MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
	}
	public void OnCreateClick()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
			MessageBox.Show("请输入昵称");
			return;
        }
		UserService.Instance.SendCharCreate(this.charName.text, this.charClass);
    }
}
