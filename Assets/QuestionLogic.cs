using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;
using Random = System.Random;


namespace KidsLearning
{
    [RequireComponent(typeof(ArithmeticGenerator))]
    public class QuestionLogic : MonoBehaviour
    {
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private ArithmeticGenerator _arithmeticGenerator;
        [SerializeField] private Transform _levelsParent;
        private int _currentLevel = 0;
        private AnswerParts[] _allAnswerParts;
        private List<Arithmetic> _questions = new List<Arithmetic>();
        private int _currentQuestionIndex = -1;

        public Action<Arithmetic, List<string>> CorrectAnsweredEvent;
        public Action LevelFinishedEvent;
        public Action<bool> BeginPuzzleEvent;
        public Action ShapeFinishedEvent;
        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private AudioClip _correctSound;
        [SerializeField] private AudioClip _yaySound;
        [SerializeField] private GameObject _congratsPanel;
        private List<string> _wrongAnswers = new List<string>();

        void Awake()
        {
            
        }

        void Start()
        {
            iTween.ScaleTo(_questionText.gameObject, iTween.Hash(
                "scale", Vector3.one * 0.8f,
                "time", 1f,
                "looptype", "pingpong",
                "easetype", iTween.EaseType.easeInOutCubic
            ));
            AssembleLevel();
            NextQuestion();
        }

        public AnswerParts GetAnswerPart(AnswerChoice answerChoice)
        {
            return _allAnswerParts.Where(answer => answer.MyAnswerChoice == answerChoice).First();
        }

        public AnswerParts GetCurrentAnswer()
        {
            try
            {
                return _allAnswerParts[_currentQuestionIndex];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public RectTransform GetCurrentLevelTransform()
        {
            return _levelsParent.GetChild(_currentLevel).GetComponent<RectTransform>();
        }

        private void AssembleLevel()
        {
            var levelParent = GetCurrentLevelTransform();
            levelParent.gameObject.SetActive(true);
            //RandomizeChildren();
            _allAnswerParts = levelParent.GetComponentsInChildren<AnswerParts>();
            ShufflePartsToGuess();
            _questions.Clear();
            foreach (var answerPart in _allAnswerParts)
            {
                var problem = _arithmeticGenerator.GetRandomArithmeticProblem();

                while (_questions.Any(question => question.result == problem.result))
                {
                    problem = _arithmeticGenerator.GetRandomArithmeticProblem();
                }

                _questions.Add(problem);
                answerPart.SetAnswer(problem.result);
            }
        }

        public void NextQuestion()
        {
            
            int previousIndex = _currentQuestionIndex;
            _currentQuestionIndex++;
            _questionText.text = string.Empty;

            try
            {
                var previousAnswer = _allAnswerParts[previousIndex];
                _soundEffect.PlayOneShot(_correctSound);
                CorrectAnsweredEvent?.Invoke(_questions[previousIndex], _wrongAnswers);
            }
            catch (IndexOutOfRangeException)
            {

            }
            catch (NullReferenceException)
            {

            }

            try
            {
                if (_currentQuestionIndex < _questions.Count)
                {
                    string[] words = _questions[_currentQuestionIndex].ToString().Split('=');
                    _questionText.text = $"{words[0]} = ?";
                    _allAnswerParts[_currentQuestionIndex].SetAsCurrent();
                    BeginPuzzleEvent?.Invoke(true);
                }
                else
                {
                    int previousLevel = _currentLevel;
                    _currentLevel++;
                    if (_currentLevel < _levelsParent.childCount)
                    {
                        ShapeFinishedEvent?.Invoke();
                        _levelsParent.GetChild(previousLevel).gameObject.SetActive(false);
                        _currentQuestionIndex = -1;
                        AssembleLevel();
                        NextQuestion();
                        BeginPuzzleEvent?.Invoke(false);
                    }
                    else
                    {
                        _soundEffect.PlayOneShot(_yaySound);
                        LevelFinishedEvent?.Invoke();
                        _congratsPanel.SetActive(true);
                        Debug.Log("Level Finished!");
                        BeginPuzzleEvent?.Invoke(false);
                    }
                }
                _wrongAnswers.Clear();
            }
            catch (IndexOutOfRangeException exception)
            {
                Debug.LogError(exception.ToString());
            }
        }

        private void ShufflePartsToGuess()
        {
            Random rnd = new Random();
            int n = _allAnswerParts.Length;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = _allAnswerParts[k];
                _allAnswerParts[k] = _allAnswerParts[n];
                _allAnswerParts[n] = value;
            }
        }

        void RandomizeChildren()
        {
            var parent = GetCurrentLevelTransform().Find("answer_bg");
            int childCount = parent.childCount;
            if (childCount == 0) return;

            // Create a list of the children
            List<Transform> children = new();
            for (int i = 0; i < childCount; i++)
            {
                children.Add(parent.GetChild(i));
            }

            // Randomize the list of children
            System.Random rng = new();
            children = children.OrderBy(x => rng.Next()).ToList();

            // Re-parent the children in the randomized order
            for (int i = 0; i < childCount; i++)
            {
                children[i].SetParent(parent);
            }
        }
    }
}