using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenHandler : MonoBehaviour
{
    public int LevelChainLength = 1;
    public int CurrentSection = 0;

    public LevelSection[] LevelPrefabArray;

    public LevelSectionTransition[] LevelConnectorArray;

    public LevelSection[] CurrentLevelList;

    public GameObject FogWall;

    // Start is called before the first frame update
    void Start()
    {
        CurrentLevelList = new LevelSection[LevelChainLength];
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
            }
            else
            {
                //Make a new Section at the Right Transition of the Previous Section
                LevelSection newSection = GameObject.Instantiate<LevelSection>(LevelPrefabArray[(int)Mathf.Floor(Random.Range(0, LevelPrefabArray.Length))], CurrentLevelList[i-1].RightTransition.RightSidePosition.transform);
                //Set the Left Transition of the New Section to the Right Transition of the Previous Section
                newSection.LeftTransition = CurrentLevelList[i-1].RightTransition;

                //Connect a level transition section to the right side
                newSection.RightTransition = GameObject.Instantiate<LevelSectionTransition>(LevelConnectorArray[(int)Mathf.Floor(Random.Range(0, LevelConnectorArray.Length))], newSection.RightSidePosition.transform);

                CurrentLevelList[i] = newSection;
                newSection.InitializeSection();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        FogWall.transform.position = Vector3.Lerp(FogWall.transform.position, CurrentLevelList[CurrentSection].RightSidePosition.transform.position, .1f);
    }
}
