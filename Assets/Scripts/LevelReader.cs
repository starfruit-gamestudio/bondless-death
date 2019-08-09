using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelReader : MonoBehaviour
{
    public Sprite sprite;

    Sprite[] sprites;

    public ColorToObject[] colorsAndObjects;

    string[] bs;

    Texture2D t;
    void Start()
    {
        //GetMap(PlayerPrefs.GetString("lastModifiedMap"));
    }

    public void GetMap(string map)
    {
        if (!File.Exists(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + map + ".txt"))
            return;

        string line = File.ReadAllText(System.Environment.SpecialFolder.MyDocuments + "\\GJP\\" + map + ".txt");
        bs = line.Split(',');
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
        t = newText;
        newText.SetPixels32(colorArray);
        ReadPixels(newText);
    }

    private void OnDisable()
    {
        Resources.UnloadUnusedAssets();
    }
    private void ConvertBytesToSprite()
    {
        byte[] byt = ConvertImageToByte(sprite);
        Color32[] c = ConvertBytesToColor(byt);
        Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var colorArray = new Color32[byt.Length / 4];
        for (var i = 0; i < byt.Length; i += 4)
        {
            var color = new Color32(byt[i + 0], byt[i + 1], byt[i + 2], byt[i + 3]);
            colorArray[i / 4] = color;
        }
        newText.SetPixels32(colorArray);
        //Sprite s = Sprite.Create(newText,new Rect(0,0,16,16), Vector2.zero);
        ReadPixels(newText);
    }

    public void ReadPixels(Sprite sprite)
    {
        Texture2D croppedTexture = textureFromSprite(sprite);
        for (int x = 0; x < croppedTexture.width; x++)
        {
            for (int y = 0; y < croppedTexture.height; y++)
            {
                for (int i = 0; i < colorsAndObjects.Length; i++)
                {
                    
                    if (colorsAndObjects[i].HasColor(sprite.texture.GetPixel(x, y)))
                    {
                        switch (colorsAndObjects[i].type)
                        {
                            case ColorToObject.Type.ground:
                                GameObject d = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                               // d.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                            case ColorToObject.Type.wall:
                                GameObject z = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                                //z.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                        }
                        GameObject ob = Instantiate(colorsAndObjects[i].prefab, new Vector2(x, y), Quaternion.identity, transform);
                       // ob.GetComponent<SpriteRenderer>().color = colorsAndObjects[i].color;
                    }
                }
            }
        }
    }
    public void ReadPixels()
    {
        Texture2D croppedTexture = textureFromSprite(sprite);
        for (int x = 0; x < croppedTexture.width; x++)
        {
            for (int y = 0; y < croppedTexture.height; y++)
            {
                for (int i = 0; i < colorsAndObjects.Length; i++)
                {

                    if (colorsAndObjects[i].HasColor(sprite.texture.GetPixel(x, y)))
                    {
                        switch (colorsAndObjects[i].type)
                        {
                            case ColorToObject.Type.ground:
                                GameObject d = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                                //d.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                            case ColorToObject.Type.wall:
                                GameObject z = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                                //z.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                        }
                        GameObject ob = Instantiate(colorsAndObjects[i].prefab, new Vector2(x, y), Quaternion.identity,transform);
                        //ob.GetComponent<SpriteRenderer>().color = colorsAndObjects[i].color;
                    }
                }
            }
        }
    }
    public void ReadPixels(Texture2D sprite)
    {
        Texture2D croppedTexture = sprite;
        for (int x = 0; x < croppedTexture.width; x++)
        {
            for (int y = 0; y < croppedTexture.height; y++)
            {
                for (int i = 0; i < colorsAndObjects.Length; i++)
                {
                    
                    if (colorsAndObjects[i].HasColor(croppedTexture.GetPixel(x, y)))
                    {
                        switch (colorsAndObjects[i].type)
                        {
                            case ColorToObject.Type.ground:
                                GameObject d = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                                //d.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                            case ColorToObject.Type.wall:
                                GameObject z = Instantiate(colorsAndObjects[(int)colorsAndObjects[i].type].prefab, new Vector2(x, y), Quaternion.identity, transform);
                                //z.GetComponent<SpriteRenderer>().color = colorsAndObjects[(int)colorsAndObjects[i].type].color;
                                break;
                        }
                        GameObject ob = Instantiate(colorsAndObjects[i].prefab, new Vector2(x, y), Quaternion.identity, transform);
                        //ob.GetComponent<SpriteRenderer>().color = colorsAndObjects[i].color;
                    }
                }
            }
        }
    }
    public byte[] ConvertImageToByte(Sprite image)
    {
// (image.texture.GetPixels32());
        byte[] bytes;
        List<byte> bytesL = new List<byte>();
        foreach (var item in image.texture.GetPixels32())
        {
            bytesL.Add(item.r);
            bytesL.Add(item.g);
            bytesL.Add(item.b);
            bytesL.Add(item.a);

        }
        //for (int x = 0; x < image.texture.width; x++)
        //{
        //    for (int y = 0; y < image.texture.height; y++)
        //    {
        //        bytesL.Add((byte)image.texture.GetPixel(x,y).r);
        //        bytesL.Add((byte)image.texture.GetPixel(x,y).g);
        //        bytesL.Add((byte)image.texture.GetPixel(x,y).b);
        //    }
        //}
        bytes = bytesL.ToArray();
        return bytes;
    }
    //public byte[] ConvertColorPosToByte(Sprite image)
    //{
    //    List<byte> bytes = new List<byte>();
    //    for (int x = 0; x < image.texture.width; x++)
    //    {
    //        for (int y = 0; y < image.texture.height; y++)
    //        {
    //            bytesL.Add((byte)image.texture.GetPixel(x, y).r);
    //            bytesL.Add((byte)image.texture.GetPixel(x, y).g);
    //            bytesL.Add((byte)image.texture.GetPixel(x, y).b);
    //        }
    //    }

    //    return bytes;
    //}

    public Color32[] ConvertBytesToColor(byte[] bytes)
    {
        List<Color32> color = new List<Color32>();
        for (int i = 0; i < bytes.Length / 3; i++)
        {
            color.Add(new Color(bytes[i], bytes[i + 1], bytes[i + 2]));
        }

        return color.ToArray();
    }
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            print(newColors.Length);
            newText.SetPixels(newColors);
            newText.Apply(false, false);
            return newText;
        }
        else
            return sprite.texture;
    }
    //public static Texture2D textureFromSprite(Sprite sprite)
    //{

    //        Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
    //        Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
    //                                                     (int)sprite.textureRect.y,
    //                                                     (int)sprite.textureRect.width,
    //                                                     (int)sprite.textureRect.height);
    //        print(newColors.Length);
    //        newText.SetPixels(newColors);
    //        newText.Apply(false, false);
    //        return newText;
    //}

    [System.Serializable]
    public class ColorToObject
    {
        public Type type;
        public Color color;
        public GameObject prefab;

        public bool HasColor(Color color)
        {
            if (this.color == color)
                return true;

            return false;
        }

        public enum Type
        {
            ignore = 2,
            wall = 0,
            ground = 1
        }
    }
}

//public KeyCode[] keyCodes;

//float timeBetween;

//public string text;
//private void Update()
//{
//    for (int i = 0; i < keyCodes.Length; i++)
//    {
//        if (Input.GetKeyDown(keyCodes[i]))
//        {
//            text += keyCodes[i].ToString();
//            timeBetween = 0;
//        }

//        else
//            timeBetween += Time.deltaTime;

//    }
//    if (timeBetween > 3)
//    {
//        timeBetween = 0;
//        text = String.Empty;
//    }
//    if (text == "ASD")
//    {
//        print("dafuq");
//        text = String.Empty;
//    }

//    //if(Input.GetKeyDown(keyCodes[]))
//}