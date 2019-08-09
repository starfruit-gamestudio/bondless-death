using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class LevelCreatorManager : MonoBehaviour
{
    public int size = 16;

    public GameObject tilePrefab;

    public Color selectedColor;
    public Sprite selectedSprite;


    public Camera mainCamera;

    [SerializeField] LayerMask lay;


    [SerializeField] GameObject objectPickUpPrefab;
    [SerializeField] GameObject panelParent;
    [SerializeField] Color[] colorsToShow;

    [SerializeField] SpritesAndColors[] spritesAndColors;

    WaitForSeconds waitOpacity = new WaitForSeconds(0.5f);

    List<Tile_LevelCreator> tiles = new List<Tile_LevelCreator>();
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SetColorsToShow();
        GenerateBlankTiles();
        //LoadLastMap();
    }
    public void GetMap(string map)
    {
        if (!File.Exists(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + map + ".txt"))
            return;

        string line = File.ReadAllText(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + map + ".txt");


        string[] bs = line.Split(',');
        List<byte> b = new List<byte>();
        for (int i = 0; i < 1024; i++)
        {
            b.Add(byte.Parse(bs[i]));
        }

        Color32[] c = ConvertBytesToColor(b.ToArray());
        Texture2D newText = new Texture2D(16, 16);
        var colorArray = new Color32[b.Count / 4];
        for (var i = 0; i < b.Count; i += 4)
        {
            var color = new Color32(b[i + 0], b[i + 1], b[i + 2], b[i + 3]);
            colorArray[i / 4] = color;
        }
        newText.SetPixels32(colorArray);
        ReadPixels(newText);
    }
    public Color32[] ConvertBytesToColor(byte[] bytes)
    {
        List<Color32> color = new List<Color32>();
        for (int i = 0; i < bytes.Length / 3; i++)
        {
            color.Add(new Color(bytes[i], bytes[i + 1], bytes[i + 2]));
        }

        return color.ToArray();
    }
    public void ReadPixels(Texture2D sprite)
    {
        Texture2D croppedTexture = sprite;
        for (int x = 0; x < croppedTexture.width; x++)
        {
            for (int y = 0; y < croppedTexture.height; y++)
            {
                GameObject ob = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                ob.GetComponent<SpriteRenderer>().color = sprite.GetPixel(x, y);
            }
        }
    }
    private void LoadLastMap()
    {
        GetMap(PlayerPrefs.GetString("lastModifiedMap"));
    }

    private void SetColorsToShow()
    {
        for (int i = 0; i < spritesAndColors.Length; i++)
        {
            GameObject ob = Instantiate(objectPickUpPrefab, panelParent.transform);
            Sprite s = spritesAndColors[i].sprite;
            Color c = spritesAndColors[i].color;
            ob.GetComponent<UnityEngine.UI.Image>().sprite = s;
            ob.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                SetActualColor(c);
                SetActualSprite(s);
            });
        }
    }

    private void SetActualSprite(Sprite sprite)
    {
        this.selectedSprite = sprite;
    }

    private void GenerateBlankTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector2(y, x), Quaternion.identity);
                tiles.Add(tile.GetComponent<Tile_LevelCreator>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
            ChangeColor();

    }

    private void ChangeColor()
    {
        Ray r = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction * 100);
        if (hit)
        {
            SetColorTo(selectedColor, hit.collider.gameObject);
            SetSpriteTo(selectedSprite, hit.collider.gameObject);
        }
    }

    private void SetSpriteTo(Sprite selectedSprite, GameObject gameObject)
    {
        if (!gameObject.GetComponent<Tile_LevelCreator>())
            return;

        gameObject.GetComponent<SpriteRenderer>().sprite = selectedSprite;
    }

    private void SetColorTo(Color selectedColor, GameObject gameObject)
    {
        if (!gameObject.GetComponent<Tile_LevelCreator>())
            return;

        gameObject.GetComponent<Tile_LevelCreator>().SetColor(selectedColor);
    }

    public void SetActualColor(Color selectedColor)
    {
        this.selectedColor = selectedColor;
    }

    public byte[] ConvertLevelToBytes()
    {
        List<byte> bytes = new List<byte>();
        for (int i = 0; i < tiles.Count; i++)
        {
            Color32 c32 = tiles[i].actualColor;

            bytes.Add(c32.r);
            bytes.Add(c32.g);
            bytes.Add(c32.b);
            bytes.Add(c32.a);

        }
        return bytes.ToArray();
    }
    #region unused
    [Obsolete]
    public void SaveBytesToImg()
    {
        print(Application.streamingAssetsPath + @"\tes2.bmp");
        print(Application.dataPath + "/tes2.bmp");
        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(size, size);

        for (int i = 0; i < tiles.Count; i++)
        {
            System.Drawing.Color c = System.Drawing.Color.FromArgb(tiles[i].GetColor32().r, tiles[i].GetColor32().g, tiles[i].GetColor32().b);
            bmp.SetPixel((int)tiles[i].transform.position.x, (int)tiles[i].transform.position.y, c);
        }
        bmp.Save(Application.dataPath + "/Resources/Levels/test.png", System.Drawing.Imaging.ImageFormat.Png);
    }
    #endregion

    public static string RandomString(int length)
    {
        System.Random random = new System.Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public void SaveBytesToTxt()
    {
        if (!Directory.Exists(System.Environment.SpecialFolder.MyDocuments + "\\GJP"))
            Directory.CreateDirectory(System.Environment.SpecialFolder.MyDocuments + "\\GJP");


        string s = RandomString(10);
        PlayerPrefs.SetString("lastModifiedMap", s);

        FileStream fs = File.Create(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + s + ".txt");

        fs.Dispose();
        fs.Close();

        fs = File.OpenWrite(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + s + ".txt");


        using (StreamWriter sw = new StreamWriter(fs))
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                sw.Write(tiles[i].GetColor32().r + "," + tiles[i].GetColor32().g + "," + tiles[i].GetColor32().b + "," + tiles[i].GetColor32().a + ",");
            }
        }
        fs.Dispose();
        fs.Close();
        LoadSceneTest();
    }

    public void LoadSceneTest()
    {
        Player.freeze = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("isTestMode", 1);
    }
    public void LoadSceneMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("isTestMode", 0);
    }


    IEnumerator SetOpacity(GameObject objectToSet)
    {
        var normalOpacity = objectToSet.GetComponent<SpriteRenderer>().color;
        var toGoColor = new Color(normalOpacity.r, normalOpacity.g, normalOpacity.b, normalOpacity.a / 2);
        objectToSet.GetComponent<SpriteRenderer>().color = Color.Lerp(normalOpacity, toGoColor, Time.deltaTime);
        yield return waitOpacity;
        objectToSet.GetComponent<SpriteRenderer>().color = Color.Lerp(toGoColor, normalOpacity, Time.deltaTime);

        yield return null;
    }

    [System.Serializable]
    public class SpritesAndColors
    {
        public Sprite sprite;
        public Color color;
    }
}
