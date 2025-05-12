using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public AudioSource[] se;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// SEを鳴らす(0:ゲームオーバー 1:回復 2:被弾 3:攻撃 4:UI)
    /// </summary>
    /// <param name="x"></param>
    public void PlaySE(int x)
    {
        se[x].Stop();

        se[x].Play();
    }
}
