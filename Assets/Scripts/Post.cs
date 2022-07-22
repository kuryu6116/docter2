using System.Collections;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using TMPro;
namespace AITalker
{
    public class Post : MonoBehaviour
    {
        #region 変数宣言
        [SerializeField]
        GameObject EntryUI;
        [SerializeField]
        GameObject LoginErrorUI;
        [SerializeField]
        GameObject NewUserErrorUI;
        [SerializeField]
        GameObject NotConnectUI;
        [SerializeField]
        AudioSource audioS;
        [SerializeField]
        AudioClip[] clip;

        StringBuilder url = new StringBuilder("https://uryu:kohei@aitalker.jp.ngrok.io",60);

        public string result_text;
        public string result_emotion;

        //byte[] pict_byte;
        PostData postData = new PostData();
        #endregion
        

        [System.Serializable]
        public class PostData
        {
            public string talk;
            public string username;
            public string password;
            public string userid;
            public string point;
            public string question;
            public string answer;
            public string mail;
            public byte[] pict;
            public string error;
            public string nega_posi;
        }

        private void Start()
        {
            postData.userid = PlayerPrefs.GetString("user_id", "");
        }

        public void ConnectCheck()
        {
            StartCoroutine(APIConnectCheck());
        }
        #region APICheck
        IEnumerator APIConnectCheck()
        {
            // JSONのデータをBodyに入れてPOSTする
            UnityWebRequest request = new UnityWebRequest(url.Append("/check").ToString(), "POST");
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            //Debug.Log(request.downloadHandler.text);
            //GameParam.user_id = int.Parse(request.downloadHandler.text);
            if (request.downloadHandler.text != "ok")
            {
                NotConnectUI.SetActive(true);
            }
            url.Length = 0;
            url.Append("https://uryu:kohei@aitalker.jp.ngrok.io");

            request = null;

        }
        #endregion



        // Start is called before the first frame update
        #region APILogin()
        public IEnumerator APILogin(string _user, string _pass)
        {
            postData.username = _user;
            postData.password = _pass;
            string myJson = JsonUtility.ToJson(postData);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(myJson);
            UnityWebRequest request = new UnityWebRequest(url.Append("/login").ToString(), "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(byteData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            //GameParam.user_id = int.Parse(request.downloadHandler.text);
            if (request.downloadHandler.text != "error")
            {
                string[] result_list = request.downloadHandler.text.Split(',');

                PlayerPrefs.SetString("user_id", result_list[0]);
                PlayerPrefs.SetInt("Point", int.Parse(result_list[1]));
                PlayerPrefs.SetFloat("nega_posi", float.Parse(result_list[2]));
                PlayerPrefs.Save();
                audioS.PlayOneShot(clip[1]);
                FadeManager.Instance.LoadScene("Scene/Main", 2.0f);

            }
            else
            {
                audioS.PlayOneShot(clip[1]);
                LoginErrorUI.SetActive(true);
            }
            //初期化処理
            url.Length = 0;
            url.Append("https://uryu:kohei@aitalker.jp.ngrok.io");
            postData.username = null;
            postData.password = null;
            myJson = null;
            byteData = null;
            request = null;

        }
        #endregion

        #region APIEntry
        public IEnumerator APIEntry(string _user, string _pass, string _mail)
        {
            postData.username = _user;
            postData.password = _pass;
            postData.mail = _mail;
            // JSONのデータをBodyに入れてPOSTする
            string myJson = JsonUtility.ToJson(postData);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(myJson);
            UnityWebRequest request = new UnityWebRequest(url.Append("/entry").ToString(), "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(byteData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.downloadHandler.text != "error")
            {
                EntryUI.SetActive(true);
            }
            else
            {
                NewUserErrorUI.SetActive(true);
            }

            //初期化処理
            postData.username = null;
            postData.password = null;
            postData.mail = null;
            url.Length = 0;
            url.Append("https://uryu:kohei@aitalker.jp.ngrok.io");
            myJson = null;
            byteData = null;
            request = null;

        }
        #endregion

        #region APIPoint
        public IEnumerator APIPoint(int _point, float _nega_posi)
        {
            postData.userid = PlayerPrefs.GetString("user_id", "");
            postData.point = _point.ToString();
            postData.nega_posi = _nega_posi.ToString();
            string myJson = JsonUtility.ToJson(postData);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(myJson);
            UnityWebRequest request = new UnityWebRequest(url.Append("/point").ToString(), "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(byteData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            postData.userid = null;

            url.Length = 0;
            url.Append("https://uryu:kohei@aitalker.jp.ngrok.io");
            myJson = null;
            byteData = null;
            request = null;
        }
        #endregion

        //渡すパラメータの指定

        public IEnumerator APIResult(string _talk,string _url)
        {
            //result_text = null;
            postData.talk = _talk;
            
            // JSONのデータをBodyに入れてPOSTする
            string myJson = JsonUtility.ToJson(postData);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(myJson);
            UnityWebRequest request = new UnityWebRequest(url.Append("/").Append(_url).ToString(), "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(byteData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            result_text = request.downloadHandler.text;

            url.Length = 0;
            url.Append("http://127.0.0.1:5000/");
            postData.talk = null;
            myJson = null;
            byteData = null;
            request = null;
        }

        public IEnumerator APIError(string _error)
        {
            postData.error = _error;
            string myJson = JsonUtility.ToJson(postData);
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(myJson);
            UnityWebRequest request = new UnityWebRequest(url.Append("error").ToString(), "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(byteData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            postData.error = null;
            url.Length = 0;
            url.Append("https://uryu:kohei@aitalker.jp.ngrok.io");
            myJson = null;
            byteData = null;
            request = null;
        }
    }
}