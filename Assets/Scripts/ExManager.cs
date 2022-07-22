using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FadeManager.Instance.FadeIn(2.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeManager.Instance.LoadScene("Scene/Start", 3.0f);
        }
    }
}
