using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public const float HeightOffset = 2.2f;
    public MergerManager mergerManager;
    public Tower spawnTower;
    public int highestLevel;
    private Camera mainCamera;
    public  TextMeshProUGUI waveText;
    public int waveCounter = 0;

    public bool HitSuccess;
    Ray ray;
    public RaycastHit raycastHit;
    public LayerMask moveLayer, snapLayer;
    public Vector3 hitPoint;
    LineRenderer lineRenderer;
    
    public Camera GetCamera => mainCamera;

    [Header("UI")]
    #region UI
    [Header("Spawn Logic")]
    public float timeToFillElixir;
    private float fillRate;
    public float maxElixirAmount;
    public float currentElixirAmount;
    public float spawnCost;
    public Gradient gradient;
    public Button spawnButton;
    public Image elixirBar;
    public Button toggleMerge;
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

        toggleMerge.onClick.RemoveAllListeners();
        toggleMerge.onClick.AddListener(() =>
        {
            MergerManager.MERGE_ONLY = !MergerManager.MERGE_ONLY;
            bool value = MergerManager.MERGE_ONLY;
            toggleMerge.GetComponentInChildren<TextMeshProUGUI>().text = value ? "MergeOnly Mode" : "Freedom Mode";
        });
        waveCounter = 1;
    }

    private void SpawnTower()
    {
        if (mergerManager.AnyEmptySlot() && currentElixirAmount > spawnCost)
        {
            currentElixirAmount -= spawnCost;
            var tower = Instantiate(spawnTower);
            tower.gameObject.SetActive(true);
            mergerManager.RegisterTowerToRandomSlot(tower.GetComponent<Merger>());
        }
        else
        {
            //  $"No Slot Is Empty".LOG();
        }
        // StartCoroutine(SpawnButtonTimer());
    }
    IEnumerator SpawnButtonTimer()
    {
        spawnButton.interactable = false;
        yield return new WaitForSeconds(spawnTowerDelay);
        spawnButton.interactable = true;
    }

    private void Update()
    {
        ElixirLogic();
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
    private void ElixirLogic()
    {
        fillRate = maxElixirAmount / timeToFillElixir;
        spawnButton.interactable = currentElixirAmount >= spawnCost;
        currentElixirAmount += fillRate * Time.deltaTime;
        elixirBar.fillAmount = Mathf.InverseLerp(0, maxElixirAmount, currentElixirAmount);
        elixirBar.color = gradient.Evaluate(elixirBar.fillAmount);
        if (currentElixirAmount >= maxElixirAmount)
        {
            currentElixirAmount = maxElixirAmount;
        }
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

    public void CheckHigherLevel(int level)
    {
        if (level > highestLevel)
        {
            highestLevel = level;
        }
    }

    public void NextWave()
    {

        waveCounter++;
        waveText.text = $"Wave :{waveCounter}";
    }

  
}
