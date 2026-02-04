using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    float desiredAlpha = 0;
    float currentAlpha = 1.0f;

    [SerializeField]
    float fadeSpeed = 1.0f;
    [SerializeField]
    Image fadeImage;

    public bool Fading { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, fadeSpeed * Time.deltaTime);

        Color tempColor = fadeImage.color;
        tempColor.a = currentAlpha;
        fadeImage.color = tempColor;

        Fading = currentAlpha != desiredAlpha;
    }

    public void SetDesiredAlpha(float a)
    {
        desiredAlpha = a;
        Fading = true;
    }
}
