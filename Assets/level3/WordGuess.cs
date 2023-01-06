using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace KidsLearning
{
    [CreateAssetMenu(menuName = "Tools/WordGuess")]
    public class WordGuess : ScriptableObject
    {
        [SerializeField] private string _word;
        [SerializeField] private Sprite _sprite;
        public string MyWord => _word;
        public Sprite MySprite => _sprite;

    }
}