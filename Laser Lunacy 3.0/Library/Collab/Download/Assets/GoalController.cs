using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{

    public MoveGoal goal;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        if (tag == "Down")
        {
            goal.MoveDown();
        }
        if (tag == "Up")
        {
            goal.MoveUp();
        }
        if (tag == "Left")
        {
            goal.MoveLeft();
        }
        if (tag == "Right")
        {
            goal.MoveRight();
        }
    }
}
