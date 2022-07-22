using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using System.Text;

namespace AITalker
{


    public class Form : MonoBehaviour
    {
        [SerializeField]
        PointClass pointClass;
        FaceAPI faceapi;
        string url_face = "https://uryu:kohei@aitalker.jp.ngrok.io/face";

        private void Start()
        {
            faceapi = GetComponent<FaceAPI>();
        }

        public IEnumerator APIExample(byte[] _byte)
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", _byte, "uryu.png", "image/png");
            //form.AddField("userid", PlayerPrefs.GetString("user_id", ""), Encoding.UTF8);
            //URLをPOSTで用意
            UnityWebRequest webRequest = UnityWebRequest.Post(url_face, form);
            //UnityWebRequestにバッファをセット
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            //URLに接続して結果が戻ってくるまで待機
            yield return webRequest.SendWebRequest();

            faceapi.DispFixParam(webRequest.downloadHandler.text);

            if (webRequest.isNetworkError)
            {
                //通信失敗
                Debug.Log(webRequest.error);
            }
            else
            {
                //通信成功
                faceapi.DispFixParam(webRequest.downloadHandler.text);
                pointClass.MinusPoint(2);

            }
        }
    }
}