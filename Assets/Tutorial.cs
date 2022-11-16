// Aaron Williams
// 11/16/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] slides;
    private GameObject currentSlide;

    // Start is called before the first frame update
    void Start()
    {
        currentSlide = slides[0];
    }

    void NextSlide()
    {
        //currentSlide.SetActive;
    }
}
