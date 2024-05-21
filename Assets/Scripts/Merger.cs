using TMPro;
using UnityEngine;

public class Merger : MonoBehaviour
{
    public GameManager gameManager;
    public MergerManager mergerManager;
    public TextMeshProUGUI text;
    public RaycastHit hit => gameManager.raycastHit;
    public int level;
    public bool isBeingDragged;

    public Vector3 lastSlotPosition;
    // public Vector3 offset;
    void OnMouseDown()
    {
        mergerManager.AssignMergerForDrag(this);
    }
    public void Release()
    {
        isBeingDragged = false;
        transform.parent = null;
        GetComponent<Tower>().canShoot = true;
    }
    public void Merged()
    {
        level++;
        text.text = $"{level}";
        GetComponent<Tower>().LevelIncrease();
        GetComponent<Tower>().canShoot = true;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!isBeingDragged)
        {
            return;
        }
        var other = collider.GetComponent<Merger>();
        if (other != null)
        {
            mergerManager.SetAnotherMerger(other);
            $"{gameObject.name} Target Enter with {other.name}".LOG(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (!isBeingDragged)
        {
            return;
        }
        var other = collider.GetComponent<Merger>();
        if (other != null)
        {
            mergerManager.SetAnotherMerger(null);
            $"{gameObject.name} Target Exit with {other.name}".LOG(other.gameObject);
        }

    }

    public void Drag(Vector3 point)
    {
        GetComponent<Tower>().canShoot = false;
        isBeingDragged = true;

        SetPosition(point);
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, GameManager.HeightOffset, pos.z);//- offset;
        lastSlotPosition = transform.position;
    }
}
