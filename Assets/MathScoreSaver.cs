using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace KidsLearning.Assets
{
    public class MathScoreSaver : MonoBehaviour
    {
        [SerializeField] private QuestionLogic _questionLogic;
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
        private RESTConnection _connection;
        private float _lastTimerTime;

        void Start()
        {
            _puzzleTimer.BeforeResetTimerEvent += (time) => _lastTimerTime = time;
            _connection = FindObjectOfType<RESTConnection>();
            if (_connection != null)
            {
                Debug.Log(_connection.GetUserId());
                _questionLogic.CorrectAnsweredEvent += OnCorrectAnswerEvent;
            }
        }

        private void OnCorrectAnswerEvent(Arithmetic arithmetic)
        {
            int score = ComputeScore(_lastTimerTime);
            string summary = ConstructSummary(arithmetic);
            _connection.TrySaveScore(score, _lastTimerTime, summary);
        }

        private string ConstructSummary(Arithmetic arithmetic)
        {
            string summary = $"Solving {arithmetic}";

            return summary;
        }

        private int ComputeScore(float time)
        {
            int score = _maxScore;
            var clampedTime = Math.Clamp(time, _minTime, _maxTime);
            var timeRangePercentage = PercentageBetweenTwoNumbers(clampedTime, _minTime, _maxTime);
            score = (int)FindScoreFromTime(timeRangePercentage, _minScore, _maxScore);

            return score;
        }

        public double PercentageBetweenTwoNumbers(double number1, double number2, double number3)
        {
            return number1 / (number2 + number3) * 100;
        }

        public double FindScoreFromTime(double percentage, double minimum, double maximum)
        {
            return percentage * (maximum - minimum) / 100 + minimum;
        }


    }
}