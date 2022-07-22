using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace AITalker
{
    public class NegaPosiClass : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI textUI;
        float nega_posi;

        private void Start()
        {
            NegaPosi = PlayerPrefs.GetFloat("nega_posi", 0.0f);
        }

        public float NegaPosi
        {
            get { return nega_posi; }
            set
            {
                nega_posi = value;
                SetText(nega_posi);
            }
        }
        // Start is called before the first frame update
        public void SumNegaPosi(float _nega_posi)
        {
            NegaPosi = NegaPosi + _nega_posi;
        }
        public void MainusNegaPosi(float _negaposi)
        {
            NegaPosi = NegaPosi - _negaposi;
        }
        void SetText(float _point)
        {
            string text_word = "";
            if (_point > 15)
            {
                text_word = "絶好調";
            }
            else if (_point > 5)
            {
                text_word = "良好";
            }
            else if (_point > -5)
            {
                text_word = "普通";
            }
            else if (_point > -15)
            {
                text_word = "不調";
            }
            else if (_point > -30)
            {
                text_word = "絶不調";
            }
            textUI.text = text_word;
            text_word = null;
        }
    }
}