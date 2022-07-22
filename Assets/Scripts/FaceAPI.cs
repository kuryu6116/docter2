using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace AITalker
{
    public class FaceAPI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI text_ui_1, text_ui_2, item_ui_1, item_ui_2;
        Form form;
        [SerializeField]
        RawImage rawimage;  //Image for rendering what the camera sees.
        WebCamTexture webcamTexture = null;
        [SerializeField]
        Sprite[] sprite;
        [SerializeField]
        Image[] image;
        [SerializeField]
        NegaPosiClass negaposi;

        Button button;
        float time = 20;
        [SerializeField]
        PermissionAuthorization permissionAuth;


        private void Start()
        {
            //permissionAuth = GetComponent<PermissionAuthorization>();
            form = GetComponent<Form>();

            StartCoroutine(CameraStart());
        }

        private void Update()
        {
            time += Time.deltaTime;
            
            if (time >= 30 & permissionAuth.permission_flag)
            {
                //texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);
                time -= 30;
                Texture2D texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);
                webcamTexture.Play();
                //Save the image to the Texture2D
                texture.SetPixels(webcamTexture.GetPixels());
                texture.Apply();
                //Encode it as a PNG.
                byte[] bytes = texture.EncodeToPNG();
                texture = null;
                webcamTexture.Pause();
                StartCoroutine(form.APIExample(bytes));
            }
            
        }

        IEnumerator CameraStart()
        {
            yield return StartCoroutine(permissionAuth.ICameraPermission());
            //Save get the camera devices, in case you have more than 1 camera.
            //if (permissionAuth.permission_flag)
            if (permissionAuth.permission_flag)
            {
                
                WebCamDevice[] camDevices = WebCamTexture.devices;
                //Get the used camera name for the WebCamTexture initialization.
#if UNITY_IOS && UNITY_ANDROID
            string camName = camDevices[1].name;
#elif UNITY_2018_1_OR_NEWER
                string camName = camDevices[0].name;
#endif
                webcamTexture = new WebCamTexture(camName);
                camName = null;
                //Render the image in the screen.
                rawimage.texture = webcamTexture;
                //rawimage.material.mainTexture = webcamTexture;
                webcamTexture.Play();
                


            }
            else
            {
                //設定をお願いする

            }
        }




        //時間で撮影する場合
        /*    IEnumerator MainCoroutin()
            {
                if (main_flag)
                {
                    yield break;
                }
                main_flag = true;
                yield return new WaitForSeconds(10);
                StartCoroutine("SnapShot");
                yield return new WaitForSeconds(5);
                //post.FaceMain(pict);
                yield return new WaitForSeconds(10);
                //DispFixParam();
                main_flag = false;

            }*/

        public void DispFixParam(string _result)
        {
            if (_result != "error")
            {
                string[] api_result = _result.Split(',');
                item_ui_1.text = api_result[0];
                text_ui_1.text = api_result[1];
                if (api_result.Length >= 3)
                {
                    item_ui_2.text = api_result[2];
                    text_ui_2.text = api_result[3];
                }
                ChangeImage(image[0], api_result[0]);
                ChangeImage(image[1], api_result[2]);
            }
        }

        public void OnClickPhoto()
        {
            StartCoroutine(SaveImage());
        }

        IEnumerator SaveImage()
        {
            yield return StartCoroutine(permissionAuth.ICameraPermissionCheck());
            //if (permissionAuth.permission_flag)
            if (true)
            {
                //button.interactable = false;
                //rawimage.enabled = true;
                //yield return null;
                //webcamTexture.Play();
                yield return new WaitForSeconds(1);
                //Create a Texture2D with the size of the rendered image on the screen.
                Texture2D texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);

                //Save the image to the Texture2D
                texture.SetPixels(webcamTexture.GetPixels());
                texture.Apply();
                //Encode it as a PNG.
                byte[] bytes = texture.EncodeToPNG();

                StartCoroutine(form.APIExample(bytes));
                //webcamTexture.Pause();
                //rawimage.enabled = false;

                yield return new WaitForSeconds(15);
                //button.interactable = true;
                //webcamTexture.Pause();


                /*
                bool flag = true;
                for (var i = 0; flag; i++)
                {
                    if (System.IO.Directory.Exists(Application.dataPath + "/images/image" + i + ".png"))
                    {
                        File.WriteAllBytes(Application.dataPath + "/images/image" + i + ".png", bytes);
                        flag = false;
                    }
                }
                */
            }
            else
            {
                //インフォメーションの表示
            }
        }

        public void ChangeImage(Image _image, string _text)
        {
            switch (_text)
            {
                case "怒り":
                    _image.sprite = sprite[0];
                    negaposi.MainusNegaPosi(0.2f);
                    break;
                case "軽蔑":
                    _image.sprite = sprite[1];
                    negaposi.MainusNegaPosi(0.2f);
                    break;
                case "嫌悪":
                    _image.sprite = sprite[2];
                    negaposi.MainusNegaPosi(0.2f);
                    break;
                case "恐れ":
                    _image.sprite = sprite[3];
                    negaposi.MainusNegaPosi(0.2f);
                    break;
                case "幸福":
                    _image.sprite = sprite[4];
                    negaposi.SumNegaPosi(1);
                    break;
                case "平常":
                    _image.sprite = sprite[5];
                    negaposi.SumNegaPosi(0.01f);
                    break;
                case "悲しみ":
                    _image.sprite = sprite[6];
                    negaposi.MainusNegaPosi(0.3f);
                    break;
                case "驚き":
                    _image.sprite = sprite[7];
                    negaposi.SumNegaPosi(0.1f);
                    break;
            }
        }






    } // class TestFaceAPI
}