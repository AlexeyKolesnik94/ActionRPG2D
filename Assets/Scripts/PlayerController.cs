using UniRx;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    
    [SerializeField] private Animator animator;

    private const float SpeedMove = 10f;
    private Vector2 _moveDirection;
    private bool _isFacingRight;

    private Transform _playerTransform;
    private Controls _controls;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private static readonly int Speed = Animator.StringToHash("Speed");
    

    private void Awake() {
        _controls = new Controls();
        
    }

    private void Start() {
        _playerTransform = GetComponent<Transform>();
        Observable.EveryUpdate()
            .Subscribe(_ => {
                _moveDirection = _controls.Player.Move.ReadValue<Vector2>();
                Move(_moveDirection);
            }).AddTo(_disposable);
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
        _disposable?.Dispose();
    }


    private void Move(Vector2 direction) {
        float scaledMoveSpeed = SpeedMove * Time.deltaTime;
        Vector3 moveDirection = new Vector3(direction.x, direction.y,0);
        transform.position += moveDirection * scaledMoveSpeed;
        animator.SetFloat(Speed, _moveDirection.sqrMagnitude);
        
        switch (_isFacingRight) {
            case false when _moveDirection.x < 0:
            case true when _moveDirection.x > 0:
                Flip();
                break;
        }
    }
    
    private void Flip() {
        _isFacingRight = !_isFacingRight;
        Vector3 scaler = _playerTransform.localScale;
        scaler.x *= -1;
        _playerTransform.localScale = scaler;
    }
}