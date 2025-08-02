using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using Models;

public class NPCController : MonoBehaviour {

	public int npcID;
	private bool inInteractive = false;

	SkinnedMeshRenderer renderer;
	Animator anim;
	Color originColor;
	NPCDefine npc;

	// Use this for initialization
	void Start () {
		renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
		originColor = renderer.sharedMaterial.color;
		npc = NPCManager.Instance.GetNPCDefine(this.npcID);
		this.StartCoroutine(Actions());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Actions()
    {
        while (true)
        {
			if (inInteractive)
				yield return new WaitForSeconds(2f);
			else yield return new WaitForSeconds(Random.Range(5f, 10f));
			this.Relax();
        }
    }
	void Relax()
    {
		anim.SetTrigger("Relax");
    }
	void Interactive()
    {
        if (!inInteractive)
        {
			inInteractive = true;
			StartCoroutine(DoInteractive());
        }
    }
	IEnumerator DoInteractive()
    {
		yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
			anim.SetTrigger("Talk");
        }
		yield return new WaitForSeconds(3f);
		inInteractive = false;
    }
	IEnumerator FaceToPlayer()
    {
		Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
		faceTo.y = 0;
		while(Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
			this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
			yield return null;
		}
    }
	void OnMouseDown()
    {
		Interactive();
	}
	void OnMouseOver()
    {
		Highlight(true);
    }
	void OnMouseEnter()
	{
		Highlight(true);
	}
	void OnMouseExit()
	{
		Highlight(false);
	}
	void Highlight(bool highlight)
    {
        if (highlight)
        {
			if (renderer.sharedMaterial.color != Color.white)
				renderer.sharedMaterial.color = Color.white;
        }
        else
        {
			if (renderer.sharedMaterial.color != originColor)
				renderer.sharedMaterial.color = originColor;
        }
    }
}
