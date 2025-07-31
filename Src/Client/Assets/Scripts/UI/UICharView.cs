using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharView : MonoBehaviour {

	public GameObject[] chars;
	private int curChar = 0;
	public int CurChar
    {
        get
        {
			return curChar;
        }
        set
        {
			curChar = value;
			this.UpdateCur();
        }
    }
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 3; i++)
        {
			chars[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void UpdateCur()
    {
		for(int i = 0; i < 3; i++)
        {
			chars[i].SetActive(i == this.curChar);
        }
    }
}
