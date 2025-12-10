using UnityEngine;
/// <summary>
/// 冷蔵庫の処理
/// </summary>
public class Refrigerator : FurnitureOwner
{
    [Header("アニメーター自動"), SerializeField]
    private Animator m_animator;

    [Header("冷蔵庫のドアが開いたかどうか"), SerializeField]
    private bool m_DoorRefrigerator = false;

    [Header("処理中の無視フラグ"),SerializeField]
    private bool m_IsAnimating=false;

    [Header("冷蔵庫を開く音"), SerializeField]
    private AudioSource m_RefrigeratorOpenSE;

    [Header("冷蔵庫を閉める音"), SerializeField]
    private AudioSource m_RefrigeratorCloseSE;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // Animatorを全て取得
        m_animator = GetComponent<Animator>();

        //Keyの入力
        m_UseKey = UseKey.Q;

        //UIを表示
        m_KeyHint = "Q";

        //Soundを停止
        m_RefrigeratorCloseSE.Stop();
        m_RefrigeratorOpenSE.Stop();
    }

    /// <summary>
    /// 冷蔵庫のアニメーションの処理
    /// </summary>
    public override void Interact()
    {
        //アニメーション中なら処理しない
        if(m_IsAnimating)return;

        m_IsAnimating = true;

        //Qキーを押したらドアが開く
        if (!m_DoorRefrigerator)
        {
            Debug.Log("冷蔵庫のドアが開いた");

            m_animator.SetBool("Open", true);

            //Doorフラグオン
            m_DoorRefrigerator = true;

            //再生
            m_RefrigeratorOpenSE.Play();
        }
        else
        {
            Debug.Log("冷蔵庫のドアが閉まった");

            m_animator.SetBool("Open", false);

            //Doorフラグオフ
            m_DoorRefrigerator = false;

            //再生
            m_RefrigeratorCloseSE.Play();
        }

        //アニメーションに合わせる
        Invoke(nameof(ReleaseLock), 1.0f);
    }

    /// <summary>
    /// 開くor閉まるアニメーションが終わったらフラグを戻す処理
    /// </summary>
    private void ReleaseLock()
    {
        m_IsAnimating=false;
    }
}
