using System;
using Convai.Scripts.Runtime.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UI button actions for spawning and controlling Convai NPCs.
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Button addNPCButton;
    [SerializeField] private Button removeNPCButton;
    [SerializeField] private Button ciaButton;
    [SerializeField] private Button airaButton;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private GameObject currentActiveNPC;
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private NPCSpawner npcSpawner;

    #endregion

    #region Constants

    public const string CIA_Name = "CIA";
    public const string CIA_CharacterID = "479ceb76-21ce-11f0-af55-42010a7be01a";
    public const string AIRA_Name = "AIRA";
    public const string AIRA_CharacterID = "35a2c95c-29b9-11f0-88ec-42010a7be01d";

    #endregion

    #region Private Fields

    private INPCService npcService;

    #endregion

    #region Unity Events

    private void Start()
    {
        npcService = FindObjectOfType<NPCSpawner>();

        addNPCButton.onClick.AddListener(OnAddNPC);
        removeNPCButton.onClick.AddListener(OnRemoveNPC);
        ciaButton.onClick.AddListener(() => AssignPersona(CIA_Name, CIA_CharacterID));
        airaButton.onClick.AddListener(() => AssignPersona(AIRA_Name, AIRA_CharacterID));
        undoButton.onClick.AddListener(npcService.UndoLastAction);
        redoButton.onClick.AddListener(npcService.RedoLastAction);
        messageInput.onSubmit.AddListener(SendMessage);

        airaButton.interactable = false;
        ciaButton.interactable = false;

        npcSpawner.commandManager.OnStateChanged += UpdateUndoRedoButtons;
        UpdateUndoRedoButtons(npcSpawner.commandManager.CanUndo(), npcSpawner.commandManager.CanRedo());
    }

    private void OnEnable()
    {
        ConvaiNPCManager.Instance.OnActiveNPCChanged += UpdateActiveEnemy;
        NPCSpawner.OnPersonaChangedAction += UpdateActiveEnemy;
    }

    private void OnDisable()
    {
        ConvaiNPCManager.Instance.OnActiveNPCChanged -= UpdateActiveEnemy;
        NPCSpawner.OnPersonaChangedAction -= UpdateActiveEnemy;
    }

    private void OnDestroy()
    {
        addNPCButton.onClick.RemoveListener(OnAddNPC);
        removeNPCButton.onClick.RemoveListener(OnRemoveNPC);
        ciaButton.onClick.RemoveAllListeners();
        airaButton.onClick.RemoveAllListeners();
        undoButton.onClick.RemoveListener(npcService.UndoLastAction);
        redoButton.onClick.RemoveListener(npcService.RedoLastAction);
        messageInput.onSubmit.RemoveListener(SendMessage);
    }

    #endregion

    #region Button Handlers

    private void OnAddNPC()
    {
        Vector3 spawnPoint = Vector3.zero; // This can be replaced with raycast
        npcService.SpawnWithUndo(spawnPoint);
    }

    private void OnRemoveNPC()
    {
        npcSpawner.UndoLastAction();
    }

    private void AssignPersona(string characterName, string characterID)
    {
        if (currentActiveNPC != null)
        {
            Debug.Log($"AssignPersona----charactername : {characterName} ---characterID : {characterID}");
            currentActiveNPC.GetComponent<NPCController>().SetPersona(characterName, characterID);
            HandlePersonaButtons(characterID);
        }
        else
        {
            Debug.Log("AssignPersona-----No Active enemy so cant change persona");
        }
    }

    private void SendMessage(string msg)
    {
        if (NPCSpawner.instance.activeNPCs.Count == 0) return;
        var last = NPCSpawner.instance.activeNPCs[^1];
        last.GetComponent<NPCController>().SendMessageToNPC(msg);
        messageInput.text = string.Empty;
        messageInput.ActivateInputField();
    }

    #endregion

    #region UI Updates

    private void UpdateUndoRedoButtons(bool canUndo, bool canRedo)
    {
        undoButton.interactable = canUndo;
        redoButton.interactable = canRedo;
    }

    private void UpdateActiveEnemy(ConvaiNPC obj)
    {
        if (obj)
        {
            Debug.Log("Setting active enemy");
            currentActiveNPC = obj.gameObject;
            HandlePersonaButtons(obj.characterID);
        }
        else
        {
            Debug.Log("No active enemy");
            currentActiveNPC = null;
            airaButton.interactable = false;
            ciaButton.interactable = false;
        }
    }

    private void HandlePersonaButtons(string characterID)
    {
        switch (characterID)
        {
            case AIRA_CharacterID:
                airaButton.interactable = false;
                ciaButton.interactable = true;
                break;
            case CIA_CharacterID:
                airaButton.interactable = true;
                ciaButton.interactable = false;
                break;
        }
    }

    #endregion
} 
