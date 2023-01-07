using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KidsLearning.Assets.level3
{
    public class LetterChoice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TMP_Text _text;
        public TMP_Text MyLetter => _text;
        public RectTransform MyRectTransform => _rectTransform;
        private LetterDragManager _manager = null;
        private RectTransform _rectTransform;
        private LetterPair _currentLetterPairHover;
        private Vector2 _originalSizeDelta;
        private Vector2 _centerPoint;
        private int _indexBeforeDragging = -1;
        public int IndexBeforeDragging => _indexBeforeDragging;
        private Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);

        public void SetLetter(string letter)
        {
            _text.text = letter;
        }
        void Start()
        {
            _manager = transform.root.GetComponent<LetterDragManager>();
            _centerPoint = (transform as RectTransform).rect.center;
            _rectTransform = GetComponent<RectTransform>();
            _originalSizeDelta = _rectTransform.sizeDelta;
            _indexBeforeDragging = transform.GetSiblingIndex();
            iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", Vector3.one,
                "time", 0.4f,
                "easetype", iTween.EaseType.easeInOutCubic
            ));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _manager.RegisterDraggedObject(this);
            iTween.ScaleTo(gameObject, iTween.Hash(
                "scale", Vector3.one * 1.2f,
                "time", 0.4f,
                "easetype", iTween.EaseType.easeInOutCubic
            ));
            _indexBeforeDragging = transform.GetSiblingIndex();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_manager.IsWithinBounds(_worldCenterPoint + eventData.delta))
            {
                transform.Translate(eventData.delta);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (_currentLetterPairHover == null)
            {
                _manager.UnregisterDraggedObject(this);
                transform.SetSiblingIndex(_indexBeforeDragging);
            }
            else
            {
                _manager.UnregisterDraggedObject(this, _currentLetterPairHover.transform);
            }

            _currentLetterPairHover = null;

            try
            {
                iTween.ScaleTo(gameObject, iTween.Hash(
                    "scale", Vector3.one,
                    "time", 0.4f,
                    "easetype", iTween.EaseType.easeInOutCubic
                ));
            }
            catch (MissingReferenceException) { }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out LetterPair answerPart) && _manager != null && _manager.GetCurrentDraggedObject() == this)
            {
                _currentLetterPairHover = answerPart;
                Debug.Log("Enter!!", answerPart.gameObject);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out LetterPair answerPart) && _currentLetterPairHover == answerPart && _manager.GetCurrentDraggedObject())
            {
                _currentLetterPairHover = null;
                Debug.Log("Leave!!", answerPart.gameObject);
            }
        }
    }
}