using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AnimationEvents : MonoBehaviour
{
    public Script_ControlManager associatePlayerScript;
    public AudioSource audioSourceComp;
    public AudioClip soundToPlay;

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void AllowMove()
    {
        associatePlayerScript.canMove = true;
    }

    public void Dig()
    {
        associatePlayerScript.Mine();
    }

    public void PlayASound()
    {
        audioSourceComp.PlayOneShot(soundToPlay);
    }
}
