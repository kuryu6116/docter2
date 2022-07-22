using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using IBM.Watson.Examples;
using System;
using TMPro;
using UnityEngine.Advertisements;

namespace AITalker
{
    public class TalkManagement : MonoBehaviour
    {
        IBM.Watsson.Examples.ExampleStreaming exampleStreaming;

        ExampleTextToSpeechV1 textSpeech;

        [SerializeField]
        Post post;

        
        [SerializeField]
        NegaPosiClass negaposi;

        float score, f;
        string result;
        StringBuilder return_text=new StringBuilder();
        StringBuilder emotion_text = new StringBuilder(15);
        public List<string> List = new List<string>();

        Animator anim;
        [SerializeField]
        TextMeshProUGUI input_result;
        [SerializeField]
        TextMeshProUGUI output_result;

        

        // Start is called before the first frame update
        void Start()
        {
            textSpeech = GetComponent<ExampleTextToSpeechV1>();
            exampleStreaming = GetComponent<IBM.Watsson.Examples.ExampleStreaming>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Text to Speech
            if (exampleStreaming.List.Count != 0)
            {
                StartCoroutine(MainCorutine());
            }
        }

        private string AIresult(string _text)
        {

            
            if (_text.Contains(","))
            {
                string[] dest = _text.Split(',');
                if (dest[0]=="[UNK]")
                {
                    return_text.Append("すいません。分かりません");
                }
                else
                {
                    return_text.Append(dest[0] + "です");
                }
                dest = null;
            }
            else
            {
                return_text.Append(_text); 
            }
            
            
            return return_text.ToString();
        }

        IEnumerator AnimCouroutine(string emotion, int time)
        {
            switch (emotion)
            {
                case "good":
                    anim.SetBool("IsHappy",true);
                    break;
                case "bad":
                    anim.SetBool("IsBad", true);
                    break;
            }
            yield return new WaitForSeconds(time);
            anim.SetBool("IsBad", false);
            anim.SetBool("IsHappy", false);

        }

        IEnumerator MainCorutine()
        {
            //[0]だけでよいか要確認
            /*
            foreach (string v in exampleStreaming.List)
            {
                to_text += v;
            }
            */
            
            string to_text = exampleStreaming.List[0];
            exampleStreaming.List.Clear();

            //入力文字をUiに反映
            //if (result_UI.activeSelf)
            //{
                input_result.text = to_text;
            //}

            if (to_text.Length <= 20)
            {
                yield return StartCoroutine(post.APIResult(to_text, "chat"));
            }
            else
            {
                yield return StartCoroutine(post.APIResult(to_text, "emotion"));
            }


            string post_text = post.result_text;


            exampleStreaming.StopRecording();

            //結果が数字の場合の処理
            if (float.TryParse(post_text, out f))
            {

                //表情から
                score = f;

                //ネガポジ判定
                if (score > 0.5)
                {
                    result = ("good");
                    emotion_text.Append("すごいね");
                }
                else if (score < -1)
                {
                    result = ("bad");
                    emotion_text.Append("悪いのは君じゃない");
                }
                else
                {
                    result = ("normal");
                    emotion_text.Append("なるほど");
                }

                //アニメーションの反映

                ResultTextUI(emotion_text.ToString());
                emotion_text.Length = 0;
                textSpeech.TextToSpeech(result);
                //表情の変更
                yield return StartCoroutine(AnimCouroutine(result, 3));
                result = null;

            }
            //結果が文字情報の場合
            else
            {
                result = "good";

                //喋る処理
                string speech_text = AIresult(post_text);
                ResultTextUI(speech_text);

                textSpeech.TextToSpeechAI(speech_text);
                //yield return StartCoroutine(animationController.SetAnimation(result, time_count));

                speech_text = null;
                result = null;

            }

            post_text = null;
            return_text.Length = 0;
            //マイク音声の初期化
            to_text = null;
            post.result_text = null;
            
            yield return new WaitForSeconds(1.5f);
            exampleStreaming.StartRecording();
        }

        void ResultTextUI(string _text)
        {

        output_result.text = _text;
            
        }

        

    }
}