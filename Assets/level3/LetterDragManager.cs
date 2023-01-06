using System.Collections;
using UnityEngine;

namespace KidsLearning.Assets.level3
{
    public class LetterDragManager : MonoBehaviour
    {

        [SerializeField] private WordGame _wordGame;
        [SerializeField]
        private RectTransform
            _defaultLayer = null,
            _dragLayer = null;

        private Rect _boundingBox;

        private LetterChoice _currentDraggedObject = null;

        public LetterChoice CurrentDraggedObject => _currentDraggedObject;

        public LetterChoice GetCurrentDraggedObject()
        { return _currentDraggedObject; }

        private void Awake()
        {
            SetBoundingBoxRect(_dragLayer);
        }

        public void EvaluateAnswer()
        {

        }

        public void SwapLetter(Transform a, int index1, Transform b) {
            int index2 = b.GetSiblingIndex();
            a.SetSiblingIndex(index2);
            b.SetSiblingIndex(index1);
            Debug.Log($"Swapped {a}:index {index2} into {b}:index {index1}");
        }

        public void RegisterDraggedObject(LetterChoice drag)
        {
            _currentDraggedObject = drag;
            drag.transform.SetParent(_dragLayer);
        }

        public void UnregisterDraggedObject(LetterChoice drag)
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
            _wordGame.NextQuestion();
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