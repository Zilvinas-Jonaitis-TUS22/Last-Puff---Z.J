using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool vaping;
        public bool interacting;
        public bool inventoryState = false;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            if (!inventoryState)
            {
                MoveInput(value.Get<Vector2>());
            }
            else
            {
                MoveInput(Vector2.zero);
            }
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                if (!inventoryState)
                {
                    LookInput(value.Get<Vector2>());
                }
                else
                {
                    LookInput(Vector2.zero);
                }
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            if (!inventoryState)
            {
                SprintInput(value.isPressed);
            }
        }

        public void OnOpenInventory(InputValue value)
        {
            inventoryState = !inventoryState;
            if (inventoryState)
            {
                vaping = false;
                sprint = false;
                SetCursorState(!cursorLocked);
            }
            else
            {
                SetCursorState(cursorLocked);
            }
        }
        public void OnVaping(InputValue value)
        {
            if (!inventoryState)
            {
                VapeInput(value.isPressed);
            }
            

        }
        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void VapeInput(bool vapeState)
        {
            vaping = vapeState;
        }

        public void InteractInput(bool interactingState)
        {
            interacting = interactingState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}