using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KidsLearning.Assets
{    
    public class FullScreenToggle : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Full Screen Toggle");
            Screen.fullScreen = !Screen.fullScreen;
            base.OnPointerDown(eventData);
        }
    }
}