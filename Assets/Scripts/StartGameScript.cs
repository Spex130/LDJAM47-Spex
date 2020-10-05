using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GenTestScene");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("InstructionScreen");
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}
