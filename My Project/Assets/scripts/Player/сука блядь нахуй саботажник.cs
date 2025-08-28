using UnityEngine;
using UnityEngine.UI;

public class SabotageActions : MonoBehaviour
{
    public float interactRange = 3f; // Радиус действия
    public LayerMask playerLayer;     // Слой для других игроков
    public LayerMask buildingLayer;   // Слой для построек
    public Camera playerCamera;

    private GameObject currentTarget = null;
    private Text hintText; // Текст "E" над объектом

    void Update()
    {
        if (playerCamera == null) return;

        CheckTarget();

        if (currentTarget != null && Input.GetKey(KeyCode.E))
        {
            if (((1 << currentTarget.layer) & playerLayer) != 0)
            {
                KillPlayer(currentTarget);
            }
            else if (((1 << currentTarget.layer) & buildingLayer) != 0)
            {
                DestroyBuilding(currentTarget);
            }
        }
    }

    void CheckTarget()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        currentTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            currentTarget = hit.collider.gameObject;
            ShowHint(currentTarget, "E");
        }
        else
        {
            HideHint();
        }
    }

    void KillPlayer(GameObject player)
    {
        Debug.Log("Игрок " + player.name + " убит саботажником!");
        // Тут можно добавить анимацию смерти или отключение игрока
        Destroy(player);
    }

    void DestroyBuilding(GameObject building)
    {
        Debug.Log("Постройка " + building.name + " разрушена!");
        Destroy(building);
    }

    void ShowHint(GameObject target, string text)
    {
        if (hintText == null)
        {
            GameObject canvas = new GameObject("HintCanvas");
            canvas.transform.SetParent(target.transform);
            canvas.transform.localPosition = Vector3.up * 2f;

            GameObject goText = new GameObject("HintText");
            goText.transform.SetParent(canvas.transform);

            hintText = goText.AddComponent<Text>();
            hintText.alignment = TextAnchor.MiddleCenter;
            hintText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            hintText.color = Color.red;
        }

        hintText.text = text;
    }

    void HideHint()
    {
        if (hintText != null) hintText.text = "";
    }
}
