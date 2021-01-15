using UnityEngine;
using UnityEngine.EventSystems;

namespace florius.DragnDrop
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class Dragable : MonoBehaviuorWithRectTransform, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }

        public bool IsDragable;

        private CanvasGroup canvasGroup;

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(IsDragable) rectTransform.anchoredPosition += eventData.delta;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if(IsDragable) CanvasGroup.blocksRaycasts = false;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual void OnPostDropped()
        {
        }
    }
}