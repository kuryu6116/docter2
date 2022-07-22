using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    Animator anim;
    AnimatorStateInfo currentState;
    AnimatorStateInfo previousState;


    FaceUpdate faceAnim;

    // Start is called before the first frame update
    void Start()
    {
        faceAnim = GetComponent<FaceUpdate>();
        anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;


        //SetAnimation("good");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetAnimation(string result,int _num)
    {
        //ポジティブ
        if (result=="good")
        {
            faceAnim.OnCallChangeFace("smile2");
            //faceAnim.OnCallChangeFace("smile2@sd_hmd");
        }
        //普通
        else if (result=="normal")
        {
            faceAnim.OnCallChangeFace("mth_o");
            //faceAnim.OnCallChangeFace("mth_o@sd_hmd");
        }
        //
        else
        {
            faceAnim.OnCallChangeFace("sad");
            //faceAnim.OnCallChangeFace("sad@sd_hmd");
        }

        yield return new WaitForSeconds(_num);
        faceAnim.OnCallChangeFace("default");
    }



    
}
