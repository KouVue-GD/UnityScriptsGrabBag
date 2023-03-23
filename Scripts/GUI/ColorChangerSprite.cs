using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//generated by chatgpt on openai
public class ColorChangerSprite : MonoBehaviour
{
    public float colorChangeDuration = 2f; // The duration of the color change in seconds
    public List<Color> colorsToChangeTo;

    private Image image; // Reference to the SpriteRenderer component

    private void Start()
    {
        image = GetComponent<Image>(); // Get the SpriteRenderer component on this GameObject
        StartCoroutine(ChangeColorOverTime()); // Start the color change Coroutine
    }

    private IEnumerator ChangeColorOverTime()
    {
        Color startColor = image.color; // Get the initial color of the SpriteRenderer
        Color endColor = colorsToChangeTo[Random.Range(0, colorsToChangeTo.Count)]; // Create a random color to transition to

        float timeElapsed = 0f; // The amount of time that has elapsed since the color change started

        while (timeElapsed < colorChangeDuration)
        {
            // Lerp between the start color and end color over time
            image.color = Color.Lerp(startColor, endColor, timeElapsed / colorChangeDuration);

            timeElapsed += Time.deltaTime; // Increment the time elapsed

            yield return null; // Wait for the next frame
        }

        // Start a new color change Coroutine
        StartCoroutine(ChangeColorOverTime());
    }
}
