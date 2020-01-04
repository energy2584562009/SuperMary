using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
   
    [Header("玩家物件")]
    public GameObject Play;
    [Space]

    [Header("相機移動速度")]
    public float Speed;

    void Start()
    {
        //相機移動速度跟玩家同步
        Speed = Play.GetComponent<Play>().Speed;
        //不刪除物件
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        FollowPlay();
    }

    #region 鏡頭跟隨玩家
    /// <summary>
    /// 鏡頭跟隨玩家
    /// </summary>
    public void FollowPlay()
    {
        if (this.transform.position.x > -84f || this.transform.position.x < 90f)
        {
            //  跟隨玩家移動
            if (this.transform.position.x < Play.transform.position.x)
            {
                this.transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
            }

            if (this.transform.position.x > Play.transform.position.x)
            {
                this.transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);
            }
        }
       
        if(this.transform.position.x <= -84f || this.transform.position.x >= 90f)
        {
            if (this.transform.position.x < -84f)
            {
                this.transform.position = new Vector3(-84f, this.transform.position.y, this.transform.position.z);
            }

            if (this.transform.position.x > 90f)
            {
                this.transform.position = new Vector3(90f, this.transform.position.y, this.transform.position.z);
            }
        }
       
   
    }
    #endregion
}
