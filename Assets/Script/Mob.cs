using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField]protected bool walking;

    protected Vector2 nextPosition;
    protected Vector2 direction;
    protected Vector3 finalPos;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float walkTolerance;


    // Start is called before the first frame update
    protected void Start()
    {
        finalPos = transform.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (walking)
        {
            nextPosition = transform.position + (Vector3)direction * Time.deltaTime / walkSpeed;
            if (Vector3.Distance(nextPosition, finalPos) < walkTolerance)
            {
                transform.position = finalPos;
                walking = false;
            }
            else
                transform.position = nextPosition;
        }
    }
    protected void StartMove(Vector3 pos)
    {
        walking = true;
        finalPos = transform.position + (Vector3)direction;
        if (finalPos == transform.position)
            walking = false;
    }

    public Vector3 GetFinalPos()
    {
        return finalPos;
    }


}
