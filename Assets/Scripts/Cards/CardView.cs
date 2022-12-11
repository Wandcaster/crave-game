using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour
{
    //Animator animator;
    Vector3 startPos;
    private static Vector3 scale= new Vector3(0.21F, 0.21F, 0.21F);
    private void Start()
    {
        startPos= transform.position;
        //animator=GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        //animator.enabled = false;
        transform.localScale = scale;
    }
    private void OnMouseUp()
    {
        transform.position = startPos;
        //animator.enabled = true;
    }
    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position= mousePos;
    }
    private void OnMouseEnter()
    {
        //GetComponent<Animator>().SetBool("MouseHover", true);
    }
    private void OnMouseExit()
    {
        //GetComponent<Animator>().SetBool("MouseHover", false);
    }

}
