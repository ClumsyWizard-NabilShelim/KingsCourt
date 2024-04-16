using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClumsyWizard.Utilities
{
    public class CWIconTextPair : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public void Initialize(Sprite sprite, string text)
        {
            icon.sprite = sprite;
            this.text.text = text;
        }

        public void UpdateText(string text)
        {
            this.text.text = text;
        }

        public void UpdateIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}