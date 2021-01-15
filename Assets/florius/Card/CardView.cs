using System;
using System.Linq;
using Coffee.UIEffects;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using florius.DragnDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace florius.Card
{
    public class CardView : Dragable
    {
        [NonSerialized] public bool IsPlaced;
        [Header("UI")] public Image Icon;
        public Text Hp;
        public Text Mana;
        public Text Attack;

        [Header("Counter animation settings")] public float tweenTime = 2f;
        public Ease easing = Ease.Linear;

        public CardData Card
        {
            get => card;
            set
            {
                if (card != null) card.cardUpdate -= OnCardDataChanged; //unsubsribe from old one
                card = value;
                card.cardUpdate += OnCardDataChanged;
                OnCardChanged(card);
            }
        }
        
        protected UIShiny ShinyEffect
        {
            get
            {
                if (shinyEffect == null) shinyEffect = GetComponentInChildren<UIShiny>();
                return shinyEffect;
            }
            set => shinyEffect = value;
        }

        public event Action<CardView> OnZeroHp
        {
            add
            {
                if (onZeroHp == null || !onZeroHp.GetInvocationList().Contains(value))
                {
                    onZeroHp += value;
                }
            }
            remove { onZeroHp -= value; }
        }

        public event Action<CardView, bool> OnDragged
        {
            add
            {
                if (onDragged == null || !onDragged.GetInvocationList().Contains(value))
                {
                    onDragged += value;
                }
            }
            remove { onDragged -= value; }
        }

        private event Action<CardView> onZeroHp;
        private event Action<CardView, bool> onDragged;
        private CardData card;
        private UIShiny shinyEffect;

        public void Kill()
        {
            onZeroHp?.Invoke(this);
            Destroy(gameObject);
        }
        
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            onDragged?.Invoke(this, true);
            IsPlaced = false;
            ShinyEffect.Play();
        }

        public override void OnPostDropped()
        {
            IsPlaced = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            onDragged?.Invoke(this, IsPlaced);
            ShinyEffect.Stop();
            ShinyEffect.effectFactor = 0;
        }

        private void OnCardDataChanged(CardData c)
        {
            Icon.sprite = c.Image;
            CounterTween(Hp, c.Hp);
            CounterTween(Mana, c.Mana);
            CounterTween(Attack, c.Attack).OnComplete(() =>
            {
                if (c.Hp > 0) return;
                Kill();
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
            card.cardUpdate += OnCardDataChanged;
            OnCardChanged(card);
        }

        private void OnEnable()
        {
            if (card == null) return;
            card.cardUpdate += OnCardDataChanged;
        }

        private void OnDisable()
        {
            if (card != null) card.cardUpdate -= OnCardDataChanged;
            if (onZeroHp != null) foreach (var d in onZeroHp.GetInvocationList()) onZeroHp -= (Action<CardView>) d;
            if (onDragged != null)
            {
                foreach (var d in onDragged.GetInvocationList()) onDragged -= (Action<CardView, bool>) d;
            }
        }
        
        private TweenerCore<int, int, NoOptions> CounterTween(Text t, int n)
        {
            return DOTween.To(() => int.Parse(t.text), x => t.text = x.ToString(), n, tweenTime)
                .SetEase(easing);
        }
    }
}