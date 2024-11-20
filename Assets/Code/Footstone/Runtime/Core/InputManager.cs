using UnityEngine;
namespace Lost.Runtime.Footstone.Core
{

    public class InputManager
    {
        private const string MouseScrollWheel = "Mouse ScrollWheel";
        public float MouseWheelDelta { get => Input.GetAxis(MouseScrollWheel); }

        public bool IsKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public bool IsKeyPressed(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }

        public bool IsKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public Vector2 GetMousePosition()
        {
            return Input.mousePosition;
        }

        public Vector2 GetUIMousePoistion()
        {
            Vector2 pos = Input.mousePosition;
            pos.x -= Screen.width * 0.5f;
            pos.y -= Screen.height * 0.5f;
            return pos;
        }

        public bool IsLeftMouseButtonDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool IsLeftMouseButtonUp()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}



