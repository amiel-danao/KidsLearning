using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KidsLearning
{

    public class Clicker : MonoBehaviour
    {
        [SerializeField] GraphicRaycaster m_Raycaster;
        PointerEventData m_PointerEventData;
        [SerializeField] EventSystem m_EventSystem;
        private AnswerChoice _currentItem;
        private bool _isDragging = false;
        private Vector3 _originalItemPosition;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {




            if (Input.GetMouseButtonDown(0) && !_isDragging)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);
                //Set the Pointer Event Position to that of the game object
                m_PointerEventData.position = this.transform.localPosition;

                //Create a list of Raycast Results
                List<RaycastResult> results = new List<RaycastResult>();

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);

                if (results.Count > 0)
                {
                    Debug.Log("Hit " + results[0].gameObject.name);

                    GameObject hitObject = results[0].gameObject;
                    Debug.Log(hitObject.name);
                    if (hitObject.TryGetComponent(out AnswerChoice answerChoice))
                    {
                        _currentItem = answerChoice;
                        _originalItemPosition = _currentItem.transform.position;

                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
                if (_currentItem != null)
                {
                    _currentItem.transform.position = _originalItemPosition;
                }

                _currentItem = null;
            }
        }

        void OnMouseDrag()
        {
            if (_currentItem == null || _isDragging == false) return;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            _currentItem.transform.position = curPosition;
        }
    }
}