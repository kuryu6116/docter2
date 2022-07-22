using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIManage : MonoBehaviour
{
    [SerializeField]
    GameObject entryUI;
    [SerializeField]
    GameObject addUserUI;

    public void UndispUI()
    {
        addUserUI.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (entryUI.activeSelf)
            {
                entryUI.SetActive(false);
                addUserUI.SetActive(false);
            }
        }
    }
}
