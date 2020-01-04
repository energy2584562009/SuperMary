using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 介面宣告

    [Header("死亡介面")]
    [HeaderAttribute("-----介面-----")]
    public GameObject DeadGUI;
    [Header("開始介面")]
    public GameObject StartGUI;
    [Header("暫停介面")]
    public GameObject TimeUpGUI;
    [Header("勝利畫面")]
    public GameObject WinGUI;
    [Space]

    #endregion

    #region 開關宣告

    [Header("開始介面開關")]
    [HeaderAttribute("-----開關-----")]
    public bool Start;
    [Header("死亡介面開關")]
    public bool Dead;
    [Header("暫停介面開關")]
    public bool TimeUp;
    [Header("勝利介面開關")]
    public bool Win;
    [Space]

    #endregion

    #region 物件宣告

    [HeaderAttribute("-----物件-----")]
    [Header("玩家")]
    public GameObject Play;
    [Header("相機")]
    public GameObject Camera;
    [Header("讀取介面")]
    public GameObject LandingGUI;
    [Space]

    #endregion

    #region 其他宣告

    [HeaderAttribute("-----其他-----")]
    [Header("下一關場景")]
    public string Scene;
    [Space]

    [Header("進度條")]
    public Image ImageLoading;
    [Space]

    [Header("進度物件")]
    public RectTransform animatorGameobject;

    #endregion

    #region 下一關
    /// <summary>
    /// 下一關
    /// </summary>
    public void NextLevel()
    {
        //打開載入介面
        LandingGUI.SetActive(true);
        //協程處理載入畫面
        StartCoroutine(Loading(Scene));
        //初始位置
        Play.transform.position = new Vector3(-90, -3, 0);
        //相機位置
        Camera.transform.position = new Vector3(-90, Camera.transform.position.y, Camera.transform.position.z);
   
    }

    #endregion

    #region 重新開始

    public void RePlay()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        Scene ReScene = SceneManager.GetActiveScene();
        //打開載入介面
        LandingGUI.SetActive(true);
        //協程處理載入畫面
        StartCoroutine(Loading(ReScene.name));
        //初始位置
        Play.transform.position = new Vector3(-90, -3, 0);
        //相機位置
        Camera.transform.position = new Vector3(-90, Camera.transform.position.y, Camera.transform.position.z);
    }

    #endregion

    #region 下一關 && 重新開始 協程

    /// <summary>
    /// 下一關 && 重新開始 協程
    /// </summary>
    /// <param name="Scene"></param>
    /// <returns></returns>
    public IEnumerator Loading(string Scene)
    {
        //更換地圖
        AsyncOperation ao = SceneManager.LoadSceneAsync(Scene);
        //關閉自動換場景
        ao.allowSceneActivation = false;
        //設定換場景條件
        while (ao.isDone == false)
        {
            //進度變化顯示於介面上(進度條)
            ImageLoading.fillAmount = ao.progress / 0.9f;
            //進度變化顯示於介面上(物件)
            animatorGameobject.anchoredPosition = new Vector2(-700 + (1400f * (ao.progress / 0.9f)), 0);

            yield return null;
            //如果讀取進度到0.9才啟動換場景
            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true; 
            }
        }
        //關閉載入介面
        LandingGUI.SetActive(false);

        if (Start)
        {
            //關閉開始介面
            StartGUI.SetActive(false);
        }

        if (Win)
        {
            //關閉勝利介面
            WinGUI.SetActive(false);
        }
        if (Dead)
        {
            //關閉死亡介面
            DeadGUI.SetActive(false);
        }

        if (TimeUp)
        {
            //關閉暫停介面
            TimeUpGUI.SetActive(false);
        }
    }

    #endregion

    #region 遊戲暫停
    /// <summary>
    /// 遊戲暫停
    /// </summary>
    public void TimeOut()
    {
        //遊戲執行速率 = 0
        Time.timeScale = 0;
    }
    #endregion

    #region 遊戲繼續
    /// <summary>
    /// 遊戲繼續
    /// </summary>
    public void TimeStartUp()
    {
        //遊戲執行速率 = 1
        Time.timeScale = 1;
    }
    #endregion

    #region 結束遊戲
    /// <summary>
    /// 結束遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
