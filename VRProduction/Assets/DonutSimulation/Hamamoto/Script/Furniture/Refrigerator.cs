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
    }

    /// <summary>
    /// 冷蔵庫のアニメーションの処理
    /// </summary>
    public override void Interact()
    {
        //Qキーを押したらドアが開く
        if (!m_DoorRefrigerator)
        {
            Debug.Log("冷蔵庫のドアが開いた");

            m_animator.SetBool("Open", true);

            //Doorフラグオン
            m_DoorRefrigerator = true;
        }
        else
        {
            Debug.Log("冷蔵庫のドアが閉まった");

            m_animator.SetBool("Open", false);

            //Doorフラグオフ
            m_DoorRefrigerator = false;
        }
    }


}
