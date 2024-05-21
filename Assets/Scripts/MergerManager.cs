using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class MergerManager : MonoBehaviour
{
    public Merger current;
    public Merger target;
    public Transform hitTr;

    public Transform mergeAreaParent;
    public MergeSlot[] mergeSlots;
    public List<Merger> mergers = new List<Merger>();
    public MergeSlot currentSlot;


    void Start()
    {
        mergeSlots = new MergeSlot[16];
        for (int i = 0; i < 16; i++)
        {
            mergeSlots[i] = mergeAreaParent.GetChild(i).GetComponent<MergeSlot>();
            mergeSlots[i].slotNo = i + 1;
        }
        mergers = FindObjectsOfType<Merger>().ToList();
        foreach (var m in mergers)
        {
            RegisterMergerToRandomSlot(m);
        }
    }


    public bool CanMerge()
    {
        return current != null && target != null && (current.level == target.level);
    }
    public bool CanSwap()
    {
        return current != null && target != null && (current.level != target.level);
    }

    public void RegisterMergerToRandomSlot(Merger m)
    {
        foreach (var slot in mergeSlots)
        {
            if (slot.IsEmpty)
            {
                slot.Insert(m);
                return;
            }
        }
    }
    public void RegisterMergerToSlot(MergeSlot otherSlot)
    {
        current.Release();
        if (otherSlot.IsEmpty)
        {
            otherSlot.Insert(current);
        }
        else
        { // check for Mergeable or not?
            if (CanMerge())
            {
                //change model of mergerA which is being dragged
                //delete the mergerB
                current.Merged();
                otherSlot.Insert(current);
                target.gameObject.SetActive(false);
                SetAnotherMerger(null);

            }
            else
            {
                currentSlot.Insert(current);
                //Reset current to its Original Slot
            }
        }
        current.GetComponent<Tower>().canShoot = true;
        currentSlot = null;
        current = null;

    }
    public void AssignMergerForDrag(Merger a)
    {
        if (a == null && current != null)
        {
            current.transform.parent = null;
        }
        if (a != null)
        {
            current = a;
            current.transform.parent = hitTr;
            var temp = mergeSlots.GetSlot(a);
            if (currentSlot != temp)
            {
                temp?.Remove();
                currentSlot = temp;
            }
            if (current != null)
            {
                current.GetComponent<Tower>().canShoot = false;
            }
            if (currentSlot != null)
            {
                $"Dragging {a.gameObject.name} from {currentSlot.name}".LOG();
            }
        }

    }
    public void SetAnotherMerger(Merger b)
    {
        target = b;
    }
    public bool MergerExists => current != null;

    public void Drag(Vector3 position)
    {
        if (current != null)
        {
            current.Drag(position);
        }
    }

    public bool ValidateSwap()
    {
        if (CanSwap())
        {
            current.SetPosition(target.transform.position);
            target.SetPosition(current.lastSlotPosition);
        }
        return false;
    }

    public bool AnyEmptySlot()
    {
        foreach (var item in mergeSlots)
        {
            if (item.IsEmpty)
            {
                return true;
            }
        }
        return false;
    }
}
