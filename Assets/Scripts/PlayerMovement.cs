using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("이동 속도 설정")]
    public float speed = 5f;

    [Header("회전 속도 설정")]
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Animator animator;

    private InputAction moveAction;
    private Vector2 movementInput;

    private void Awake()
    {
        // CharacterController와 Animator 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 이동 액션 초기화
        moveAction = new InputAction("Move", binding: "<Keyboard>/w");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();

        // 이동 액션 이벤트 구독
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // 이동 액션 이벤트 구독 해제 및 비활성화
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        moveAction.Disable();
    }

    // 이동 입력이 수행될 때 호출
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    // 이동 입력이 취소될 때 호출
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movementInput = Vector2.zero;
    }

    private void Update()
    {
        // 입력을 3D 방향으로 변환
        Vector3 inputDirection = new Vector3(movementInput.x, 0, movementInput.y);
        Vector3 moveDirection = transform.TransformDirection(inputDirection) * speed;

        // CharacterController를 사용하여 이동
        controller.Move(moveDirection * Time.deltaTime);

        // Animator에 Speed 파라미터 설정
        float speedPercent = movementInput.magnitude;
        animator.SetFloat("Speed", speedPercent);

        // 이동 중일 때 플레이어 회전
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}