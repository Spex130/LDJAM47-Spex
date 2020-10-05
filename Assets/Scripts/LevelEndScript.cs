using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndScript : MonoBehaviour
{
    float timer = 3f;

    public DiamondBlock db;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(db == null)
        {
            timer-=Time.deltaTime;
        }
        if(timer <= 0)
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
