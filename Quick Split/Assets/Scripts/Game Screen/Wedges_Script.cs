using UnityEngine;
using System.Collections;

public class Wedges_Script : MonoBehaviour {

	//this script is for the wedges that make the splits on the splitter

	Animator animator;
	SpriteRenderer spriteRenderer;
	float animStartTime;
	bool hasPlayedAnim = false;

	string SplitterType;
	int index;

	public Sprite[] IdleSprites;
	public RuntimeAnimatorController[] wedgeAnimators;

	Achievement_Script achievementHandler;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		achievementHandler = GameObject.Find ("Achievement Handler").GetComponent<Achievement_Script> ();
		SplitterType = PlayerPrefs.GetString ("Splitter Type", "Default");
		if (SplitterType == "Programmer") {
			Destroy(gameObject);
			return;
		}
		index = achievementHandler.Splitter_Lookup_Index_by_Name (SplitterType);
		animator.runtimeAnimatorController = wedgeAnimators [index];
		spriteRenderer.sprite = IdleSprites [index];
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(hasPlayedAnim && animator.GetCurrentAnimatorStateInfo(0).length + animStartTime < Time.time){
			float animPlayLength = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * (1/gameObject.GetComponent<Animator>().speed);
			if(animPlayLength + animStartTime < Time.time){
				gameObject.GetComponent<Animator>().SetBool("isFiring", false);
				hasPlayedAnim = false;
				spriteRenderer.sprite = IdleSprites [index];
			}
		}
	}

	//begins the firing animations
	public void Has_Fired()
	{
		animator.SetBool ("isFiring", true);
		animStartTime = Time.time;
		hasPlayedAnim = true;
	}

}