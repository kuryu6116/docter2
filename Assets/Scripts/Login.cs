using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace AITalker
{
    public class Login : MonoBehaviour
    {
        [SerializeField]
        Post post;
        [SerializeField]
        TMP_InputField user_name;
        [SerializeField]
        TMP_InputField pass;
        [SerializeField]
        TMP_InputField mail;
        [SerializeField]
        GameObject Error_UI;
        [SerializeField]
        GameObject EntryErrorUI;

        [SerializeField]
        AudioClip[] clip;



        private void Start()
        {
            user_name.text = PlayerPrefs.GetString("User", "");
            pass.text = PlayerPrefs.GetString("Pass", "");
        }



        public void LoginStart()
        {
            StartCoroutine(post.APILogin(user_name.text, pass.text));
            PlayerPrefs.SetString("User", user_name.text);
            PlayerPrefs.SetString("Pass", pass.text);
            PlayerPrefs.Save();
        }
        public void LoginNewUser()
        {
            if (user_name.text == "" || pass.text == "" || mail.text == "")
            {
                EntryErrorUI.SetActive(true);
            }
            else
            {
                StartCoroutine(post.APIEntry(user_name.text, pass.text, mail.text));
                PlayerPrefs.SetString("User", user_name.text);
                PlayerPrefs.SetString("Pass", pass.text);
                PlayerPrefs.Save();
            }
        }
    }
}