using System;
using florius.Card;
using UnityEngine;
using Random = System.Random;

namespace florius.Deck
{
    [RequireComponent(typeof(Deck))]
    public class DeckController : MonoBehaviour
    {
        public Deck Deck;
        public CardView CardViewPrefab;
        public CardDataFactory Factory;
        public int NumberOfCards;
        
        private int idx;
        private Random random = new Random();
        
        public void Cycle()
        {
            var r = random.Next(0, 20);
            var c = Deck.Cards[idx];
            
            c.OnZeroHp += Death;
            
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
            if (idx >= Deck.Cards.Count) idx = 0;
        }

        private void Death(CardView c)
        {
            idx -= 1;
            if (idx >= Deck.Cards.Count || idx < 0) idx = 0;
        }

        public void RefillDeck(int count)
        {
            Deck.Clear();
            
            for (int i = 0; i < count; i++)
            {
                var c = CreateCardView();
                Deck.AddToDeck(c);
            }
        }
        
        private CardView CreateCardView()
        {
            var data = Factory.RequestNewCardData();
            var c = Instantiate(CardViewPrefab, transform);
            c.Card = data;

            return c;
        }

        private void Start()
        {
            RefillDeck(NumberOfCards);
        }
    }
}