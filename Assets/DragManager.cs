using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private QuestionLogic _questionLogic;
        [SerializeField] private RectTransform _dragLayer = null;
        private RectTransform _defaultLayer= null;

        private Rect _boundingBox;

        private AnswerChoice _currentDraggedObject = null;

        public AnswerChoice CurrentDraggedObject => _currentDraggedObject;
        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private AudioClip _clickSound, _unClickSound, _wrongSound;

        private void Start()
        {
            OnShapeFinished();
            SetBoundingBoxRect(_dragLayer);
            _questionLogic.ShapeFinishedEvent += OnShapeFinished;
        }

        private void OnShapeFinished()
        {
            _defaultLayer = _questionLogic.GetCurrentLevelTransform().Find("answer_choices").GetComponent<RectTransform>();
        }

        public void RegisterDraggedObject(AnswerChoice drag)
        {
            _soundEffect.PlayOneShot(_clickSound);
            _currentDraggedObject = drag;
            _currentDraggedObject.MyRectTransform.sizeDelta = _questionLogic.GetAnswerPart(_currentDraggedObject).MyRectTransform.sizeDelta;
            drag.transform.SetParent(_dragLayer);
        }

        public void UnregisterDraggedObject(AnswerChoice drag)
        {
            _soundEffect.PlayOneShot(_unClickSound);
            _currentDraggedObject.ResetSize();
            drag.transform.SetParent(_defaultLayer);
            _currentDraggedObject = null;
        }

        public bool IsWithinBounds(Vector2 position)
        {
            return _boundingBox.Contains(position);
        }

        public void SetCorrectAnswer(AnswerChoice answerChoice, AnswerParts answerParts)
        {
            Destroy(answerChoice.gameObject);
            Destroy(answerParts.MyText);
            iTween.Stop(answerParts.gameObject);
            answerParts.SetDone();
            _questionLogic.NextQuestion();
        }

        private void SetBoundingBoxRect(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var position = corners[0];

            Vector2 size = new Vector2(
                rectTransform.lossyScale.x * rectTransform.rect.size.x,
                rectTransform.lossyScale.y * rectTransform.rect.size.y);

            _boundingBox = new Rect(position, size);
        }

        internal void AddWrongAnswer(AnswerChoice answerChoice)
        {
            _soundEffect.PlayOneShot(_wrongSound);
            _questionLogic.AddWrongAnswer(answerChoice);
        }
    }
}