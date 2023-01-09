using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KidsLearning.Assets
{
    public class PuzzleTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private QuestionLogic _questionLogic;
        [SerializeField] private WordGame _wordGame;
        public float interval = 1.0f;
        public bool repeat = false;
        public Action<float> BeforeResetTimerEvent;
        private float elapsedTime = 0.0f;
        private bool _start = false;

        private void Start()
        {
            UpdateTimerText();
            if (_questionLogic != null)
            {
                _questionLogic.BeginPuzzleEvent += ResetTimer;
                _questionLogic.CorrectAnsweredEvent += (answer, wrongAnswers) =>
                {
                    _start = false;
                    BeforeResetTimerEvent?.Invoke(elapsedTime);
                };
            }

            if (_wordGame != null)
            {
                _wordGame.BeginPuzzleEvent += ResetTimer;
                _wordGame.CorrectAnsweredEvent += (answer) =>
                {
                    _start = false;
                    BeforeResetTimerEvent?.Invoke(elapsedTime);
                };
            }
        }

        private void UpdateTimerText()
        {
            _text.text = FormatTime((int)elapsedTime);
        }

        public static string FormatTime(int timeInSeconds)
        {
            int hours = timeInSeconds / 3600;
            int minutes = (timeInSeconds % 3600) / 60;
            int seconds = timeInSeconds % 60;
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }


        private void ResetTimer(bool autoStart)
        {
            elapsedTime = 0;
            if (autoStart)
            {
                _start = true;
            }
        }

        void Update()
        {
            if (_start)
            {
                elapsedTime += Time.deltaTime;
                UpdateTimerText();
            }
        }
    }
}