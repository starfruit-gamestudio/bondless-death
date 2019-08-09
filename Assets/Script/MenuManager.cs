using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    bool intro;
    WaitForSeconds menuDelay;
    Animator backgroundAnim;
    AudioSource audio;
    [SerializeField] float menuDelayTime;

    [SerializeField] UnityEvent menuEvent;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        menuDelay = new WaitForSeconds(menuDelayTime);
        backgroundAnim = GameObject.Find("BackGround").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!intro && Input.anyKey)
        {
            audio.Play();
            backgroundAnim.SetBool("Skip", true);
            intro = true;
            StartCoroutine(ShowMenu());
        }
    }
    IEnumerator ShowMenu()
    {
        yield return menuDelay;
        menuEvent.Invoke();
    }
    public void ToCampaign()
    {
        PlayerPrefs.SetInt("isTestMode", 0);
        LevelManager.freeMode = false;
        SceneManager.LoadScene("Scene_GameLevel");
    }

    public void ToFreeMode()
    {
        SceneManager.LoadScene("Scene_GameLevel");
    }

    public void ToEditMode()
    {
        SceneManager.LoadScene("Scene_LevelCreator");
    }

    public void SelectLevel(int index)
    {
        LevelManager.freeMode = true;
        LevelManager.actualLevel = index;
        ToFreeMode();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
