using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomButton : MonoBehaviour
{
    public UnityEvent click;
    bool down;
    Camera cam;
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            down = true;
            /*
            RaycastHit2D[] hit = Physics2D.RaycastAll(cam.ScreenPointToRay(Input.mousePosition).origin, cam.ScreenPointToRay(Input.mousePosition).direction);
            Debug.Log("Hit " + hit[hit.Length-1].transform.name);
            Debug.Log("Nome " + name);
            if (hit[hit.Length - 1].transform.gameObject == gameObject) { 
                    down = true;
            }
            */
        }
        else
        {
            
            down = false;
        }
        if (down)
            click.Invoke();
    }
    private void OnMouseExit()
    {
        down = false;
    }
}
