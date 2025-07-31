using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {

	public SkillBridge.Message.NCharacterInfo charInfo;
	public Text charName;
	public Text charClassText;
	public Image charImg;
	public Button charButton;

	public Sprite[] classImg, classHighImg;

	private bool highlight = false;
	private CharacterClass charClass;

	// Use this for initialization
	void Start () {
        if (charInfo != null)
        {
			charClass = charInfo.Class;
			charName.text = charInfo.Name;
			charClassText.text = charClass.ToString();
			charImg.sprite = classImg[(int)charClass];
        }
        else
        {
			charClass = CharacterClass.None;
			charName.text = "创建新角色";
			charClassText.text = "";
			charImg.sprite = classImg[0];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public bool Selected
	{
		get { return highlight; }
		set
		{
			highlight = value;
			if (highlight)
				charImg.sprite = classHighImg[(int)charClass];
			else charImg.sprite = classImg[(int)charClass];
		}
	}
}
