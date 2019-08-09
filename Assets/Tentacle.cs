using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{

    Animator anim;
    AudioSource audio;
    WaitForSeconds waitForSeconds;
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        waitForSeconds = new WaitForSeconds(0.3f);
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Attack());
        }

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("TentacleAttack");
        

        Player.energy -= 2;
        print(Player.energy);
        yield return waitForSeconds;
        audio.Play();
        //Debug.Log(anim);
    }
}
