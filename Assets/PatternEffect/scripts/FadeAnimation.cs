using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeAnimation : MonoBehaviour
{
    public float speed = 0.1f;

    public Color FadeOut(Color originalColor)
    {
        Color newColor = originalColor;
        newColor.a = newColor.a - this.speed;

        return newColor;
    }

    public Color FadeIn(Color originalColor)
    {
        Color newColor = originalColor;
        newColor.a = newColor.a + this.speed;

        return newColor;
    }

    public Color AlphaZero(Color originalColor)
    {
        Color newColor = originalColor;
        newColor.a = 0;

        return newColor;
    }
}
