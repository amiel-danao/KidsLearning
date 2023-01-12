using KidsLearning.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning
{
    public class ScoreSaver : MonoBehaviour, IScoreKeeper
    {
        [SerializeField] private IGameManager _gameManager;
        [SerializeField] private PuzzleTimer _puzzleTimer;
        [Tooltip("_minScore is minimum score a player can get even if it messes up so badly")]
        [Range(0, 5)]
        [SerializeField] private int _minScore = 5;
        [Tooltip("_maxScore is maximum score a player can get.")]
        [Range(5, 10)]
        [SerializeField] private int _maxScore = 10;
        [Tooltip("_maxTime is the seconds when the score is already at _minScore")]
        [Range(20f, 60f)]
        [SerializeField] private float _maxTime = 60f;
        [Tooltip("_minTime is the seconds when a score can be at _maxScore")]
        [Range(1f, 20f)]
        [SerializeField] private float _minTime = 20f;
        [SerializeField] private string _gameManagerName = "GameManager";
        private RESTConnection _connection;
        private float _lastTimerTime;
        private int _totalScore = 0;
        public int TotalScore => _totalScore;
        void Start()
        {
            _puzzleTimer.BeforeResetTimerEvent += (time) => _lastTimerTime = time;
            _connection = FindObjectOfType<RESTConnection>();
            if (_connection != null)
            {
                Debug.Log(_connection.GetUserId());
                var appManager = GameObject.Find(_gameManagerName);

                _gameManager = appManager.GetComponent<IGameManager>();
                if (_gameManager != null)
                {
                    _gameManager.CorrectAnsweredEvent += OnCorrectAnswerEvent;
                }
            }
        }

        private void OnCorrectAnswerEvent(IQuestion arithmetic, List<string> wrongAnswers)
        {
            int score = ComputeScore(_lastTimerTime, wrongAnswers.Count);
            _totalScore += score;
            string summary = ConstructSummary(arithmetic, wrongAnswers);
            _connection.TrySaveScore(score, _lastTimerTime, summary);
        }
        public virtual string ConstructSummary(IQuestion question, List<string> wrongAnswers)
        {
            return string.Empty;
        }

        private int ComputeScore(float time, int wrongAnswersCount)
        {
            var clampedTime = Math.Clamp(time, _minTime, _maxTime);
            var timeRangePercentage = 100 - PercentageBetweenTwoNumbers(clampedTime, _minTime, _maxTime);
            int score = (int)FindScoreFromTime(timeRangePercentage, _minScore, _maxScore);

            return Math.Max(_minScore, score - wrongAnswersCount);
        }

        public double PercentageBetweenTwoNumbers(double value, double minNumber, double maxNumber)
        {
            return (value - minNumber) / (maxNumber - minNumber) * 100;
        }

        public double FindScoreFromTime(double percentage, double minimum, double maximum)
        {
            return percentage * (maximum - minimum) / 100 + minimum;
        }
    }
}