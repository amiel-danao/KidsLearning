using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KidsLearning.Assets;
using System;

namespace KidsLearning
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreNumberText;
        private RESTConnection _connection;
        [SerializeField] private Animator _animator;
        void Start()
        {
            _connection = FindObjectOfType<RESTConnection>();
            if (_connection != null )
            {
                _connection.SavedScoreEvent += OnScoreSavedEvent;
            }
        }

        private void OnScoreSavedEvent(int score)
        {
            _animator.SetBool("go", true);
            _scoreNumberText.text = $"+{score}";
        }

        private void EndScoreUI()
        {
            _animator.SetBool("go", false);
            _scoreNumberText.text = string.Empty;
        }
    }
}
