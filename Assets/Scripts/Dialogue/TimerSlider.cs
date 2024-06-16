using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{

    public Slider timerSlider;
    public Image barColor;
    public float totalTime = 10.0f;
    public AudioManager audioManager;

    [NonSerialized]
    public float remainingTime;

    void OnEnable()
    {
        if (timerSlider != null)
        {
            remainingTime = totalTime;
            timerSlider.maxValue = totalTime;
            timerSlider.value = timerSlider.maxValue;
        }

        if (audioManager != null)
        {
            audioManager.HeartbeatSound.Play();
            audioManager.EarRinging.Play();
        }
    }

    private void OnDisable()
    {
        audioManager.HeartbeatSound.Stop();
        audioManager.EarRinging.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerSlider.value = remainingTime;

            // Change color based on closeness to 0;
            float halfTime = totalTime / 2.0f;
            if (remainingTime > halfTime)
            {
                barColor.color = Color.Lerp(Color.yellow, Color.green, (remainingTime - halfTime) / halfTime);
            }
            else
            {
                barColor.color = Color.Lerp(Color.red, Color.yellow, remainingTime / halfTime);
            }


            // Change volume based on closeness to 0;
            audioManager.EarRinging.volume = (1.0f - (remainingTime / totalTime)) / 4;
            audioManager.HeartbeatSound.volume = 1.0f - (remainingTime / totalTime);
        }
        else
        {
            remainingTime = 0;
            gameObject.SetActive(false);
        }
    }
}
