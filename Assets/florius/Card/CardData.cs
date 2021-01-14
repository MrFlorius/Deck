using System;
using UnityEngine;
using Random = System.Random;

namespace florius.Card
{
    public class CardData
    {
        public CardData(int hp, int attack, int mana, Sprite image)
        {
            this.hp = hp;
            this.attack = attack;
            this.mana = mana;
            this.image = image;
        }

        public event Action<CardData> cardUpdate;

        private int hp;
        private int attack;
        private int mana;
        private Sprite image;

        public Sprite Image
        {
            get => image;
            set
            {
                image = value;
                cardUpdate?.Invoke(this);
            }
        }


        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                cardUpdate?.Invoke(this);
            }
        }

        public int Attack
        {
            get => attack;
            set
            {
                attack = value;
                cardUpdate?.Invoke(this);
            }
        }

        public int Mana
        {
            get => mana;
            set
            {
                mana = value;
                cardUpdate?.Invoke(this);
            }
        }
    }
}