using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource playerSound;

    public AudioClip[] stepsLeft;
    public AudioClip[] stepsRight;

    private float stepCountdown;
    public float stepGap = 1f;
    private bool nextStepRight = true;
    private int randomClip;

    // Start is called before the first frame update
    void Start()
    {
        stepCountdown = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WalkSound()
    {
        if (Time.time - stepCountdown > stepGap)
        {
            randomClip = Random.Range(0, stepsLeft.Length);

            if (nextStepRight)
            {
                playerSound.clip = stepsRight[randomClip];
            }
            else
            {
                playerSound.clip = stepsLeft[randomClip];
            }

            playerSound.Play();
            stepCountdown = Time.time;
        }
    }
}
