using UnityEngine;

public class Flayer : MonoBehaviour
{
    [Header("アニメーター自動")]
    [SerializeField] Animator[] m_animator;
    [SerializeField] bool m_FlayerIN = false;
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
    private void Update()
    {
        //Qキーを押したらドアが開く
        if (Input.GetKeyDown(KeyCode.F) && !m_FlayerIN)
        {
            foreach (var anim in m_animator)
            {
                Debug.Log("フライヤーを入れた");
                anim.SetBool("IN", true);
                m_FlayerIN = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F) && m_FlayerIN)
                foreach (var anim in m_animator)
                {
                    Debug.Log("フライヤーを出した");
                    anim.SetBool("IN", false);
                    m_FlayerIN = false;
                }
        }
    }
}
