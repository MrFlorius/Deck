using UnityEngine;
using UnityEngine.EventSystems;

namespace florius.DragnDrop
{
    public class DropPanel : MonoBehaviuorWithRectTransform, IDropHandler
    {
        public Dragable CurrentObject;
        
        public virtual void OnDrop(PointerEventData eventData)
        {
            var dragable = eventData?.pointerDrag.GetComponent<Dragable>();
            if(dragable == null || !dragable.IsDragable) return;
            CurrentObject = dragable;
        }
    }
}
