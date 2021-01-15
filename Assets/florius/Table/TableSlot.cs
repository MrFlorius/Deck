using DG.Tweening;
using florius.DragnDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace florius.Table
{
    public class TableSlot : DropPanel
    {
        public float PlacementDuration;
        public Ease Easing;
        public Vector2 TargetPosition;
        public Vector3 TargetRotation;
        
        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log("OnDrop");
            base.OnDrop(eventData);
            CurrentObject.rectTransform.SetPivot(new Vector2(0.5f, 0.5f));
            CurrentObject.rectTransform.SetParent(rectTransform);
            CurrentObject.IsDragable = false;

            CurrentObject.OnPostDropped();
            
            DOTween.To(
                () => CurrentObject.rectTransform.anchoredPosition,
                x => CurrentObject.rectTransform.anchoredPosition = x,
                TargetPosition,
                PlacementDuration
            ).SetEase(Easing);

            DOTween.To(
                () => CurrentObject.rectTransform.localRotation,
                x => CurrentObject.rectTransform.localRotation = x,
                TargetRotation,
                PlacementDuration
            ).SetEase(Easing).OnComplete(() => CurrentObject.IsDragable = true);
        }
    }
}