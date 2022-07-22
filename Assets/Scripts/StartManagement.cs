using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITalker
{
    public class StartManagement : MonoBehaviour
    {
        PermissionAuthorization permissionAuth;
        Post post;

        // Start is called before the first frame update
        void Start()
        {
            permissionAuth = GetComponent<PermissionAuthorization>();
            StartCoroutine(permissionAuth.ICameraPermission());

            post = GetComponent<Post>();
            post.ConnectCheck();
        }
    }
}