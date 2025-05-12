using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Totitle()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlaySE(4);
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main");
        SoundManager.instance.PlaySE(4);
    }
}
