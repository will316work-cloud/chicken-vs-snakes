using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChickenSnakes.UI
{
    public class LiveDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _numericCounter;
        [SerializeField] private GridLayoutGroup _spriteCounter;

        private TMP_Text _numericCountText;

        private void Awake()
        {
            _numericCountText = _numericCounter.GetComponentInChildren<TMP_Text>();
        }

        public void UpdateDisplay(uint lives)
        {
            int spriteCount = _spriteCounter.transform.childCount;

            if (lives <= spriteCount)
            {
                _numericCounter.SetActive(false);
                _spriteCounter.gameObject.SetActive(true);

                for (int i = 0; i < spriteCount; i++)
                {
                    _spriteCounter.transform.GetChild(i).gameObject.SetActive(lives > i);
                }
            }
            else
            {
                _numericCounter.SetActive(true);
                _spriteCounter.gameObject.SetActive(false);

                _numericCountText.text = "x " + lives;
            }
        }
    }
}
