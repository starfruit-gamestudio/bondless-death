using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    public static int actualLevel;
    public static bool freeMode;
    //[SerializeField] Sprite[] levels;
    [SerializeField] Sprite[] levels;
    LevelReader levelReader;
    static bool reloadLevel;
    int startLevel;
    bool testMap;

    //Dictionary<int, Sprite> levels = new Dictionary<int, Sprite>();
    // Start is called before the first frame update
    void Start()
    {

        testMap = PlayerPrefs.GetInt("isTestMode") == 1 ? true : false;


        levelReader = GetComponent<LevelReader>();
        levels = Resources.LoadAll<Sprite>("levels") as Sprite[];

        if (testMap)
            levelReader.GetMap(PlayerPrefs.GetString("lastModifiedMap"));
        else if (!freeMode)
            actualLevel = 1;
        else
        {
            startLevel = actualLevel;
            LoadLevel(actualLevel - 1);
        }



    }
    // Update is called once per frame
    void Update()
    {
        if (testMap)
            return;

        if (!freeMode)
        {
            if (actualLevel > levels.Length)
            {
                SceneManager.LoadScene("Scene_MainMenu");
            }
            else if (levelReader.sprite != levels[actualLevel - 1])
            {
                LoadLevel(actualLevel - 1);
            }
        }
        else
        {
            if (actualLevel != startLevel)
            {
                SceneManager.LoadScene("Scene_MainMenu");
            }
        }
        if (reloadLevel)
        {
            reloadLevel = false;
            if (freeMode)
                LoadLevel(startLevel - 1);
            else
                LoadLevel(actualLevel - 1);
        }
    }


    public class LevelSprite
    {
        public int index;
        public Sprite levelSprite;

    }
    public static void ToMenu()
    {
        Player.freeze = false;
        Player.exiting = false;
        SceneManager.LoadScene("Scene_MainMenu");
    }
    public static void Restart()
    {
        Player.freeze = false;
        Player.exiting = false;
        reloadLevel = true;
    }
    public void RestartBtn()
    {
        Player.freeze = false;
        Player.exiting = false;

        if (testMap)
        {
            SceneManager.LoadScene("Scene_levelCreator");
            return;
        }

        reloadLevel = true;
    }

    void LoadLevel(int index)
    {
        levelReader.sprite = levels[index];
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);

        }
        Player.exiting = false;
        levelReader.ReadPixels();
    }

}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Linq;
//public class LevelManager : MonoBehaviour
//{
//    public static int actualLevel;
//    public static bool freeMode;
//    //[SerializeField] Sprite[] levels;
//    [SerializeField] Sprite[] levels;
//    LevelReader levelReader;
//    static bool reloadLevel;
//    int startLevel;
//    //Dictionary<int, Sprite> levels = new Dictionary<int, Sprite>();
//    // Start is called before the first frame update
//    void Start()
//    {
//        levelReader = GetComponent<LevelReader>();
//        levels = Resources.LoadAll<Sprite>("levels") as Sprite[];

//        if (!freeMode)
//            actualLevel = 1;
//        else
//        {
//            startLevel = actualLevel;
//            LoadLevel(actualLevel - 1);
//        }



//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (!freeMode)
//        {
//            if (actualLevel > levels.Length)
//            {
//                SceneManager.LoadScene("Scene_MainMenu");
//            }
//            else if (levelReader.sprite != levels[actualLevel - 1])
//            {
//                LoadLevel(actualLevel - 1);
//            }
//        }
//        else
//        {
//            if (actualLevel != startLevel)
//            {
//                SceneManager.LoadScene("Scene_MainMenu");
//            }
//        }
//        if (reloadLevel)
//        {
//            reloadLevel = false;
//            if(freeMode)
//                LoadLevel(startLevel - 1);
//            else
//                LoadLevel(actualLevel - 1);
//        }
//    }


//    public class LevelSprite
//    {
//        public int index;
//        public Sprite levelSprite;

//    }
//    public static void ToMenu()
//    {
//        Player.freeze = false;
//        Player.exiting = false;
//        SceneManager.LoadScene("Scene_MainMenu");
//    }
//    public static void Restart()
//    {
//        Player.freeze = false;
//        Player.exiting = false;
//        reloadLevel = true;
//    }
//    public void RestartBtn()
//    {

//        Player.freeze = false;
//        Player.exiting = false;
//        reloadLevel = true;
//    }

//    void LoadLevel(int index)
//    {
//        levelReader.sprite = levels[index];
//        if(GameObject.FindObjectOfType<Player>())
//            Destroy(GameObject.FindObjectOfType<Player>().gameObject);
//        for (int i = transform.childCount - 1; i >= 0; i--)
//        {
//            Destroy(transform.GetChild(i).gameObject);

//        }
//        Player.exiting = false;
//        levelReader.ReadPixels();
//    }

//}
