using UnityEngine;
/// <summary>
/// チョコレートを入れる鍋の処理
/// </summary>
public class Hotpot : FurnitureOwner
{
    [Header("ChocolateCoatingのscriptをアタッチ"), SerializeField]
    private ChocolateCoating m_HotpotChocolateCoating;

    [Header("PlayerPickupscriptをアタッチ"), SerializeField]
    private PlayerPickup m_PlayerPickup;

    [Header("鍋に入ってるチョコ"), SerializeField]
    private GameObject m_HptpotChoko;

    [Header("チョコレートを入れたかどうか"), SerializeField]
    public bool m_InChoko = false;

    [Header("チョコを鍋に入れるSE"), SerializeField]
    private AudioSource m_InChocolateSE;

    [Header("ドーナツをチョコに付けるSE"), SerializeField]
    private AudioSource m_CoatingChocolateSE;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //非表示
        m_HptpotChoko.SetActive(false);

        //Key入力とUI表示
        m_UseKey = UseKey.RightClick;
        m_KeyHint = "右クリック";

        //音を停止
        m_InChocolateSE.Stop();
        m_CoatingChocolateSE.Stop();
    }

    /// <summary>
    /// 鍋にチョコかどうかを調べる
    /// </summary>
    public override void Interact()
    {
        //何も持っていなかったら
        if (m_PlayerPickup.m_HaveItem == null)
        {
            Debug.Log("何も持ってません");
            return;
        }

        //手にチョコを持っていれば
        if (m_PlayerPickup.CheckHaveItem("Chocolate"))
        {

            m_PlayerPickup.UseItem();
            PutInChocolate();
        }
        else
        {
            Debug.Log("それはチョコじゃない！");
        }

        //中にチョコレートが入っており手にドーナツが入っていたら
        if (m_InChoko && m_PlayerPickup.CheckHaveItem("Dount"))
        {
            //再生
            m_CoatingChocolateSE.Play();
            //今手に持っているものをゲットコンポーネント
            Dount dount = m_PlayerPickup.m_HaveItem.GetComponent<Dount>();
            if (dount != null)
            {
                dount.DountChangeMaterial();
            }

        }

    }

    /// <summary>
    /// 鍋の中身がChocolateかどうか調べる
    /// </summary>
    /// <param name="other">鍋の中身</param>
    private void OnTriggerEnter(Collider other)
    {
        //レイヤーがItemじゃなかったら処理しない
        if (other.gameObject.layer != LayerMask.NameToLayer("Item")) return;

        //Chocolateなら持っているチョコレートを消して次の処理へ
        GameObject itemObj = other.gameObject;
        if (itemObj.CompareTag("Chocolate"))
        {
            bool success = PutInChocolate();
            if (success)
            {
                Destroy(itemObj);
            }
        }
    }

    /// <summary>
    /// 鍋にチョコレート入れる処理
    /// </summary>
    /// <returns>チョコが入ってなかったらtrue,満たされていたらfalse</returns>
    private bool PutInChocolate()
    {
        //中にチョコレートが入っていたら何もしない
        if (m_InChoko)
        {
            Debug.Log("もう中にチョコが入ってます");
            return false;
        }

        Debug.Log("チョコを鍋に");

        //表示
        m_HptpotChoko.SetActive(true);

        //フラグをオン
        m_InChoko = true;

        //再生
        m_InChocolateSE.Play();

        return true;
    }
}
