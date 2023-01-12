using System.Collections;
using TMPro;
using UnityEngine;

namespace KidsLearning.Assets
{
    public class SummaryUI : MonoBehaviour
    {
        private IGameManager _gameManager;
        [SerializeField] private TMP_Text _scoreSummary;
        [SerializeField] private IScoreKeeper _scoreKeeper;
        [SerializeField] private Transform _summaryEntriesContainer;
        [SerializeField] private GameObject _summaryEntryPrefab;
        [SerializeField] private string _gameManagerName = "GameManager";

        void Start()
        {
            var appManager = GameObject.Find(_gameManagerName);

            _gameManager = appManager.GetComponent<IGameManager>();
            _scoreKeeper = appManager.GetComponent<IScoreKeeper>();
            _gameManager.LevelFinishedEvent += OnLevelFinishedEvent;
        }

        private void CreateSummary()
        {
            var summaryCorrect = Instantiate(_summaryEntryPrefab, _summaryEntriesContainer).GetComponent<TMP_Text>();
            summaryCorrect.text = $"Number of correct answers: {_gameManager.AllCorrectAnswers.Count}";

            foreach (var item in _gameManager.AllCorrectAnswers)
            {
                var correctAnswer = Instantiate(_summaryEntryPrefab, _summaryEntriesContainer).GetComponent<TMP_Text>();
                correctAnswer.color = Color.green;
                correctAnswer.text = item;
            }

            var summaryWrong = Instantiate(_summaryEntryPrefab, _summaryEntriesContainer).GetComponent<TMP_Text>();
            summaryWrong.text = $"Number of wrong answers: {_gameManager.AllWrongAnswers.Count}";

            foreach (var item in _gameManager.AllWrongAnswers)
            {
                var wrongAnswer = Instantiate(_summaryEntryPrefab, _summaryEntriesContainer).GetComponent<TMP_Text>();
                wrongAnswer.color = Color.red;
                wrongAnswer.text = item;
            }
        }

        private void OnLevelFinishedEvent()
        {
            var originalScoreSummary = _scoreSummary.text;
            _scoreSummary.text = $"{originalScoreSummary} {_scoreKeeper.TotalScore}";

            CreateSummary();
        }
    }
}