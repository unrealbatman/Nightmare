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
		public bool crouch;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


		public float interactRadius = 1.0f;

        private RaycastHit[] hit = new RaycastHit[1];



        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		// Add crouching SendMessage() receiver;
		// See Player Input component on the player 
		public void OnCrouch(InputValue value)
		{
			//Debug.Log(value.isPressed);
			CrouchInput(value.isPressed);
		}
        public void OnInteract(InputValue value)
        {
            //Debug.Log(value.isPressed);
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
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		// Crouching is implemented as a toggle
		private void CrouchInput(bool newCrouchState)
		{
			if (newCrouchState)
			{
				crouch = !crouch;
			}
		}
        private void InteractInput(bool value)
        {
            var ray = new Ray(CinemachineCameraTarget.transform.position, CinemachineCameraTarget.transform.forward);
            // ray-cast for interables
            int hits = Physics.RaycastNonAlloc(ray, hit, interactRadius, LayerMask.GetMask("LevelEntry"));
			//Debug.Log("hits: " + hits);
            if (hits > 0)
            {
				Debug.Log(hit[0].collider.gameObject);
				
				GameManager.Instance.LoadLevel(hit[0].collider.gameObject);
            }
        }
        private void OnDrawGizmos()
        {

            // Draw a blue line from the object's position to a point above it
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(CinemachineCameraTarget.transform.position, CinemachineCameraTarget.transform.position +interactRadius * CinemachineCameraTarget.transform.forward );
        }
    }
	
}