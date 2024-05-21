using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    public const float HeightOffset = 2.2f;
    public MergerManager mergerManager;
    public Tower spawnTower;
    private Camera mainCamera;

    public bool HitSuccess;
    Ray ray;
    public RaycastHit raycastHit;
    public LayerMask moveLayer, snapLayer;
    public Vector3 hitPoint;
    LineRenderer lineRenderer;
    public Camera GetCamera => mainCamera;

    #region UI

    public Button spawnButton;
    public float spawnTowerDelay = 2f;

    #endregion

    public bool MouseDownInput
    {
        get;
        private set;
    }
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mergerManager = GetComponent<MergerManager>();
    }
    void Start()
    {
        mainCamera = Camera.main;
        spawnButton.onClick.RemoveAllListeners();
        spawnButton.onClick.AddListener(SpawnTower);
    }

    private void SpawnTower()
    {
        if (mergerManager.AnyEmptySlot())
        {
            var tower = Instantiate(spawnTower);
            tower.gameObject.SetActive(true);
            mergerManager.RegisterTowerToRandomSlot(tower.GetComponent<Merger>());
        }
        else
        {
            $"No Slot Is Empty".LOG();
        }
        StartCoroutine(SpawnButtonTimer());
    }
    IEnumerator SpawnButtonTimer()
    {
        spawnButton.interactable = false;
        yield return new WaitForSeconds(spawnTowerDelay);
        spawnButton.interactable = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseDownInput = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
        if (MouseDownInput)
        {
            Drag();
        }
        HitRayCast();
    }
    void Release()
    {
        MouseDownInput = false;
        if (!mergerManager.MergerExists)
        {
            return;
        }
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, snapLayer))
        {
            mergerManager.TryRegisterMergerToSlot(raycastHit.transform.GetComponent<MergeSlot>());
            // mergerManager.Move(raycastHit.transform.position, true);
        }
        else
        {
            mergerManager.ResetToOriginal();
        }
        // mergerManager.AssignMergerForDrag(null);

    }
    void HitRayCast()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, moveLayer))
        {
            lineRenderer.enabled = true;

            // HitTransform = raycastHit.transform;
            hitPoint = raycastHit.point;
            HitSuccess = true;
            lineRenderer.SetPositions(new Vector3[] { ray.origin, hitPoint });
            mergerManager.hitTr.position = hitPoint;
        }
        else
        {
            lineRenderer.enabled = false;
            HitSuccess = false;
            Release();
        }
    }
    void Drag()
    {
        mergerManager.Drag(hitPoint);
    }
}
