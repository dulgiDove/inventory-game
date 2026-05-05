using UnityEngine;

public class GameTest : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Player player;

    [Header("테스트 아이템")]
    [SerializeField] private ItemData smallHealthPotion;
    [SerializeField] private ItemData smallManaPotion;
    [SerializeField] private ItemData smallElixir;
    [SerializeField] private ItemData ironSword;

    void Start()
    {
        // 시작 시 기본 아이템 추가
        if (player != null && player.Inventory != null)
        {
            player.Inventory.AddItem(smallHealthPotion, 3);
            player.Inventory.AddItem(smallManaPotion, 3);
            player.Inventory.AddItem(smallElixir, 3);

            player.Inventory.AddItem(ironSword, 1);

            Debug.Log("테스트 아이템 추가 완료!");
        }
    }

    void Update()
    {
        // 1번 키: 체력 물약 추가
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.Inventory.AddItem(smallHealthPotion, 1);
        }

        // 2번 키: 철 검 추가
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.Inventory.AddItem(ironSword, 1);
        }
    }
}