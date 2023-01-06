using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;


namespace KidsLearning
{
    [RequireComponent(typeof(ArithmeticGenerator))]
    public class QuestionLogic : MonoBehaviour
    {
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private ArithmeticGenerator _arithmeticGenerator;
        private AnswerParts[] _allAnswerParts;
        private List<Arithmetic> _questions = new List<Arithmetic>();
        private int _currentQuestionIndex = -1;

        public Action<AnswerParts> CorrectAnsweredEvent;
        public Action LevelFinishedEvent;
        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private AudioClip _correctSound;
        [SerializeField] private AudioClip _yaySound;
        [SerializeField] private GameObject _congratsPanel;

        void Awake()
        {
            _allAnswerParts = transform.GetComponentsInChildren<AnswerParts>();
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

        void Start()
        {
            iTween.ScaleTo(_questionText.gameObject, iTween.Hash(
                "scale", Vector3.one * 0.8f,
                "time", 1f,
                "looptype", "pingpong",
                "easetype", iTween.EaseType.easeInOutCubic
            ));
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

        public void NextQuestion()
        {
            int previousIndex = _currentQuestionIndex;
            _currentQuestionIndex++;
            _questionText.text = string.Empty;

            try
            {
                var previousAnswer = _allAnswerParts[previousIndex];
                _soundEffect.PlayOneShot(_correctSound);
                CorrectAnsweredEvent?.Invoke(previousAnswer);
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
                }
                else
                {
                    _soundEffect.PlayOneShot(_yaySound);
                    LevelFinishedEvent?.Invoke();
                    _congratsPanel.SetActive(true);
                    Debug.Log("Level Finished!");
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Debug.LogError(exception.ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}