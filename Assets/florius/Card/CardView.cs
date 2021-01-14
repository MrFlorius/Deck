using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace florius.Card
{
    public class CardView : MonoBehaviour
    {

        [Header("UI")]
        public Image Icon;
        public Text Hp;
        public Text Mana;
        public Text Attack;

        [Header("Counter animation settings")]
        public float tweenTime = 2f;
        public Ease easing = Ease.Linear;
        
        public RectTransform rectTransform
        {
            get
            {
                if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
            set => _rectTransform = value;
        }
        
        public CardData Card
        {
            get => card;
            set
            {
                if(card != null) card.cardUpdate -= OnCardDataChanged; //unsubsribe from old one
                card = value;
                card.cardUpdate += OnCardDataChanged;
                OnCardChanged(card);
            }
        }

        public event Action<CardView> OnZeroHp; 

        private RectTransform _rectTransform;
        private CardData card;

        private void OnCardDataChanged(CardData c)
        {
            Icon.sprite = c.Image;
            CounterTween(Hp, c.Hp);
            CounterTween(Mana, c.Mana);
            CounterTween(Attack, c.Attack).OnComplete(() =>
            {
                Debug.Log("Checking hp");
                if(c.Hp < 1) OnZeroHp?.Invoke(this);
            });
        }
        
        private void OnCardChanged(CardData c)
        {
            Icon.sprite = c.Image;
            Hp.text = c.Hp.ToString();
            Mana.text = c.Mana.ToString();
            Attack.text = c.Attack.ToString();
        }

        private void Start()
        {
            if (card == null) return;
            OnCardChanged(card);
            card.cardUpdate += OnCardDataChanged;
        }

        private void OnDisable()
        {
            if(card != null) card.cardUpdate -= OnCardDataChanged;
        }

        private TweenerCore<int, int, NoOptions> CounterTween(Text t, int n)
        {
            return DOTween.To(() => int.Parse(t.text), x => t.text = x.ToString(), n, tweenTime)
                .SetEase(easing);
        }
    }
}
