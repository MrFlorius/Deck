using System;
using florius.Deck;
using UnityEngine;

namespace florius.Table
{
    public class TableController : MonoBehaviour
    {
        public DeckController DeckController;
        public TableSlot TableSlotPrefab;

        private void Start()
        {
            for (int i = 0; i < DeckController.NumberOfCards; i++)
                Instantiate(TableSlotPrefab, transform);
        }
    }
}