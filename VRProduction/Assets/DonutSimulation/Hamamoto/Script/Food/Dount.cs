using UnityEngine;

public class Dount : MonoBehaviour
{
    [Header("コーティング対象"), SerializeField]
    private Renderer m_Renderer;

    [Header("チョコ素材"), SerializeField]
    private Material m_DarkMat;

    private void Awake()
    {
        m_Renderer.enabled = false;
    }

    /// <summary>
    /// ドーナツのコーティングのマテリアルを変更
    /// </summary>
    public void DountChangeMaterial()
    {
        Debug.Log("ドーナツの色を変更");
        //表示
        m_Renderer.enabled = true;
        //コーティングの色を変更
        m_Renderer.material = Instantiate(m_DarkMat);
    }


}
