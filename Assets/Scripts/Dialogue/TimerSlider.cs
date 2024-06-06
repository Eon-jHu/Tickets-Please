using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{
    [SerializeField]
    public Slider timerSlider;

    [SerializeField]
    public Image barColor;

    [SerializeField]
    public float totalTime = 10.0f;

    private float remainingTime;

    void Start()
    {
        if (timerSlider != null)
        {
            remainingTime = totalTime;
            timerSlider.maxValue = totalTime;
            timerSlider.value = timerSlider.maxValue;
        }
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

        }
        else
        {
            // TODO: Finish logic with a loss;
        }
    }
}
