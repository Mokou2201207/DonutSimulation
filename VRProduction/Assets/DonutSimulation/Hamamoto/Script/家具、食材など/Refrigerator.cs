using UnityEngine;

public class Refrigerator : FurnitureOwner
{
    [Header("アニメーター自動")]
    [SerializeField] Animator m_animator;
    [SerializeField] bool m_DoorRefrigerator = false;
    private void Start()
    {
        // 子のAnimatorを全て取得
        m_animator = GetComponent<Animator>();
        //Keyの入力を入れる
        useKey = UseKey.Q;
        //UIを表示
        m_KeyHint = "Q";
    }
    public override void Interact()
    {
        //Qキーを押したらドアが開く
        if (!m_DoorRefrigerator)
        {
            m_animator.SetBool("Open", true);
                m_DoorRefrigerator = true;
        }
        else
        {
                    Debug.Log("冷蔵庫のドアが閉まった");
              m_animator.SetBool("Open", false);
                    m_DoorRefrigerator = false;
        }
    }


}
