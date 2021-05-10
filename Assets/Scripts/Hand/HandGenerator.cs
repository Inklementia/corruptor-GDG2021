﻿using System.Collections;
using DG.Tweening;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hand
{
    public class HandGenerator : MonoBehaviour
    {
        [SerializeField] private HandStruct[] hands;
        [SerializeField] private float handMovementInterval = 2;
        [SerializeField] private float handMovementTime = .5f;

        [SerializeField] private float handStayDuration = .5f;
        [SerializeField] private float blockDurationForMent = 3;
        [SerializeField] private LevelController levelController;
        [SerializeField] private GameObject jailPanelGO;
        [SerializeField] private Animator jailAnimator;

        private int _index;

        public bool _isBlocked;
        private bool _isBlockedByMent;

        private bool _canGoBack;

        private bool _canMoveHands = true;

        private float _elapsedMoveTime = 0.0f;
        private float _elapsedWaitTime = 0.0f;
        private float _elapsedBlockTime = 0.0f;

        private float _elapsedHandGeneratorBlockTime = 0.0f;
        private float _handGeneratorBlockTime = 1f;

        #region Cache

        private static readonly int MoveUp = Animator.StringToHash("MoveUp");

        #endregion


        private void Update()
        {
            _elapsedMoveTime += Time.deltaTime;

            if (_canMoveHands)
            {
                if (!_isBlocked && !_isBlockedByMent)
                {
                    MoveHandForward();
                }

                else if (_isBlockedByMent)
                {
                    ShowJail();
                }
            }

            MoveHandBack();
        }

        public void UnblockHandGenerator()
        {
            _isBlockedByMent = false;
            _isBlocked = false;
        }

        public void UnblockHandGeneratorAfterWait()
        {
            StartCoroutine(WaitAndUnblock(1f));
        }

        private IEnumerator WaitAndUnblock(float time)
        {
            hands[_index].handGO.SetActive(true);
            yield return new WaitForSeconds(time);
            _isBlocked = false;
        }

        public void BlockHandGenerator()
        {
            _isBlocked = true;
            hands[_index].handGO.SetActive(false);
            Debug.Log("Should lock generaor");
        }

        public void BlockHandGeneratorByMent()
        {
            _isBlockedByMent = true;
            _isBlocked = true;
        }

        public void StopHands()
        {
            Debug.Log("Stop Hands");
            _canMoveHands = false;
        }

        public void MoveHands()
        {
            Debug.Log("Stop Hands");
            _canMoveHands = true;
        }

        private void ShowJail()
        {
            jailPanelGO.SetActive(true);
            _elapsedBlockTime += Time.deltaTime;

            if (_elapsedBlockTime >= blockDurationForMent)
            {
                Debug.Log("MoveUP");
                _elapsedBlockTime = 0.0f;
                jailAnimator.SetTrigger(MoveUp);
                _isBlocked = false;
            }
        }

        public void DeactivateJail()
        {
            _isBlockedByMent = false;
            jailPanelGO.SetActive(false);
        }

        private void MoveHandForward()
        {
            if (_elapsedMoveTime >= handMovementInterval && _canMoveHands)
            {
                _canMoveHands = false;
                _index = Random.Range(0, hands.Length);
                // go to target
                hands[_index].handGO.transform.DOMove(hands[_index].target.position, handMovementTime)
                    .SetEase(Ease.OutCubic).OnComplete(() => { _canGoBack = true; });
            }
        }

        private void MoveHandBack()
        {
            // wait time
            if (_canGoBack)
            {
                _elapsedWaitTime += Time.deltaTime;

                if (_elapsedWaitTime >= handStayDuration)
                {
                    // go to initial position
                    hands[_index].handGO.transform.DOMove(hands[_index].initialPosition.position, handMovementTime)
                        .OnComplete(
                            () =>
                            {
                                hands[_index].cashGO.SetActive(true);
                                hands[_index].cashGO.GetComponent<Cash.Cash>().CashCanBeTaken();
                                _canMoveHands = true;
                            });
                    _elapsedMoveTime = 0.0f;
                    _elapsedWaitTime = 0.0f;
                    _canGoBack = false;
                }
            }
        }

        public void OnLevelUp()
        {
            // accelerates hand movement speed according to current level
            // called in level controller
            handMovementInterval -= 0.066f;
            handMovementTime -= 0.066f;
            handStayDuration -= 0.066f;
        }
    }
}