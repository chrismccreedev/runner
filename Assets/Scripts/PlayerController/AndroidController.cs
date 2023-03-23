using UnityEngine;

namespace VitaliyNULL.PlayerController
{
    public class AndroidController : SwipeController, IController
    {
        public void Run()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                IsSwiping = true;
                StartTouchPos = Input.mousePosition;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }
            CheckSwipe();
        }
    }
}