using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomButton : MonoBehaviour
{
    [SerializeField] UnityEvent click;
    bool down;
    Camera cam;
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetMouseButtonUp(0))
        {
            if (down)
                click.Invoke();
            down = false;
        }
    }
    private void OnMouseExit()
    {
        down = false;
    }
}
