using TMPro;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class Merger : MonoBehaviour
{
    public GameManager gameManager;
    public MergerManager mergerManager;
    public TextMeshProUGUI text;
    public int level;
    public RaycastHit hit => gameManager.raycastHit;
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
    }
    public void Merged()
    {
        level++;
        text.text = $"{level}";
        GetComponent<Tower>().LevelIncrease();
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
        isBeingDragged = true;

        SetPosition(point);
        // if (release)
        // {
        //     Release();
        //     if (mergerManager.ValidateSwap())
        //     {
        //         return;
        //     }
        //     if (mergerManager.ValidateMerge())
        //     {
        //         ///get new model and set this mergers stats
        //         ///Tower get tower and check
        //         level++;
        //         text.text = $"{level}";
        //     }

        //     SetPosition(point);
        //     return;
        // }
        // transform.parent = mergerManager.hitTr;
    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, GameManager.HeightOffset, pos.z);//- offset;

        lastSlotPosition = transform.position;
    }
}
