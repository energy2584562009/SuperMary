using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("介面判斷開關")]
    public bool Start, Dead ,Win;


    [Header("下一關場景 && 重新開始 名稱")]
    public string Scene;
 
    [Header("勝利畫面")]
    public GameObject WinGUI;

    
    [Header("玩家")]
    public GameObject Play;

    
    [Header("相機")]
    public GameObject Camera;

   
    [Header("讀取介面")]
    public GameObject LandingGUI;

   
    [Header("進度條")]
    public Image ImageLoading;

    
    [Header("進度物件")]
    public RectTransform animatorGameobject;

   
    [Header("死亡介面")]
    public GameObject DeadGUI;

    [Header("開始介面")]
    public GameObject StartGUI;

    #region 下一關 && 重新開始
    /// <summary>
    /// 下一關 && 重新開始
    /// </summary>
    public void NextLevel()
    {
        //打開載入介面
        LandingGUI.SetActive(true);
        //協程處理載入畫面
        StartCoroutine(Loading());
        //初始位置
        Play.transform.position = new Vector3(-90, -3, 0);
        //相機位置
        Camera.transform.position = new Vector3(-90, Camera.transform.position.y, Camera.transform.position.z);
   
    }

    public IEnumerator Loading()
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
