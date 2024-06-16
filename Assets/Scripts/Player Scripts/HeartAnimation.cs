using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeartAnimation : MonoBehaviour
{
    public Sprite[] heartSprites; // Array to hold the different heart sprites
    public float animationSpeed = 0.1f; // Speed of the animation
    public float startDelay = 1f; // Delay before the animation starts
    public float loopDelay = 1f; // Delay after each animation loop

    private Image heartImage;

    void Start()
    {
        heartImage = GetComponent<Image>();
        StartCoroutine(AnimateHeart());
    }

    IEnumerator AnimateHeart()
    {
        // delay before starting the animation
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // Iterate through each sprite in the array
            for (int i = 0; i < heartSprites.Length; i++)
            {
                heartImage.sprite = heartSprites[i];
                yield return new WaitForSeconds(animationSpeed);
            }

            // Delay after completing the loop
            yield return new WaitForSeconds(loopDelay);
        }
    }
}
