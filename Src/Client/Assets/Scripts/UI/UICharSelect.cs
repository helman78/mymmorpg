using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;

public class UICharSelect : MonoBehaviour {

	public GameObject selectPanel;
	public GameObject createPanel;
	public UICharView charView;
	public Button startButton;
    public Transform UICharList;
    public GameObject UICharInfo;
	public List<GameObject> chars;
    private int selectCharIdx = -1;

	// Use this for initialization
	void Start () {
        InitSelect();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void InitSelect()
    {
        foreach (var old in chars)
        {
            Destroy(old);
        }
        chars.Clear();

        createPanel.SetActive(false);
        selectPanel.SetActive(true);

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            GameObject go = Instantiate(UICharInfo, UICharList);
            UICharInfo chrInfo = go.GetComponent<UICharInfo>();
            chrInfo.charInfo = User.Instance.Info.Player.Characters[i];

            Button button = chrInfo.charButton;
            int idx = i;
            button.onClick.AddListener(() => {
                OnSelectChar(idx);
            });

            chars.Add(go);
            go.SetActive(true);
        }

        GameObject createGo = Instantiate(UICharInfo, UICharList);
        createGo.GetComponent<UICharInfo>().charButton.onClick.AddListener(() =>
        {
            createPanel.SetActive(true);
            selectPanel.SetActive(false);
        });
        chars.Add(createGo);
        createGo.SetActive(true);
    }
    public void OnSelectChar(int idx)
    {
        this.selectCharIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        charView.CurChar = (int)cha.Class - 1;
        for(int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo chrInfo = chars[i].GetComponent<UICharInfo>();
            chrInfo.Selected = idx == i;
        }
    }
    public void OnGameEnterClick()
    {
        if(this.selectCharIdx >= 0)
        {
            UserService.Instance.SendGameEnter(this.selectCharIdx);
        }
    }
}
