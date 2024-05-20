using UnityEngine;

public class MergeSlot : MonoBehaviour
{
    public int slotNo;
    public Merger currentlyHolding;

    public bool IsEmpty => currentlyHolding == null;
    public void Insert(Merger _a)
    {
        if (!_a)
        {
            return;
        }
        //        $"Inserting {_a.name} into {gameObject.name} Slot".LOG();
        currentlyHolding = _a;
        currentlyHolding.SetPosition(transform.position);
    }
    public void Remove()
    {
        //  $"Removing {currentlyHolding.name} from {gameObject.name} Slot".LOG();
        currentlyHolding = null;
    }

    public bool Match(Merger _a)
    {
        return currentlyHolding == _a;
    }
    public void Reset()
    {
        currentlyHolding.SetPosition(transform.position);
    }
    void Update()
    {
        if (currentlyHolding != null && !currentlyHolding.gameObject.activeInHierarchy)
        {
            Remove();
        }
    }
}
