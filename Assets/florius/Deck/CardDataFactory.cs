using System;
using System.Collections;
using florius.Card;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace florius.Deck
{
    public class CardDataFactory : MonoBehaviour
    {
        public String BaseImageUrl = "https://picsum.photos";
        public int Width, Height;
        public Sprite DefaultImage;

        private Random random;

        private void Awake()
        {
            random = new Random();
        }

        private IEnumerator GetSprite(Action<Sprite> success)
        {
            var url = $"{BaseImageUrl}/{Width}/{Height}";
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var t = ((DownloadHandlerTexture) www.downloadHandler).texture;
                var s = Sprite.Create(t, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f));
                success(s);
            }
        }

        public CardData RequestNewCardData()
        {
            var c = new CardData(random.Next(5, 50), random.Next(5, 50), random.Next(5, 50), DefaultImage);
            StartCoroutine(GetSprite(s => c.Image = s));

            return c;
        }
    }
}