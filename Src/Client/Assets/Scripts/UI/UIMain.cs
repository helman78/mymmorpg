using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {
	public Text UIAvatarName;
	public Text UIAvatarLevel;

	// Use this for initialization
	protected override void OnStart () {
		this.UpdateAvatar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateAvatar()
    {
		this.UIAvatarName.text = User.Instance.CurrentCharacter.Name;
		this.UIAvatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

	public void BackToCharSelect()
    {
		SceneManager.Instance.LoadScene("CharSelect");
		UserService.Instance.SendGameLeave();
    }
	public void OnClickBag()
    {
		UIManager.Instance.Show<UIBag>();
    }
}
