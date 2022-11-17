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

    // methods
    void Start()
    {
        slideIndex = 0;
        ActivateSlideAtIndex();
    }

    [ContextMenu("NextSlide")]
    [ContextMenu("PreviousSlide")]

    public void NextSlide()
    {
        if (slideIndex == slides.Count() - 1)
            slideIndex = 0;
        else
            slideIndex++;
        ActivateSlideAtIndex();
    }
    public void PreviousSlide()
    {
        if (slideIndex == 0)
            slideIndex = slides.Count()-1;
        else
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
