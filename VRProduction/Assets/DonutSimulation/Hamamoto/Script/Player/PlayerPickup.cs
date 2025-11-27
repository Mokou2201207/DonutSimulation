using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("拾える距離")]
    public float m_pickUpDistance = 5f;
    [Header("KeyUIをアタッチ")]
    public KeyUI m_KeyUI;
    private FurnitureOwner m_currentFurniture;
    private void Update()
    {
        //Rayをカメラの真ん中に設定
        Ray ray=Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
        //※Raycast が「何に当たったか？」を教えてくれるのが RaycastHit。
        RaycastHit hit;
        m_currentFurniture = null;
        bool furnitureHit = false;

        //3m以内でヒットしたら処理を行う
        if (Physics.Raycast(ray, out hit, m_pickUpDistance))
        {
            // FurnitureAction コンポーネントがあれば取得
            FurnitureOwner furniture = hit.collider.GetComponent<FurnitureOwner>();
            if (furniture != null)
            {
                m_currentFurniture = furniture;
                furnitureHit = true;
                Debug.Log("家具にクロスヘアが重なってます");
            }
        }
        // UIの表示/非表示は Raycast と FurnitureOwner の判定で決める
        //Image仮
        m_KeyUI.m_Image.enabled = furnitureHit;
        // Qキーでインタラクト
        if (m_currentFurniture != null && Input.GetKeyDown(KeyCode.Q))
        {
            m_currentFurniture.Interact();
        }
    }
}
