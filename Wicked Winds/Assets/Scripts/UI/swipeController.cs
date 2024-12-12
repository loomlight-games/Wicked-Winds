using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class swipeController : MonoBehaviour
{
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    [SerializeField] Image[] barImage;
    [SerializeField] Sprite barClosed, barOpen;


    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        UpdateBar();
    }
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();

        }
    }
    public void Previous()
    {
        if(currentPage > 1) 
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }
    void MovePage()
    {
        // Usamos un LeanTween con Time.unscaledDeltaTime
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime)
                      .setEase(tweenType)
                      .setIgnoreTimeScale(true);
        UpdateBar();
    }
    void UpdateBar()
    {
        foreach (var item in barImage)
        {
            item.sprite = barClosed;
        }
        barImage[currentPage-1].sprite = barOpen;
    }
}
