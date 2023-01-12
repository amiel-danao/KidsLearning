using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace KidsLearning
{
    public class PuzzleTimer : MonoBehaviour, IComponent
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private IGameManager _gameManager;
        public float interval = 1.0f;
        public bool repeat = false;
        public Action<float> BeforeResetTimerEvent;
        private float elapsedTime = 0.0f;
        private bool _start = false;
        [SerializeField] private string _gameManagerName = "GameManager";
        [SerializeField] private string _dependentToName = "GameManager";
        public IComponent DependentTo { get; set; }
        public Action DoneInitialization { get; set; }

        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            var appManager = GameObject.Find(_gameManagerName);

            _gameManager = appManager.GetComponent<IGameManager>();

            UpdateTimerText();
            if (_gameManager != null)
            {
                _gameManager.CorrectAnsweredEvent += (answer, wrongAnswers) =>
                {
                    _start = false;
                    BeforeResetTimerEvent?.Invoke(elapsedTime);
                };
                _gameManager.BeginPuzzleEvent += ResetTimer;
            }
            DoneInitialization?.Invoke();
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
            UpdateTimerText();
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