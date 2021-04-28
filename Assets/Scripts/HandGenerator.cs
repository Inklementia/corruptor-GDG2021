﻿using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HandGenerator : MonoBehaviour
{
    [SerializeField] private HandStruct[] hands;
    [SerializeField] private float duration = 2;

    private int _minIndex = 0;
    private int _maxIndex;


    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var handomIndex = Random.Range(0, hands.Length);
            // activate bribe
            hands[handomIndex].bribeGO.SetActive(true);
            MoveHand(handomIndex);
        }
    }

    private void MoveHand(int index)
    {
        hands[index].handGO.transform.DOMove(hands[index].target.position, duration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                hands[index].handGO.transform.DOMove(hands[index].initialPosition.position, duration);
                // not working properly
               
            });
    }
}