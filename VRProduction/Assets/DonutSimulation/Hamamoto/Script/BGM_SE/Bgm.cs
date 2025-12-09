using UnityEngine;
/// <summary>
/// BGMの設定
/// </summary>
public class Bgm : MonoBehaviour
{
    [Header("（自動）AudioSouseをアタッチ"), SerializeField]
    private AudioSource m_bgm;

    private void Start()
    {

        if (m_bgm == null)
        {
            m_bgm = GetComponent<AudioSource>();
        }

        //ループに設定して曲流す
        m_bgm.loop = true;
        m_bgm.Play();
    }

}
