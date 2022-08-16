using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class SnakeTypeDisplay : MonoBehaviour
    {
        public TextMeshProUGUI title;
        private Image[] images;
        private void Awake()
        {
            images = GetComponentsInChildren<Image>();
        }

        public void ShowType(SnakeTypeAsset asset)
        {
            Debug.Assert(asset.startingBlocks.Length == images.Length);
            title.text = asset.displayName;
            for (int i = 0; i < asset.startingBlocks.Length; i++)
            {
                images[i].color = asset.startingBlocks[i].color;
            }
        }

    }
}