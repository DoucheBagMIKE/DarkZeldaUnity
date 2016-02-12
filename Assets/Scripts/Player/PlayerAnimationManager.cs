﻿using UnityEngine;
using System.Collections;

public class PlayerAnimationManager : MonoBehaviour {

    public Animator anim;

    public Rigidbody2D player;
    public bool isAttacking;
    public GameObject imagePoint;
    public bool lockDirWhenAttacking;

    private Pause pauseScript;
    private float lockTimer;

    float FloorOrCeil (float val)
    {
        if(val > 0)
        {
            return Mathf.Ceil(val);
        }
        else
        {
            return Mathf.Floor(val);
        }
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        pauseScript = FindObjectOfType<Pause>();
        anim.SetFloat("lastY", -1f);
        anim.SetFloat("lastX", 0f);
    }
	
	// Update is called once per frame
	void Update () {
        if(pauseScript.paused)
        {
            anim.SetFloat("curX", 0f);
            anim.SetFloat("curY", 0f);
            anim.SetBool("isMoving", false);
            return;
        }

        if (player.velocity.magnitude == 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }

        if (isAttacking)
        {
            anim.SetTrigger("isAttacking");
            isAttacking = false;
        }

        if (lockDirWhenAttacking && (Input.GetButton("Fire1") || Input.GetButton("Fire2")))
        {
            lockTimer = 0.25f;
        }
        if(lockTimer > 0)
        {
            lockTimer -= Time.deltaTime;
            return;
        } else
        {
            lockTimer = 0f;
        }
        anim.SetFloat("curX", Input.GetAxis("Horizontal"));
        anim.SetFloat("curY", Input.GetAxis("Vertical"));

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {

            if(Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Abs(Input.GetAxis("Vertical")))
            {
                anim.SetFloat("lastX", FloorOrCeil(Input.GetAxis("Horizontal")));
                anim.SetFloat("lastY", 0f);
            }
            else
            {
                anim.SetFloat("lastX", 0f);
                anim.SetFloat("lastY", FloorOrCeil(Input.GetAxis("Vertical")));
            }
        }

        imagePoint.transform.position = player.transform.position + new Vector3(anim.GetFloat("lastX"), anim.GetFloat("lastY")).normalized/2;
    }
}
