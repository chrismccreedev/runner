using UnityEngine;

namespace VitaliyNULL.PlayerController
{
    public class EditorController : SwipeController, IController
    {
        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsSwiping = true;
                StartTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
            
            CheckSwipe();
        }
    }
}