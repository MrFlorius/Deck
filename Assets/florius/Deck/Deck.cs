using System.Collections.Generic;
using DG.Tweening;
using florius.Card;
using UnityEngine;

namespace florius.Deck
{
    public class Deck : MonoBehaviour
    {
        [Header("Deck initialization settings")]
        public int CardCount;

        [Header("Animation Settings")] public float PlacementDuration;
        public Ease Easing = Ease.InOutElastic;

        [Header("Placement Settings")] public Vector3 AngularSpacing;
        public Vector2 cardPivot;
        public Vector2 CenterAncoredPosition;
        public Vector2 Spacing;

        [Header("Current Cards")] public List<CardView> Cards = new List<CardView>();

        public void RemoveFromDeck(CardView c)
        {
            if (!Cards.Contains(c)) return;
            Cards.Remove(c);
            RearrangeCards();
        }

        public void Clear()
        {
            Cards.ForEach(x => x.Kill());
        }

        public void AddToDeck(CardView c)
        {
            if (Cards.Contains(c)) return;
            Cards.Add(c);
            c.OnZeroHp += RemoveFromDeck;
            c.OnDragged += OnCardDragged;
            c.transform.SetParent(transform, false);
            c.rectTransform.SetPivot(cardPivot);
            RearrangeCards();
        }

        public void InsertToDeck(CardView c, int idx)
        {
            if (Cards.Contains(c)) return;
            Cards.Insert(idx, c);
            c.OnZeroHp += RemoveFromDeck;
            RearrangeCards();
        }

        private void RearrangeCards()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                float centered_offset = ((float) Cards.Count / 2 - i) - 0.5f;
                var c = Cards[i];

                c.transform.DORotate(AngularSpacing * centered_offset, PlacementDuration)
                    .SetEase(Easing)
                    .OnStart(() => c.IsDragable = false)
                    .OnComplete(() => c.IsDragable = true);

                DOTween.To(
                        () => c.rectTransform.anchoredPosition,
                        x => c.rectTransform.anchoredPosition = x,
                        CenterAncoredPosition - Spacing * centered_offset, PlacementDuration)
                    .SetEase(Easing);
            }
        }
        
        private void OnCardDragged(CardView c, bool outside)
        {
            if (outside)
                RemoveFromDeck(c);
            else
                AddToDeck(c);
        }
    }
}