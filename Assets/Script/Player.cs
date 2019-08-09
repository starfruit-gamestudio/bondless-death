using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{
    [SerializeField] int startEnergy;
    public static int energy;
    [SerializeField] Color inCorpseColor;
    [SerializeField] float unfreezeDelay;
    public static bool vunerable;

    public static bool freeze = false;
    public static bool exiting;

    [SerializeField]bool dead;
    public static bool inCorpse;
    bool unfreeze;
    public static bool haveAnswer;
    bool ending;
    bool canExit;
    Vector3 lastPos;

    SpriteRenderer spr;
    Animator anim;
    WaitForSeconds waitForSeconds;
    AudioSource audioSource;
    AudioSource stepAudioSource;
    [SerializeField] AudioClip corpseClip;
    [SerializeField] AudioClip enemyClip;
    [SerializeField] AudioClip levelUpClip;
    [SerializeField] AudioClip stepClip;

    // Start is called before the first frame update
    void Start()
    {
        energy = startEnergy;
        audioSource = GetComponent<AudioSource>();
        stepAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        audioSource.loop = false;
        stepAudioSource.loop = false;
        stepAudioSource.clip = stepClip;
        stepAudioSource.volume = 0.6f;
        haveAnswer = false;
        inCorpse = false;
        base.Start();
        waitForSeconds = new WaitForSeconds(unfreezeDelay);
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        direction = Vector2.zero;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vunerable == walking)
        {
            vunerable = !walking;
        }

        base.Update();
        if (!freeze)
        {
            if (!dead)
            {
                if (!exiting)
                {
                    if (anim.GetBool("inCorpse") != inCorpse)
                    {
                        anim.SetBool("inCorpse", inCorpse);
                        anim.SetTrigger("Turn");
                    }
                    if (!walking)
                    {
                        GetDirection();
                        CheckMove();
                        
                    }
                    if (energy <= 0 && inCorpse)
                    {
                        inCorpse = false;
                        haveAnswer = false;
                        energy = 3;
                    }
                }
            }
            else
                freeze = true;
        }
        else
        {
            if (!unfreeze && !dead)
            {
                unfreeze = true;
                StartCoroutine(Unfreeze());
            }
        }
        if (dead && !ending)
        {
            ending = true;
            StartCoroutine(EndGame());
        }
        if (canExit && Input.GetKeyDown(KeyCode.Escape))
        {
            LevelManager.ToMenu();
        }
        else if (canExit && Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Restart();
        }
    }

    void StartMove(Vector3 pos)
    {
        walking = true;
        if(inCorpse)
            stepAudioSource.Play();
        finalPos = pos;
        if (finalPos == transform.position)
            walking = false;
    }

    IEnumerator EndGame()
    {
        audioSource.Stop();
        yield return waitForSeconds;
        canExit = true;
    }
    IEnumerator Unfreeze()
    {
        if (haveAnswer)
        {
            haveAnswer = false;
        }
        else
        {
            dead = true;
            energy = 0;
        }
        if (!dead)
        {
            audioSource.clip = enemyClip;
            audioSource.loop = false;
            audioSource.Play();
        }
        yield return new WaitForSeconds(0.51f);
        if (!dead)
            freeze = false;
        else if (spr.enabled)
        {
            audioSource.clip = enemyClip;
            audioSource.loop = false;
            audioSource.Play();
            spr.enabled = false;
        }
        unfreeze = false;
    }
    void CheckMove()
    {
        if (finalPos != transform.position && !freeze)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll((Vector2)transform.position + direction / 2, finalPos);
            if (hits.Length>0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    //print(hit.collider.name);
                    if (hit.collider.CompareTag("Corpse"))
                    {
                        audioSource.clip = corpseClip;
                        audioSource.loop = false;
                        audioSource.Play();

                        spr.color = inCorpseColor;
                        inCorpse = true;
                        haveAnswer = true;
                        energy = 5;
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.CompareTag("Finish"))
                    {
                        audioSource.clip = levelUpClip;
                        audioSource.loop = false;
                        audioSource.Play();
                        StartCoroutine(Exit());
                    }
                    else if (hit.collider.CompareTag("Enemy") && !hit.collider.isTrigger)
                    {
                        finalPos = transform.position;
                    }
                    else if (!hit.collider.isTrigger)
                    {
                        finalPos = transform.position;
                    }
                    if (energy < 0)
                    {
                        finalPos = transform.position;
                    }
                }
            }
            else
            {

                if (energy <= 0)
                {
                    if (inCorpse)
                    {
                        energy = 0;
                        spr.color = Color.white;
                        inCorpse = false;
                        haveAnswer = false;
                        energy = startEnergy;
                        finalPos = transform.position;
                    }
                    else
                    {
                        dead = true;
                        finalPos = transform.position;
                    }
                }
                else
                {
                    energy--;
                }
            }
            if (finalPos == transform.position)
            {
                walking = false;
            }
            else
                StartMove(finalPos);
        }
    }
    IEnumerator Exit()
    {
        exiting = true;
        yield return waitForSeconds;
        LevelManager.actualLevel++;
    }    
    void GetDirection()
    {
        int _x = (int)Input.GetAxisRaw("Horizontal");
        int _y = (int)Input.GetAxisRaw("Vertical");

        //Debug.Log(new Vector2(_x, _y).magnitude);
        if (new Vector2(_x,_y).magnitude==1)
        {
            if (_x == 0)
            {
                if (_y > 0)
                {
                    direction = Vector2.up;
                    if (anim.GetInteger("Direction") != 1)
                    {
                        anim.SetInteger("Direction", 1);
                        anim.SetTrigger("Turn");
                    }
                }
                else
                {
                    direction = Vector2.down;
                    if (anim.GetInteger("Direction") != 3)
                    {
                        anim.SetInteger("Direction", 3);
                        anim.SetTrigger("Turn");
                    }
                }
            }
            else
            {
                if (_x > 0)
                {
                    direction = Vector2.right;
                    if (anim.GetInteger("Direction") != 2)
                    {
                        spr.flipX = false;
                        anim.SetInteger("Direction", 2);
                        anim.SetTrigger("Turn");
                    }
                }
                else
                {
                    direction = Vector2.left;
                    if (anim.GetInteger("Direction") != 4)
                    {
                        spr.flipX = true;
                        anim.SetInteger("Direction", 4);
                        anim.SetTrigger("Turn");
                    }
                }
            }
            finalPos = transform.position + (Vector3)direction;
        }
        else
        {
            direction = Vector2.zero;
        }
    }
}
