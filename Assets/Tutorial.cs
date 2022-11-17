// Aaron Williams
// 11/16/2022

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    // variables
    [SerializeField] private GameObject[] slides;
    private int slideIndex;
    [SerializeField] private TextMeshProUGUI nextButtonA;
    [SerializeField] private TextMeshProUGUI nextButtonB;
    [SerializeField] private TextMeshProUGUI backButtonB;

    // methods
    void Start()
    {
        slideIndex = 0;
        ActivateSlideAtIndex();
    }

    void NextSlide()
    {
        if(slideIndex == slides.Count() - 1)
        {
            slideIndex = 0;

        }
        slideIndex++;
        ActivateSlideAtIndex();
    }
    void PreviousSlide()
    {
        slideIndex--;
        ActivateSlideAtIndex();
    }
    void ActivateSlideAtIndex()
    {
        for (int i = 0; i < slides.Count(); i++)
        {
            if (i == slideIndex)
            {
                slides[i].SetActive(true);
            }
            else
            {
                slides[i].SetActive(false);
            }
        }
    }
}
