using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace KidsLearning
{

    public class AnswerChoice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        public TMP_Text MyText => _text;
        private DragManager _manager = null;
        private AnswerParts _currentAnswerHover;

        private Vector2 _centerPoint;
        private Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);
        // Start is called before the first frame update
        private void Awake()
        {
            _manager = GetComponentInParent<DragManager>();
            _centerPoint = (transform as RectTransform).rect.center;

            iTween.ScaleTo(_text.gameObject, iTween.Hash(
                "scale", Vector3.one * 1.3f,
                "time", 1f,
                "looptype", "pingpong",
                "easetype", iTween.EaseType.easeInOutCubic
            ));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _manager.RegisterDraggedObject(this);
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
            if (_currentAnswerHover == null || _currentAnswerHover.MyAnswerChoice != this)
            {
                _manager.UnregisterDraggedObject(this);
            }
            else
            {
                _manager.SetCorrectAnswer(this, _currentAnswerHover);
                _currentAnswerHover = null;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out AnswerParts answerPart))
            {
                _currentAnswerHover = answerPart;
                Debug.Log("Enter!!");
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out AnswerParts answerPart) && _currentAnswerHover == answerPart)
            {
                _currentAnswerHover = null;
                Debug.Log("Leave!!");
            }
        }
    }

}
