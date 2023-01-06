using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using KidsLearning.Assets.level3;
using Random = System.Random;

namespace KidsLearning
{
    public class WordGame : MonoBehaviour
    {
        [SerializeField] private List<WordGuess> _wordGuesses = new List<WordGuess>();
        [SerializeField] private Image _imageToGuess;
        [SerializeField] private GameObject _letterChoicePrefab;
        [SerializeField] private GameObject _letterPairPrefab;
        [SerializeField] private Transform _letterParent;
        [SerializeField] private Transform _letterPairParent;

        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private AudioClip _correctSound;
        [SerializeField] private AudioClip _yaySound;
        [SerializeField] private GameObject _congratsPanel;
        private int _currentWordIndex = -1;
        public Action<WordGuess> CorrectAnsweredEvent;

        public Action LevelFinishedEvent;


        void Start()
        {
            ShuffleWordsToGuess();
            NextQuestion();
        }

        private void AssembleLetters(WordGuess wordGuess)
        {

            System.Random rng = new System.Random();
            var chars = wordGuess.MyWord.ToCharArray();
            var randomizedLetters = new String(chars.OrderBy(x => rng.Next()).ToArray());

            while (randomizedLetters.Equals(wordGuess.MyWord))
            {
                randomizedLetters = new string(chars.OrderBy(x => rng.Next()).ToArray());
            }

            foreach (var letter in randomizedLetters)
            {
                var newLetter = Instantiate(_letterChoicePrefab);
                var letterScript = newLetter.GetComponent<LetterChoice>();
                letterScript.SetLetter(letter.ToString());
                newLetter.transform.SetParent(_letterParent);
                var newBox = Instantiate(_letterPairPrefab);
                newBox.transform.SetParent(_letterPairParent);
            }

            _imageToGuess.sprite = wordGuess.MySprite;
        }

        void DestroyAllChildren(Transform t)
        {
            int childCount = t.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = t.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void EvaluateAnswer()
        {
            var currentGuessWord = "";
            foreach (var letter in _letterParent.GetComponentsInChildren<LetterChoice>())
            {
                currentGuessWord += letter.MyLetter.text;
            }

            if (currentGuessWord.ToLower().Equals(_wordGuesses[_currentWordIndex].MyWord))
            {
                try
                {
                    _soundEffect.PlayOneShot(_correctSound);
                    CorrectAnsweredEvent?.Invoke(_wordGuesses[_currentWordIndex]);
                }
                catch (IndexOutOfRangeException)
                {

                }
                catch (NullReferenceException)
                {

                }

                Invoke("NextQuestion", 2f);
            }
        }

        public void NextQuestion()
        {
            DestroyAllChildren(_letterParent);
            DestroyAllChildren(_letterPairParent);
            _currentWordIndex++;
            _imageToGuess.enabled = false;
            try
            {
                if (_currentWordIndex < _wordGuesses.Count)
                {
                    var word = _wordGuesses[_currentWordIndex];
                    _imageToGuess.sprite = _wordGuesses[_currentWordIndex].MySprite;
                    AssembleLetters(word);

                    _imageToGuess.enabled = true;
                    _imageToGuess.rectTransform.localScale = Vector3.one * word.DesiredScale;
                    iTween.ScaleTo(_imageToGuess.gameObject, iTween.Hash(
                        "scale", (Vector3.one * word.DesiredScale) * 0.8f,
                        "time", 1f,
                        "looptype", "pingpong",
                        "easetype", iTween.EaseType.easeInOutCubic
                    ));
                }
                else
                {
                    _imageToGuess.enabled = false;
                    iTween.Stop(_imageToGuess.gameObject);
                    LevelFinishedEvent?.Invoke();
                    _soundEffect.PlayOneShot(_yaySound);
                    _congratsPanel.SetActive(true);
                    Debug.Log("Level Finished!");
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Debug.LogError(exception.ToString());
            }
        }

        void ActionAfterTweenComplete()
        {
            iTween.Stop(_imageToGuess.gameObject);
        }

        void RandomizeChildren()
        {
            int childCount = _letterParent.childCount;
            if (childCount == 0) return;

            // Create a list of the children
            List<Transform> children = new();
            for (int i = 0; i < childCount; i++)
            {
                children.Add(_letterParent.GetChild(i));
            }

            // Randomize the list of children
            System.Random rng = new();
            children = children.OrderBy(x => rng.Next()).ToList();

            // Re-parent the children in the randomized order
            for (int i = 0; i < childCount; i++)
            {
                children[i].SetParent(_letterParent);
            }
        }

        private void ShuffleWordsToGuess()
        {
            Random rnd = new Random();
            int n = _wordGuesses.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = _wordGuesses[k];
                _wordGuesses[k] = _wordGuesses[n];
                _wordGuesses[n] = value;
            }
        }
    }
}