using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject highStock;
    [SerializeField] GameObject lowStock;
    [SerializeField]Image alert;
    [SerializeField]Transform grid;
    Image status;
    List<GameObject> stockList;
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<Image>();
        //grid = transform.GetChild(0);
        //alert = transform.GetChild(1).GetComponent<Image>();
        stockList = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            stockList.Add(Instantiate(lowStock, grid));
        }
        for (int i = 0; i < 5; i++)
        {
            stockList.Add(Instantiate(highStock, grid));
        }
    }

    // Update is called once per frame
    void Update()
    {
        alert.enabled = Player.haveAnswer;
        ManageBar();
        ManageStatus();
    }
    void ManageBar()
    {
        if (Player.energy > 0)
        {
            int stamina = 3;
            if (Player.inCorpse)
                stamina += Player.energy;
            else
                stamina = Player.energy;

            if (stamina == stockList.Count || CountActives() < stamina)
                for (int i = 0; i < stamina; i++)
                {
                    if (!stockList[i].active)
                        stockList[i].SetActive(true);
                }
            else
                for (int i = stockList.Count - 1; i >= stamina; i--)
                {
                    stockList[i].SetActive(false);
                }
        }
        else
        {
            for (int i = 0; i < stockList.Count; i++)
            {
                if (stockList[i].active)
                    stockList[i].SetActive(false);
            }
        }
    }

    void ManageStatus()
    {
        int index = Mathf.Clamp(CountActives(),0,sprites.Length-1);
        status.sprite = sprites[index];
    }

    int CountActives()
    {
        int result=0;
        for (int i = 0; i < stockList.Count; i++)
        {
            if (stockList[i].active)
                result++;
        }
        return result;
    }

}
