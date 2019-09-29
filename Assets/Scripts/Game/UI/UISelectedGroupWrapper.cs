﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedGroupWrapper : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI unitsCount;

    [SerializeField] private Image _highlightFrame;

    void Start()
    {
        SetHighlight(false);
    }

    public void SetHighlight(bool isHighlight)
    {
        _highlightFrame.gameObject.SetActive(isHighlight);
    }
}
