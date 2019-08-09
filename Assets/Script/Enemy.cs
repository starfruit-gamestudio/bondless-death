using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mob
{
    [SerializeField] Axis axis;
    Vector3 startPoint;
    int way;

    Vector2 axisToMove;

    [SerializeField] bool posMov;
    [SerializeField] float delay;

    bool canCheck;

    IEnumerator moveCoroutine;
     WaitForSeconds waitSeconds;

    Animator anim;
    Animator alertAnim;
    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        alertAnim = transform.GetChild(0).GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)transform.position + direction / 2, (Vector2)transform.position + direction);
        posMov = hits.Length > 0;
        anim.SetInteger("Way", (posMov ? 1 : -1));
        canCheck = true;
        waitSeconds = new WaitForSeconds(delay);
        base.Start();
        if (axis == Axis.Horizontal)
        {
            axisToMove = Vector2.right;
            anim.SetBool("Vertical",false);
        }
        else if(axis == Axis.Vertical)
        {
            axisToMove = Vector2.up;
            anim.SetBool("Vertical",true);
        }
        else
        {
            axisToMove = Vector2.zero;
        }
        
        //)



        moveCoroutine = MoveCoroutine();
        StartCoroutine(moveCoroutine);
        //axisToMove = ;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (!Player.freeze)
        {
            spr.flipX = !posMov;
            if (alertAnim.gameObject.active)
                alertAnim.gameObject.SetActive(false);
        }
    }

    IEnumerator MoveCoroutine()
    {

        while (true)
        {
            if (!Player.freeze)
            {
                if (!walking)
                {
                    bool stop = false;
                    canCheck = true;
                    RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)transform.position + direction/2, (Vector2)transform.position + direction);
                    Debug.DrawLine((Vector2)transform.position + direction/2, (Vector2)transform.position + direction, Color.red);

                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            if (hit.collider.CompareTag("Wall"))
                            {
                                posMov = !posMov;
                                break;
                            }
                            if (hit.collider.CompareTag("Enemy") && !hit.collider.isTrigger)
                            {
                                posMov = !posMov;
                                break;
                            }
                        }
                    }

                    if (!stop)
                    {
                        StartMove((Vector2)transform.position + GoToNextPos());
                    }
                }
            }
            yield return waitSeconds;
        }
        yield return null;
    }
    Vector2 GoToNextPos()
    {
        if (!Player.freeze)
        {
            anim.SetInteger("Way", (posMov ? 1 : -1));
            Vector2 move = axisToMove * (posMov ? 1 : -1);
            direction = move.normalized;
            return move;
        }
        return Vector2.zero;
    }


    enum Axis
    {
        Vertical,
        Horizontal,
        None
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Player.freeze)
        {
            if (collision.CompareTag("Player") && !walking && !Player.exiting && canCheck && Player.vunerable)
            {
                canCheck = false;
                Player.freeze = true;
                if (!Player.haveAnswer)
                {
                    alertAnim.SetTrigger("Alert");
                    if (collision.transform.position.x == transform.position.x)
                    {
                        if (collision.transform.position.y > transform.position.y)
                            anim.SetInteger("Way", 1);
                        else
                            anim.SetInteger("Way", -1);
                        anim.SetBool("Vertical", true);
                        
                    }
                    else
                    {
                        if (collision.transform.position.x > transform.position.x)
                        {
                            anim.SetInteger("Way", 1);
                            spr.flipX = false;
                        }
                        else
                        {
                            anim.SetInteger("Way", -1);
                            spr.flipX = true;
                        }
                        anim.SetBool("Vertical", false);
                    }
                    anim.SetTrigger("Attack");
                }
                else
                {
                    alertAnim.gameObject.SetActive(true);
                }
            }else if(collision.CompareTag("Player") && walking)
            {
                if (collision.GetComponent<Player>().GetFinalPos() == finalPos)
                {
                    transform.position = finalPos - (Vector3)direction;
                    finalPos = transform.position;
                }
                
            }
        }
    }

}
