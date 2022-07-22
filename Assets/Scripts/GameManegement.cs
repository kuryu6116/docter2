
using UnityEngine;
using UnityEngine.SceneManagement;
namespace AITalker
{
    public class GameManegement : MonoBehaviour
    {
        Post post;
        PointClass pointClass;
        NegaPosiClass nega_posiClass;

        private void Start()
        {
            pointClass = GetComponent<PointClass>();
            post = GetComponent<Post>();
            nega_posiClass = GetComponent<NegaPosiClass>();

            nega_posiClass.NegaPosi = PlayerPrefs.GetFloat("nega_posi", 0.0f);
            //ネガポジの取得

        }


        public void OnApplicationQuit()
        {
            if (SceneManager.GetActiveScene().name == "Main") {
                StartCoroutine(post.APIPoint(pointClass.Point, nega_posiClass.NegaPosi));
            }
        }
        public void DidReceiveMemoryWarning(string message)
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
        }

    }
}
