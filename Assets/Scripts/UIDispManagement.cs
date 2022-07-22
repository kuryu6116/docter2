using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace AITalker
{
    public class UIDispManagement : MonoBehaviour
    {
        [SerializeField]
        GameObject Talk_Canvas;

        [SerializeField]
        GameObject Face_Canvas;
        [SerializeField]
        TextMeshProUGUI button_text;
        int num_button = 0;
        // Start is called before the first frame update


        private void Start()
        {
            button_text = GetComponentInChildren<TextMeshProUGUI>();
        }
        public void DispUIChange()
        {
            num_button += 1;
            bool talk_flag = Talk_Canvas.activeSelf;
            //ボタンチェック
            switch (num_button % 3)
            {
                case 0:
                    button_text.text = "会話\n履歴";
                    Face_Canvas.SetActive(false);
                    Talk_Canvas.SetActive(false);
                    break;
                //デバックボタンが押された場合

                case 1:
                    button_text.text = "表情\n解析";
                    Face_Canvas.SetActive(false);
                    Talk_Canvas.SetActive(true);
                    break;
                case 2:
                    button_text.text = "表示\nなし";
                    Face_Canvas.SetActive(true);
                    Talk_Canvas.SetActive(false);
                    break;
            }
        }
    }
}