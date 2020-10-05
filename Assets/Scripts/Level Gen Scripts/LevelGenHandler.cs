using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LevelGenHandler : MonoBehaviour
{
    public PlayerScript PlayerPrefab;
    private PlayerScript InstantiatedPlayer = null;
    public SmoothFollow FollowCamera;

    private bool LevelsGenerated = false;

    public int LevelChainLength = 1;
    public int CurrentSection = 0;

    public LevelSection[] LevelPrefabArray;

    public LevelSectionTransition[] LevelConnectorArray;

    public LevelSection[] CurrentLevelList = null;

    public Text LevelText;
    public Text PlayerHealthText;

    public GameObject FogWall;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLoopLogic();
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelsGenerated)
        {
            FogWall.transform.position = Vector3.Lerp(FogWall.transform.position, CurrentLevelList[CurrentSection].RightSidePosition.transform.position, .1f);
        }
        else
        {
            FogWall.transform.position = Vector3.Lerp(FogWall.transform.position, this.transform.position + new Vector3 (10, 0, 0), .01f);
        }
        PlayerLoopLogic();
        LevelText.text = "Current Level: " + CurrentSection.ToString() + " / " + LevelChainLength.ToString();
        if(InstantiatedPlayer != null)
        {
            PlayerHealthText.text = "Player Health: " + InstantiatedPlayer?.PlayerHealthScript.GetHealth();
        }
        else
        {
            PlayerHealthText.text = "";
        }
    }

    public void GenerateLevels()
    {
        CurrentLevelList = new LevelSection[LevelChainLength];
        LevelSection previousSection = new LevelSection();
        for(int i = 0; i < LevelChainLength; i++)
        {
            if(i == 0)
            {
                //Make a new section at origin.
                LevelSection newSection = GameObject.Instantiate<LevelSection>(LevelPrefabArray[(int)Mathf.Floor(Random.Range(0, LevelPrefabArray.Length))], this.transform);

                newSection.transform.position += new Vector3(-30, 0, 0);

                //Connect a level transition section to the right side
                newSection.RightTransition = GameObject.Instantiate<LevelSectionTransition>(LevelConnectorArray[(int)Mathf.Floor(Random.Range(0, LevelConnectorArray.Length))], newSection.RightSidePosition.transform);

                //Track new section;
                CurrentLevelList[i] = newSection;
                previousSection = newSection;
                //Detach from parent
                newSection.transform.parent = null;
                newSection.LevelHandler = this;
                //This is the first section
                newSection.LeftLevelBlock.isInvincible = true;
            }
            else
            {
                //Make a new Section at the Right Transition of the Previous Section
                LevelSection newSection = GameObject.Instantiate<LevelSection>(LevelPrefabArray[(int)Mathf.Floor(Random.Range(0, LevelPrefabArray.Length))], previousSection.RightTransition.RightSidePosition.transform);
                //Set the Left Transition of the New Section to the Right Transition of the Previous Section
                newSection.LeftTransition = previousSection.RightTransition;

                //Connect a level transition section to the right side
                newSection.RightTransition = GameObject.Instantiate<LevelSectionTransition>(LevelConnectorArray[(int)Mathf.Floor(Random.Range(0, LevelConnectorArray.Length))], newSection.RightSidePosition.transform);

                CurrentLevelList[i] = newSection;
                newSection.InitializeSection();

                //Detach from parent
                newSection.transform.parent = null;
                newSection.LevelHandler = this;
                previousSection = newSection;
            }

        }
        LevelsGenerated = true;
    }

    public void ClearLevels()
    {
        LevelsGenerated = false;
        CurrentSection = 0;
        for(int i = 0; i < CurrentLevelList.Length; i++)
        {
            Destroy(CurrentLevelList[i].gameObject);
        }
        CurrentLevelList = null;
        FollowCamera.target = this.transform;
    }

    public void PlayerLoopLogic()
    {
        if(InstantiatedPlayer == null)
        {
            ClearLevels();
            GenerateLevels();
            InstantiatedPlayer = GameObject.Instantiate<PlayerScript>(PlayerPrefab, this.transform.position, Quaternion.identity);
            FollowCamera.target = InstantiatedPlayer.transform;
            CurrentSection = 0;
        }
    }

}
