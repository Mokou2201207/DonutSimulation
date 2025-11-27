using UnityEngine;

public class Refrigerator : FurnitureOwner
{
    [Header("アニメーター自動")]
    [SerializeField] Animator[] m_animator;
    [SerializeField] bool m_DoorRefrigerator = false;
    private void Start()
    {
        // 子のAnimatorを全て取得
        m_animator = GetComponentsInChildren<Animator>();

        // 確認用
        foreach (var anim in m_animator)
        {
            Debug.Log("取得したAnimator: " + anim.gameObject.name);
        }
    }
    public override void Interact()
    {
        //Qキーを押したらドアが開く
        if (!m_DoorRefrigerator)
        {
            foreach (var anim in m_animator)
            {
                Debug.Log("冷蔵庫のドアが開いた");
                anim.SetBool("Open", true);
                m_DoorRefrigerator = true;
            }
        }
        else
        {
                foreach (var anim in m_animator)
                {
                    Debug.Log("冷蔵庫のドアが閉まった");
                    anim.SetBool("Open", false);
                    m_DoorRefrigerator = false;
                }
        }
    }


}
