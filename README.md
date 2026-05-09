# Inventory Game

Unity 6 기반 2D RPG 인벤토리 시스템 구현 프로젝트
디자인 패턴 적용에 집중하여 확장 가능한 아이템/제작 및 인벤토리 시스템을 구현했습니다.

## 스크린샷(GIF)

player가 움직이고, Crafter와 상호작용하고, 아이템을 제작하고, 인벤토리에서 툴팁 확인하는 게임플레이 GIF입니다.
<img width="1112" height="840" alt="play1" src="https://github.com/user-attachments/assets/1f1c722d-3812-4896-a380-159f82c5a103" />



아이템들이 인벤토리의 TabButtons에 맞게 표시되며, Consumable 아이템을 사용하면 화면 상단의 PlayerStatsPanel에 변화가 생깁니다.
<img width="1112" height="840" alt="play2" src="https://github.com/user-attachments/assets/c00ed6e7-c590-45a3-8e27-71ebb8e3e4d8" />



## 핵심 구현 목표
1. 컴포넌트 패턴으로 아이템 기능을 조합 가능하게 설계
2. 옵저버 패턴으로 시스템 간 결합도 최소화
3. 오브젝트 풀로 SFX 성능 최적화
4. 인벤토리, 툴팁, 제작, 플레이어 HP/MP UI 구현
5. NPC 근접 시 상호작용 프롬프트 표시 및 E키를 상호작용 구현

## 기술 구현
### 1. 컴포넌트 패턴 (Component Pattern)
아이템의 효과, 착용 조건, 장비 스탯을 독립적인 ScriptableObject로 분리하여 조합하는 방식으로 설계했습니다.
새로운 아이템 타입을 추가할 때 기존 코드를 수정하지 않고 컴포넌트만 추가 및 조합하면 됩니다.
ItemComponent (추상 기반)
├── ItemEffect          - 아이템 사용 효과
│   ├── HealEffect      - 체력 회복
│   ├── ManaEffect      - 마나 회복
│   └── BuffEffect      - 스탯 버프 (지속시간 포함)
├── RequirementComponent - 착용 조건 검사
│   ├── LevelRequirement - 레벨 제한
│   ├── ClassRequirement - 직업 제한
│   └── StatRequirement  - 스탯 제한
└── EquipmentComponent   - 장비 스탯
    ├── WeaponComponent  - 공격력, 공격속도, 사거리
    └── ArmorComponent   - 방어력, 최대 체력
   
//아이템 사용 시 - 부착된 모든 효과 컴포넌트를 순회하여 실행
public void Use(GameObject target)
{
    var effects = GetComponents<ItemEffect>();
    foreach (var effect in effects)
    {
        effect.Apply(target);
    }
}

### 2. 옵저버 패턴 (Observer Pattern)
각 시스템이 서로를 직접 참조하지 않고 이벤트를 통해 통신합니다.
예시로, SoundManager는 Inventory, CraftingSystem을 구독하여 코드 변경 없이 사운드를 재생합니다.

```
// Inventory.cs - 이벤트 정의
public event Action OnInventoryChanged;
public event Action<ItemCategory> OnItemUsed;

// CraftingSystem.cs - 이벤트 정의
public event Action<RecipeData> OnCraftingStarted;
public event Action<RecipeData, bool> OnCraftingCompleted;
public event Action<CrafterType> OnCraftingUIOpened;
public event Action OnCraftingUIClosed;

// SoundManager.cs - 구독하여 자동 반응
void SubscribeToEvents()
{
    craftingSystem.OnCraftingStarted += OnCraftingStarted;
    craftingSystem.OnCraftingCompleted += OnCraftingCompleted;
    inventory.OnItemUsed += OnItemUsed;
}
```

UI 레이어에서도 같은 패턴을 적용했습니다.
InventorySlotUI는 마우스 이벤트만 감지하고, 실제 로직 처리는 InventoryUI가 담당합니다.
```
// InventorySlotUI.cs - 감지만 담당
public void OnPointerClick(PointerEventData eventData)
{
    OnSlotClicked?.Invoke();
}

// InventoryUI.cs - 로직 처리 담당
slotUI.OnSlotClicked += () => OnSlotClicked(index);
```

### 3. 오브젝트 풀 (Object Pool)
SFX 재생마다 AudioSource를 생성/삭제하면 GC 부하가 발생합니다.
미리 생성해둔 풀에서 꺼내 쓰고 반납하는 방식으로 이를 해결했습니다.
```
// 재생 요청 시 풀에서 꺼내기
AudioSource GetAvailableSFXSource()
{
    if (sfxPool.Count > 0)
        return sfxPool.Dequeue();

    if (activeSFX.Count > 0)
    {
        AudioSource oldest = activeSFX[0];
        activeSFX.RemoveAt(0);
        oldest.Stop();
        return oldest;
    }

    return CreateSFXSource();
}

// 재생 완료 후 자동 반납
IEnumerator ReturnToPoolWhenFinished(AudioSource source, float delay)
{
    yield return new WaitForSeconds(delay);
    source.gameObject.SetActive(false);
    sfxPool.Enqueue(source);
}
```

동시 재생 수에 따라 풀 크기가 자동으로 조절됩니다.
10번 재생해도 겹치지 않는다면 AudioSource 1개만 재사용합니다.

### 4. NPC 상호작용 시스템
플레이어가 NPC에 근접하면 상호작용 프롬프트가 자동으로 표시되고, E 키를 눌러 상호작용할 수 있습니다.
상호작용 가능한 오브젝트는 IInteractable 인터페이스를 구현하는 것만으로 시스템에 편입됩니다.

### 5. UI 구현
게임의 상태를 표현하는 4가지 UI 패널을 구현했습니다.

인벤토리 패널
탭 버튼과 슬롯 컨테이너로 구성되며, 탭 선택에 따라 아이템이 카테고리별로 필터링되어 표시됩니다.
I 키로 열고 닫으며, 열리는 동안 게임이 일시정지됩니다.

아이템 툴팁
슬롯에 마우스를 올리면 아이템의 상세 정보가 표시됩니다.
화면 밖으로 나가지 않도록 ClampToScreen()으로 위치를 보정합니다.

제작 패널
Crafter NPC에 접근해 E 키를 누르면 해당 NPC 타입에 맞는 레시피 목록이 표시됩니다.
보유 재료가 부족한 레시피는 반투명 오버레이로 표시되며, 제작 완료 시 결과물이 인벤토리에 자동으로 추가됩니다.

플레이어 스탯 패널
체력과 마나를 슬라이더와 텍스트로 실시간 표시합니다.
옵저버 패턴으로 PlayerHealth, PlayerMana의 이벤트를 구독하여 값이 변경될 때만 UI를 업데이트합니다.
