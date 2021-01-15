using UnityEngine;

namespace florius
{
    public class MonoBehaviuorWithRectTransform : MonoBehaviour
    {
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        
        protected RectTransform _rectTransform;
    }
}