﻿using Hand;
using Money;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class PhonePickUp : MonoBehaviour
{
    private HandGenerator _handGenerator;
    private AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _handGenerator = FindObjectOfType<HandGenerator>(); 
       // do we need to block hand generator when we talk to secretary?
  
    }

    private bool _isPickedUp;

    private void OnMouseDown()
    {
        _isPickedUp = true;
        _handGenerator.BlockHandGenerator(); 
        
        _audioManager.Play("phonePickUp");
        _audioManager.Play("secretaryTalk");
        // pickUpSound.Play();

        //   talkSound.Play();

    }
    public bool CheckIfPhoneIsPickedUp()
    {
        return _isPickedUp;
    }
    public void ResetPhone()
    {
        _isPickedUp = false;
        _handGenerator.UnblockHandGenerator();
        _audioManager.Stop("secretaryTalk");
    }
    //TODO If not taken -200$
}
