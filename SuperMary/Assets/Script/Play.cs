﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Play : MonoBehaviour
{
    #region 公開宣告
    [HideInInspector]///隱藏設定好的值
    [Header("移動速度")]
    public float Speed;

    [HideInInspector]///隱藏設定好的值
    [Header("跳耀調整值")]
    public float Hight;

    [HideInInspector]///隱藏設定好的值
    [Header("玩家剛體")]
    public Rigidbody2D Rid;

    [HideInInspector]///隱藏設定好的值
    [Header("玩家動畫")]
    public Animator Ani;

    [HideInInspector]///隱藏設定好的值
    [Header("生命次數")]
    public int HP;

    [HideInInspector]///隱藏設定好的值
    [Header("生命顯示")]
    public Text HPText;

    [HideInInspector]///隱藏設定好的值
    [Header("分數顯示")]
    public Text CoinText;

    [HideInInspector]///隱藏設定好的值
    [Header("死亡畫面")]
    public GameObject DeadGUI;

    [HideInInspector]///隱藏設定好的值
    [Header("死亡生命顯示")]
    public Text DeadHP;

    [HideInInspector]///隱藏設定好的值
    [Header("玩家物件")]
    public GameObject PlayGameObject;

    [HideInInspector]///隱藏設定好的值
    [Header("相機位置")]
    public GameObject Camera;

    [HideInInspector]///隱藏設定好的值
    [Header("死亡文字")]
    public Text Lose;

    [HideInInspector]///隱藏設定好的值
    [Header("勝利金幣")]
    public Text WinCoin;

    [HideInInspector]///隱藏設定好的值
    [Header("勝利生命")]
    public Text WinHP;

    [HideInInspector]///隱藏設定好的值
    [Header("勝利畫面")]
    public GameObject WinGUI;

    [HideInInspector]///隱藏設定好的值
    [Header("畫面")]
    public GameObject GUI;

    [HideInInspector]///隱藏設定好的值
    [Header("重新開始")]
    public GameObject RePlayGUI;

    [HideInInspector]///隱藏設定好的值
    [Header("退出")]
    public GameObject QuitGUI;

    [Header("音效")]
    public AudioSource PlayAudio;

    [Header("跳耀音效")]
    public AudioClip JumpAudio;

    [Header("死亡音效")]
    public AudioClip DeadAudio;

    [Header("吃金幣音效")]
    public AudioClip CoinAudio;

    [Header("吃道具音效")]
    public AudioClip EatAudio;
    #endregion

    #region 隱藏宣告
    [Header("分數")]
    private int Coin;

    [Header("跳耀值")]
    private float JumpHight;

    [Header("地板判斷開關")]
    private bool Floor;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //生命次數顯示
        HPText.text = HP.ToString();
        DeadHP.text = HP.ToString();
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(GUI);
    }

    // Update is called once per frame
    void Update()
    {
        // 走路方法
        Run();
    }

    void FixedUpdate()
    {
        // 跳耀方法
        Jump();
    }



    #region 玩家控制

    #region 移動
    /// <summary>
    /// 左右移動
    /// </summary>
    public void Run()
    {
        //  關閉走路動畫
        Ani.SetBool("走路", false);

        //  移動限制
        if (this.transform.position.x <= -92f)
        {
            this.transform.position = new Vector3(-92f, this.transform.position.y, this.transform.position.z);
        }
        //  移動限制
        if (this.transform.position.x >= 99f)
        {
            this.transform.position = new Vector3(99f, this.transform.position.y, this.transform.position.z);
        }

        //  走左邊
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Ani.SetBool("走路", true);
            this.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //  走右邊
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Ani.SetBool("走路", true);
            this.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    #endregion

    #region 跳耀
    /// <summary>
    /// 跳耀
    /// </summary>
    public void Jump()
    {
        //  按空白鍵跳躍
        if (Input.GetKey(KeyCode.Space) && Floor)
        {
            // 跳耀動畫
            Ani.SetBool("跳耀", true);
            PlayAudio.PlayOneShot(JumpAudio, 0.5f);
            Debug.Log(Hight * Time.deltaTime);     
            // 執行方法
            Rid.velocity = new Vector2(0, Hight * Time.deltaTime);
            Floor = false; 
        }
    }

    #endregion

    #region 碰撞框
    void OnCollisionEnter2D(Collision2D collision)
    {
        //  跳耀判斷開關
        if (collision.gameObject.tag == "地板")
        {
            Ani.SetBool("跳耀", false);
            Floor = true;
        }

        //死亡執行方法
        if (collision.gameObject.tag == "死亡")
        {
            PlayAudio.PlayOneShot(DeadAudio, 0.5f);
            Dead();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "金幣")
        {
            PlayAudio.PlayOneShot(CoinAudio, 0.5f);
            //得到分數執行方法
            CoinMain(collision);
        }

        if (collision.gameObject.tag == "勝利")
        {
            Win();
        }
    }
    #endregion

   

    #endregion

    #region 道具&分數&死亡

    #region 分數
    //得到分數 -> Trigger碰撞框
    /// <summary>
    /// 分數計算
    /// </summary>
    /// <param name="collision"></param>
    public void CoinMain(Collider2D collision)
    {
        Coin += 100;
        CoinText.text = Coin.ToString();
        Destroy(collision.gameObject);
    }

    #endregion

    #region 死亡
    //死亡扣生命
    /// <summary>
    /// 死亡扣生命
    /// </summary>
    public void Dead()
    {
        if (HP >= 1)
        {
            StartCoroutine(DeadMain(false));
            RePlayGUI.SetActive(false);
            QuitGUI.SetActive(false);
        }

        if (HP <= 0)
        {
            Lose.text = "You Lose ! Ha! Ha! Ha!";
            RePlayGUI.SetActive(true);
            QuitGUI.SetActive(true);
            StartCoroutine(DeadMain(true));
        }
    }

    /// <summary>
    /// 死亡協程
    /// </summary>
    /// <returns></returns>
    /// 死亡畫面流程
    public IEnumerator DeadMain(bool Dead)
    {
        //等待0.5秒開啟死亡畫面
        yield return new WaitForSeconds(0.5f);
        DeadGUI.SetActive(true);

        //等待0.5秒 扣生命 -> 更新GUI -> 重置玩家與相機座標
        yield return new WaitForSeconds(0.5f);
        //生命
        if (HP > 0)
        {
            HP--;
        }
        //GUI
        HPText.text = HP.ToString();
        DeadHP.text = HP.ToString();
        //座標
        PlayGameObject.transform.position = new Vector3(-90f, -3.2f, 0);
        Camera.transform.position = new Vector3(-90f, Camera.transform.position.y, Camera.transform.position.z);

        //等待1秒 關閉死亡畫面
        yield return new WaitForSeconds(1f);
        DeadGUI.SetActive(Dead);
    }

    #endregion

    #region 勝利
    /// <summary>
    /// 勝利畫面
    /// </summary>
    public void Win()
    {
        StartCoroutine(WinMain());
    }

    public IEnumerator WinMain()
    {
        //等待0.5秒開啟勝利畫面 && 更新介面
        yield return new WaitForSeconds(0.5f);
        WinHP.text = HP.ToString();
        WinCoin.text = Coin.ToString();
        WinGUI.SetActive(true);

        //等待1秒 執行總結
        yield return new WaitForSeconds(1f);
        while (Coin >= 1000)
        {
            PlayAudio.PlayOneShot(CoinAudio, 0.5f);
            Coin -= 1000;
            WinCoin.text = Coin.ToString();
            HP ++;
            WinHP.text = HP.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        //生命次數顯示
        HPText.text = HP.ToString();
        DeadHP.text = HP.ToString();
        CoinText.text = Coin.ToString();
    }
    #endregion


    #endregion

    #region 生命重置
    /// <summary>
    /// 生命重置
    /// </summary>
    public void ReHP()
    {
        HP += 3;
    }
    #endregion
}
