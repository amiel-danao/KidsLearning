using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace KidsLearning
{
    public class UiEffects : MonoBehaviour
    {
        [SerializeField] private Image _checkImage;
        [SerializeField] private WordGame _wordGame;

        void Start()
        {
            _wordGame.CorrectAnsweredEvent += OnCorrectAnswerEvent;
        }

        private void OnCorrectAnswerEvent(WordGuess wordGuess)
        {

            iTween.ScaleTo(_checkImage.gameObject, iTween.Hash(
                "scale", Vector3.one,
                "time", 0.5f,
                "easetype", iTween.EaseType.easeInOutCubic,
                "oncomplete", "ActionAfterTweenComplete", "oncompletetarget", gameObject
            ));



        }

        void ActionAfterTweenComplete()
        {
            iTween.ScaleTo(_checkImage.gameObject, iTween.Hash(
                "scale", Vector3.zero,
                "time", 0.4f,
                "easetype", iTween.EaseType.easeInOutCubic
            ));
        }
    }
}
