using UnityEngine;

public class GameTest : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Player player;

    [Header("기본 재료 아이템")]
    [SerializeField] private ItemData dew;
    [SerializeField] private ItemData herb;
    [SerializeField] private ItemData honey;
    [SerializeField] private ItemData smallManaCrystal;
    [SerializeField] private ItemData gladiolus;
    [SerializeField] private ItemData anemone;
    [SerializeField] private ItemData iris;
    [SerializeField] private ItemData clover;
    [SerializeField] private ItemData wood;
    [SerializeField] private ItemData ironIngot;
    [SerializeField] private ItemData mithrilIngot;
    [SerializeField] private ItemData leather;



    void Start()
    {
        if (player != null && player.Inventory != null)
        {
            player.Inventory.AddItem(dew, 99);
            player.Inventory.AddItem(herb, 99);
            player.Inventory.AddItem(honey, 99);
            player.Inventory.AddItem(smallManaCrystal, 99);
            player.Inventory.AddItem(gladiolus, 99);
            player.Inventory.AddItem(anemone, 99);
            player.Inventory.AddItem(iris, 99);
            player.Inventory.AddItem(clover, 99);
            player.Inventory.AddItem(wood, 99);
            player.Inventory.AddItem(ironIngot, 99);
            player.Inventory.AddItem(mithrilIngot, 99);
            player.Inventory.AddItem(leather, 99);

            Debug.Log("기본 재료 아이템 추가 완료!");
        }
    }

    void Update()
    {
    }
}