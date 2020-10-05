using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionTransition : MonoBehaviour
{

    public GameObject LeftSidePosition;
    public GameObject RightSidePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetLeftOffset()
    {
        return new Vector2(this.transform.position.x - LeftSidePosition.transform.position.x, LeftSidePosition.transform.position.y - this.transform.position.y);
    }

    public Vector2 GetRightOffset()
    {
        return new Vector2(RightSidePosition.transform.position.x - this.transform.position.x, RightSidePosition.transform.position.y - this.transform.position.y);
    }
}
