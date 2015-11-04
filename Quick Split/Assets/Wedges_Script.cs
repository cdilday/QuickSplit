using UnityEngine;
using System.Collections;

public class Wedges_Script : MonoBehaviour {

	Animator animator;
	float animStartTime;
	bool hasPlayedAnim;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(hasPlayedAnim && animator.GetCurrentAnimatorStateInfo(0).length + animStartTime < Time.time)
		{
			float animPlayLength = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * (1/gameObject.GetComponent<Animator>().speed);
			if(animPlayLength + animStartTime < Time.time)
			{
				gameObject.GetComponent<Animator>().SetBool("isFiring", false);
				hasPlayedAnim = false;
			}
		}
	}

	public void Has_Fired()
	{
		animator.SetBool ("isFiring", true);
		animStartTime = Time.time;
		hasPlayedAnim = true;
	}
}
