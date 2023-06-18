using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource playerSound;

    [SerializeField] private AudioClip[] stepsLeft;
    [SerializeField] private AudioClip[] stepsRight;

    private int randomClip;
    private bool nextStepRight = true;
    private float stepCountdown;
    private float stepGap = 1f;

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
        // stepGap is the delay between footstep sounds
        if (Time.time - stepCountdown > stepGap)
        {
            // Pick a random footstep sound
            randomClip = Random.Range(0, stepsLeft.Length);

            // Switch between left and right step sounds
            if (nextStepRight)
            {
                playerSound.clip = stepsRight[randomClip];
            }
            else
            {
                playerSound.clip = stepsLeft[randomClip];
            }

            // Play step sound and reset step countdown for stepGap
            playerSound.Play();
            stepCountdown = Time.time;
        }
    }
}
