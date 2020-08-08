using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public const string fireButtonName = "Fire1";
    public const string jumpButtonName = "Jump";
    public const string moveHorizontalAxisName = "Horizontal";
    public const string moveVerticalAxisName = "Vertical";
    public const string reloadButtonName = "Reload";

    public Vector2 moveInput { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public bool jump { get; private set; }
    
    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            moveInput = Vector2.zero;
            fire = false;
            reload = false;
            jump = false;
            return;
        }

        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        jump = Input.GetButtonDown(jumpButtonName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}