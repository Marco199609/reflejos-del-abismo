using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private GameObject[] _inventoryButtons;
    [SerializeField] private SpriteRenderer[] _inventoryIconBackgrounds;
    [SerializeField] private Texture2D _genericCursor;
    [SerializeField] private Texture2D _interactableCursor;

    [field: SerializeField] public SpriteRenderer[] InventoryIcons;

    public static UIManager Instance { get; private set; }

    private RaycastHit mouseRaycastHit;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Update()
    {
        ManageCursor();
    }

    public void Message(string message)
    {
        StartCoroutine(DisplayMessage(message, 2));
    }

    private IEnumerator DisplayMessage(string message, float delay)
    {
        _messageText.gameObject.SetActive(true);
        _messageText.text = message;

        yield return new WaitForSeconds(delay);

        _messageText.text = "";
        _messageText.gameObject.SetActive(false);
    }

    public void SelectInventoryItem()
    {
        SoundManager.Instance.PlayClick();
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        for(int i = 0; i < _inventoryButtons.Length; i++)
        {
            if(button == _inventoryButtons[i].gameObject)
            {
                _inventoryIconBackgrounds[i].color = new Color(0, 1, 0, 0.2f);
                PlayerController.Instance.Inventory.UISelectedObject(i);
            }
            else
            {
                _inventoryIconBackgrounds[i].color = new Color (1, 1, 1, 0.2f);
            }
        }
    }

    private void ManageCursor()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseRaycastHit))
        {
            if (mouseRaycastHit.collider.GetComponent<Interactable>() == null)
            {
                Cursor.SetCursor(_genericCursor, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(_interactableCursor, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(_genericCursor, Vector2.zero, CursorMode.Auto);
        }  
    }
}
