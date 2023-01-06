using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using KidsLearning.Assets.level3;

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
        private int _currentWordIndex = -1;
        public Action<WordGuess> CorrectAnsweredEvent;
        public Action LevelFinishedEvent;


        void Start()
        {
            

            iTween.ScaleTo(_imageToGuess.gameObject, iTween.Hash(
                "scale", Vector3.one * 0.8f,
                "time", 1f,
                "looptype", "pingpong",
                "easetype", iTween.EaseType.easeInOutCubic
            ));

            NextQuestion();
        }

        private void AssembleLetters(WordGuess wordGuess)
        {
            DestroyAllChildren(_letterParent);
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

        public void NextQuestion()
        {
            int previousIndex = _currentWordIndex;
            _currentWordIndex++;
            _imageToGuess.sprite = null;

            try
            {
                if (previousIndex > 0)
                {
                    var previousAnswer = _wordGuesses[previousIndex];
                    CorrectAnsweredEvent?.Invoke(previousAnswer);
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
            catch (NullReferenceException)
            {

            }

            try
            {
                if (_currentWordIndex < _wordGuesses.Count)
                {
                    _imageToGuess.sprite = _wordGuesses[_currentWordIndex].MySprite;
                    AssembleLetters(_wordGuesses[_currentWordIndex]);
                }
                else
                {
                    Debug.Log("Level Finished!");
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Debug.LogError(exception.ToString());
            }
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
    }
}