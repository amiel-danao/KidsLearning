using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace KidsLearning
{
    public class AnswerParts : MonoBehaviour
    {
        [SerializeField] private RectTransform _originalPosition;
        [SerializeField] private Image _image;
        [SerializeField] private AnswerChoice _answerChoice;
        public AnswerChoice MyAnswerChoice => _answerChoice;
        [SerializeField] private TMP_Text _myText;
        public TMP_Text MyText => _myText;
        public float MyAnswer => _answer;
        private float _answer;
        private Collider2D _collider;
        private Color colorBlinkFrom = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        private Color colorBlinkTo = new Color(1.0f, 0f, 0f, 0.6f);

        public void SetAnswer(float answer)
        {
            this._answer = answer;
            _answerChoice.MyText.text = _answer.ToString();
        }

        public void SetDone()
        {
            _image.color = Color.white;
            Destroy(_collider);
        }

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void SetAsCurrent()
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", colorBlinkFrom,
                "to", colorBlinkTo,
                "time", 1f,
                "looptype", "pingpong",
                "onupdate", "UpdateColor"
            ));
            _collider.enabled = true;
        }

        void UpdateColor(Color color)
        {
            _image.color = color;
        }
    }
}