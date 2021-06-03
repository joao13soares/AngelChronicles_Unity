using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatherAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] heartSprites;

    private int currentFps = 24;

    private Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        heartImage.sprite = heartSprites[(int)(Time.time * currentFps) % heartSprites.Length];
    }

    
}
