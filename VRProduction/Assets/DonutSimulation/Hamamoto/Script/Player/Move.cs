using System;
using System.Threading;
using UnityEngine;


public class Playercontroller : MonoBehaviour
{
    [Header("カメラのピボット（上下回転専用）")]
    public Transform m_CameraPivot;

    [Header("上下回転設定")]
    public float m_PitchSpeed = 60f;
    public float minPitch = -30f;
    public float maxPitch = 45f;
    private float m_Pitch = 0f;
    [Header("カメラスピード")]
    public float CameraSpeed;
    [Header("スピード")]
    public float moveSpeed;
    public bool isArea;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private Animator m_Animator;

    private void Start()
    {

        //格納
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
        //カーソルを中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        //カーソル非表示
        Cursor.visible = false;
    }

    private void Update()
    {
        //左右回転（プレイヤー本体）
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * CameraSpeed, 0);

        //上下回転（CameraPivot）
        float mouseY = Input.GetAxis("Mouse Y");

        m_Pitch -= mouseY * m_PitchSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);

        if (m_CameraPivot != null)
        {
            m_CameraPivot.localRotation = Quaternion.Euler(m_Pitch, 0f, 0f);
        }
    }

    void FixedUpdate()
    {

        // キャラクターとカメラの左右回転（Y軸）
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * CameraSpeed, 0));
        //移動
        float moveX = (Input.GetAxis("Horizontal") * moveSpeed);
        float moveZ = (Input.GetAxis("Vertical") * moveSpeed);

        //水平方向の速度を設定（Y速度は維持）
        Vector3 velocity = transform.right * moveX + transform.forward * moveZ;
        velocity.y = m_Rigidbody.linearVelocity.y;
        m_Rigidbody.linearVelocity = velocity;
        //キャラアニメーションで動く
        //m_Animator.SetFloat("X", Input.GetAxis("Horizontal"));
        //m_Animator.SetFloat("Y", Input.GetAxis("Vertical"));
    }






}
