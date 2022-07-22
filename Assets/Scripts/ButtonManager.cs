using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;
namespace AITalker
{
    public class ButtonManager : MonoBehaviour
    {
        public Material[] skyBox;
        public GameObject[] backView;
        [SerializeField]
        RotateSkyBox skybox;
        [SerializeField]
        PointClass point;


        [System.Serializable]
        private class Pair
        {
            public string key;
            public Button btn;

            public Pair(Button btn)
            {
                key = "";
                this.btn = btn;
            }
        }

        [SerializeField]
        private List<Pair> m_Pair;

        public UnityAction<string> onClick { get; set; }

        private void Awake()
        {
            foreach (Pair pair in m_Pair)
            {
                Button btn = pair.btn;
                if (btn == null) { continue; }
                btn.onClick.AddListener(() => OnClick(pair.key));
            }
        }
        private void Setup()
        {
            // 子供にいるボタンを列挙.
            Button[] btnList = transform.GetComponentsInChildren<Button>();

            if (m_Pair == null) { m_Pair = new List<Pair>(); }
            foreach (Button btn in btnList)
            {
                if (m_Pair.FindIndex(x => x.btn == btn) < 0)
                {
                    m_Pair.Add(new Pair(btn));
                }
            }

            // ボタンが無くなっていたらリストから削除.
            for (int i = m_Pair.Count - 1; i >= 0; i--)
            {
                if (m_Pair[i].btn == null)
                {
                    m_Pair.RemoveAt(i);
                }
            }
        }


        public Button Get(string key)
        {
            // キーで取得.
            Pair pair = m_Pair.Find(x => x.key == key);
            if (pair == null) { return null; }
            return pair.btn;
        }

        /// <summary>
        /// ボタンを押したときの処理
        /// </summary>
        /// <param name="key">各ボタンに属する値振りで文字列現状具体的には1,2,3,4</param>
        private void OnClick(string key)
        {
            point.MinusPoint(5);
            int _num = int.Parse(key) - 1;
            if (skyBox.Length != 0)
            {
                RenderSettings.skybox = skyBox[_num];
                skybox.ChangeSkybox();
            }
            if (backView.Length != 0)
            {

                for (int i = 0; i < backView.Length; i++)
                {
                    if (_num == i)
                    {
                        backView[i].SetActive(true);
                    }
                    else
                    {
                        backView[i].SetActive(false);
                    }
                }
            }

        }



#if UNITY_EDITOR
        [CustomEditor(typeof(ButtonManager))]
        public class UIButtonListEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                ButtonManager btnList = target as ButtonManager;
                if (GUILayout.Button("Setup"))
                {
                    btnList.Setup();
                }

                base.OnInspectorGUI();
            }
        }
#endif
    }
}