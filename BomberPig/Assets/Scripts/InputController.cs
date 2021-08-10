using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private Pig pig;
    [SerializeField] private SimpleTouchController moveController;

    void Update()
    {
        var xDirection = moveController.GetTouchPosition.x;
        var yDirection = moveController.GetTouchPosition.y;

        pig.Move(new Vector3(xDirection, yDirection));
    }

    public void PlantBombButtonEvent()
    {
        pig.PlantBomb();
    }
}
