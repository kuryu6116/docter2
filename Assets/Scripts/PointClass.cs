using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace AITalker
{
    public class PointClass : MonoBehaviour
    {
        [SerializeField]
        GameObject stopUI;

        [SerializeField]
        TextMeshProUGUI point_textUI;

        private int point;
        public int Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
                CheckPoint();
            }
        }

        private void Start()
        {
            Point = PlayerPrefs.GetInt("Point", 10); ;
        }

        public void SumPoint(int _num)
        {
            Point = Point + _num;
        }

        public void MinusPoint(int _num)
        {
            Point = Point - _num;
        }
        void CheckPoint()
        {
            //UIへの反映
            point_textUI.text = Point.ToString();
            if (Point <= 0)
            {
                Time.timeScale = 0f;
                stopUI.SetActive(true);
            }
            else
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1f;
                    stopUI.SetActive(false);
                }
            }
        }
    }
}