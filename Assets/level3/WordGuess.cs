using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace KidsLearning
{
    [CreateAssetMenu(menuName = "Tools/WordGuess")]
    public class WordGuess : ScriptableObject, IQuestion
    {
        [SerializeField] private string _word;
        [SerializeField] private Sprite _sprite;
        public string MyWord => _word;
        public Sprite MySprite => _sprite;
        [SerializeField] private float _desiredScale = 1;
        public float DesiredScale => _desiredScale;

        public override string ToString()
        {
            return _word;
        }
    }
}