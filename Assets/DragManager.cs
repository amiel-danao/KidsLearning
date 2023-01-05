using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private QuestionLogic _questionLogic;
        [SerializeField]
        private RectTransform
            _defaultLayer = null,
            _dragLayer = null;

        private Rect _boundingBox;

        private AnswerChoice _currentDraggedObject = null;
        public AnswerChoice CurrentDraggedObject => _currentDraggedObject;

        private void Awake()
        {
            SetBoundingBoxRect(_dragLayer);
        }

        public void RegisterDraggedObject(AnswerChoice drag)
        {
            _currentDraggedObject = drag;
            drag.transform.SetParent(_dragLayer);
        }

        public void UnregisterDraggedObject(AnswerChoice drag)
        {
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
    }
}