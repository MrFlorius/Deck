using System;
using System.Collections.Generic;
using DG.Tweening;
using florius.Card;
using UnityEngine;
using Random = System.Random;

namespace florius.Deck
{
    public class Deck : MonoBehaviour
    {
        public CardView CardViewPrefab;
        public CardDataFactory Factory;

        [Header("Animation Settings")]
        public int CardCount;
        public float PlacementAngle;
        public float PlacementDuration;
        public Ease Easing = Ease.InOutElastic;

        [Header("Placement Settings")] 
        public Vector2 cardPivot;
        public Vector3 targetAncoredPosition;
        
        [Header("Current Cards")] public List<CardView> Cards = new List<CardView>();
        
        private int idx;
        private Random random = new Random();

        private void Start()
        {
            RefillDeck(CardCount);
        }

        private CardView CreateCardView(Vector2 pivot, Vector3 anchoredPosition)
        {
            var data = Factory.RequestNewCardData();
            Debug.Log(data);
            var c = Instantiate(CardViewPrefab, transform);
            c.rectTransform.pivot = pivot;
            c.rectTransform.anchoredPosition = anchoredPosition;
            c.Card = data;

            return c;
        }

        private void RefillDeck(int count)
        {
            Cards.ForEach(Death);
            Cards.Clear();

            for (int i = 0; i < count; i++)
            {
                var c = CreateCardView(cardPivot, targetAncoredPosition);
                Cards.Add(c);
                c.OnZeroHp += Death;
                Debug.Log("Subscribed");
            }
            
            RearrangeCards();
        }

        private void RearrangeCards()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                float centered_idx = ((float) Cards.Count / 2 - i) - 0.5f;
                Cards[i].transform.DORotate(
                    new Vector3(0, 0, PlacementAngle * centered_idx / Cards.Count),
                    PlacementDuration).SetEase(Easing);
            }
        }

        public void Cycle()
        {
            var r = random.Next(0, 20);
            var c = Cards[idx];
            switch (random.Next(0, 2))
            {
                case 0:
                    c.Card.Mana = r;
                    break;
                case 1:
                    c.Card.Hp = r;
                    break;
                case 2:
                    c.Card.Attack = r;
                    break;
            }

            idx += 1;
            if (idx >= Cards.Count) idx = 0;
        }

        private void Death(CardView c)
        {
            Debug.Log("Killing");
            Cards.Remove(c);
            c.OnZeroHp -= Death;
            idx -= 1;
            Destroy(c.gameObject);
            RearrangeCards();
        }
    }
}