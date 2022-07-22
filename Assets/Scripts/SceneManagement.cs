using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITalker
{
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField]
        GameObject gameManagement;

        Post post;
        PointClass pointClass;
        NegaPosiClass negaposiClass;

        // Start is called before the first frame update

        private void Start()
        {
            if (gameObject.name == "Finish_Button")
            {
                post = gameManagement.GetComponent<Post>();
                pointClass = gameManagement.GetComponent<PointClass>();
                negaposiClass = gameManagement.GetComponent<NegaPosiClass>();
            }
        }

        public void ReStart_Button()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1f;
            }
            StartCoroutine(post.APIPoint(pointClass.Point, negaposiClass.NegaPosi));
            FadeManager.Instance.LoadScene("Scene/Start", 2.0f);
        }

        public void Finish_Button()
        {
            //StartCoroutine(post.APIPoint(pointClass.Point, negaposiClass.NegaPosi));
            //UnityEditor.EditorApplication.isPlaying = false;
            UnityEngine.Application.Quit();
        }


    }
}