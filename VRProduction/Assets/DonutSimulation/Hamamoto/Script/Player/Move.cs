using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの操作とカメラの左右上下の処理
/// </summary>
public class Move : MonoBehaviour
{
    [Header("カメラのピボット（上下回転専用）"), SerializeField]
    private Transform m_CameraPivot;

    [Header("上下回転設定（ベース）"), SerializeField]
    private float m_PitchSpeed = 60f;

    [Header("左右回転設定（ベース）"), SerializeField]
    private float m_CameraSpeed = 120f;

    [Header("視点の下のMax設定"), SerializeField]
    private float m_MinPitch = -30f;

    [Header("視点の上のMax設定"), SerializeField]
    private float m_MaxPitch = 45f;

    [Header("視点の最初の位置を固定"), SerializeField]
    private float m_Pitch = 0f;

    [Header("移動スピード"), SerializeField]
    private float m_MoveSpeed;

    [Header("Debug用のText"), SerializeField]
    private Text m_debugText;

    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private PlayerInput m_PlayerInput;
    [SerializeField] private InputAction m_MoveAction;

    // UIで変更する倍率（Slider用）
    [Header("感度倍率（UI用）"), SerializeField]
    private float m_LookSensitivityMul = 1.0f;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // Rigidbody と PlayerInput を取得
        m_Rigidbody = GetComponent<Rigidbody>();
        m_PlayerInput = GetComponent<PlayerInput>();

        m_PlayerInput.currentActionMap.Enable();
        m_MoveAction = m_PlayerInput.actions.FindAction("Move");

        // カーソルを中央に固定して非表示
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 保存された感度を読み込み
        m_CameraSpeed = PlayerPrefs.GetFloat("LookSensitivity_H", m_CameraSpeed);
        m_PitchSpeed = PlayerPrefs.GetFloat("LookSensitivity_V", m_PitchSpeed);
        m_LookSensitivityMul = PlayerPrefs.GetFloat("LookSensitivity_Mul", 1f);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // 左右回転（プレイヤー本体）
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * m_CameraSpeed * m_LookSensitivityMul * Time.deltaTime, 0);

        // 上下回転（CameraPivot）
        float mouseY = Input.GetAxis("Mouse Y");
        m_Pitch -= mouseY * m_PitchSpeed * m_LookSensitivityMul * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);

        if (m_CameraPivot != null)
        {
            m_CameraPivot.localRotation = Quaternion.Euler(m_Pitch, 0f, 0f);
        }

        // デバッグ表示
        if (m_debugText != null)
        {
            m_debugText.text = string.Format("Move：{0}", m_MoveAction.ReadValue<Vector2>());
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    void FixedUpdate()
    {
        Vector2 input = m_MoveAction.ReadValue<Vector2>();
        float moveX = input.x * m_MoveSpeed;
        float moveZ = input.y * m_MoveSpeed;

        // 水平方向の速度を設定（Y速度は維持）
        Vector3 velocity = transform.right * moveX + transform.forward * moveZ;
        velocity.y = m_Rigidbody.linearVelocity.y;
        m_Rigidbody.linearVelocity = velocity;
    }

    // -------------------
    // UI用メソッド
    // -------------------
    public void SetHorizontalSensitivity(float value)
    {
        m_CameraSpeed = value;
        PlayerPrefs.SetFloat("LookSensitivity_H", value);
    }

    public void SetVerticalSensitivity(float value)
    {
        m_PitchSpeed = value;
        PlayerPrefs.SetFloat("LookSensitivity_V", value);
    }

    public void SetLookSensitivityMul(float value)
    {
        m_LookSensitivityMul = value;
        PlayerPrefs.SetFloat("LookSensitivity_Mul", value);
    }
}

