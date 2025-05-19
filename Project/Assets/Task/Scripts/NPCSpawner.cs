// Refactored NPCSpawner.cs with documentation and structured regions
using System;
using System.Collections.Generic;
using UnityEngine;
using Convai.Scripts.Runtime.Core;
using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns and manages Convai NPCs with support for persona assignment, undo/redo, and scene persistence.
/// </summary>
public class NPCSpawner : MonoBehaviour, INPCService
{
    #region Singleton

    public static NPCSpawner instance;

    #endregion

    #region Public Fields

    public GameObject[] npcPrefabs;
    public Collider roomCollider;
    public float spawnDistance = 2.5f;
    public float checkRadius = 0.4f;
    public LayerMask collisionMask;
    public List<GameObject> activeNPCs = new();
    public CommandManager commandManager = new();
    public static Action<ConvaiNPC> OnPersonaChangedAction;
    public GameObject groundPlane;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region NPC Spawning

    /// <summary>
    /// Spawns a Convai NPC at a given or computed location.
    /// </summary>
    public GameObject SpawnNPC(Vector3 position, string characterID = null, string characterName = null, int prefabIndex = -99)
    {
        Vector3 spawnPos = position;

        if (position == Vector3.zero)
        {
            Camera cam = Camera.main;
            Vector3[] tryDirs = {
                cam.transform.forward,
                Quaternion.Euler(0, 45, 0) * cam.transform.forward,
                Quaternion.Euler(0, -45, 0) * cam.transform.forward
            };

            foreach (var dir in tryDirs)
            {
                Vector3 testPos = cam.transform.position + dir.normalized * spawnDistance;
                testPos.y = 0;

                float dist = Vector3.Distance(testPos, roomCollider.ClosestPoint(testPos));
                bool inRoom = dist < 0.05f;
                bool noCollision = !Physics.CheckSphere(testPos, checkRadius, collisionMask);

                if (inRoom && noCollision)
                {
                    spawnPos = testPos;
                    break;
                }
            }

            if (spawnPos == Vector3.zero)
            {
                Debug.LogWarning("No space in room");
                return null;
            }
        }

        if (prefabIndex == -99)
        {
            prefabIndex = Random.Range(0, npcPrefabs.Length);
        }

        Quaternion rot = Quaternion.LookRotation(Camera.main.transform.position - spawnPos);
        GameObject prefab = npcPrefabs[Mathf.Clamp(prefabIndex, 0, npcPrefabs.Length - 1)];
        spawnPos += new Vector3(0, 0.1f, 0); // Avoid merging with ground

        GameObject npc = Instantiate(prefab, spawnPos, rot);
        var chat = new ConvaiChatService(npc.GetComponent<ConvaiNPC>());
        var controller = npc.GetComponent<NPCController>();
        controller.Initialize(chat);

        if (!string.IsNullOrEmpty(characterID) && !string.IsNullOrEmpty(characterName))
        {
            controller.SetPersona(characterName, characterID);
        }

        activeNPCs.Add(npc);
        return npc;
    }

    public void SpawnWithUndo(Vector3 position, string characterID, string characterName, int prefabIndex)
    {
        ICommand command = new SpawnNPCCommand(this, position, characterID, characterName, prefabIndex);
        commandManager.ExecuteCommand(command);
    }

    #endregion

    #region Undo/Redo

    [Button]
    public void UndoLastAction()
    {
        commandManager.Undo();
    }

    [Button]
    public void RedoLastAction()
    {
        commandManager.Redo();
    }

    #endregion

    #region NPC Removal

    public void RemoveNPC(GameObject npc)
    {
        if (npc != null)
        {
            Destroy(npc);
            activeNPCs.Remove(npc);
        }
    }

    public void RemoveFromStack()
    {
        if (activeNPCs.Count > 0)
        {
            GameObject npc = activeNPCs[^1];
            activeNPCs.RemoveAt(activeNPCs.Count - 1);
            Destroy(npc);
        }
        else
        {
            Debug.Log("No more NPCs to despawn");
        }
    }

    public void DestroyNPC(ConvaiNPC npc)
    {
        Destroy(npc);
    }

    #endregion

    #region Scene Persistence

    public void SaveScene()
    {
        var data = new SaveWrapper { npcs = new List<NPCData>() };
        foreach (var npc in activeNPCs)
        {
            var ctrl = npc.GetComponent<NPCController>();
            data.npcs.Add(new NPCData
            {
                characterID = ctrl.currentPersonaID,
                characterName = npc.name,
                position = npc.transform.position
            });
        }
        PlayerPrefs.SetString("Scene", JsonUtility.ToJson(data));
    }

    public void LoadScene()
    {
        string json = PlayerPrefs.GetString("Scene", "");
        if (string.IsNullOrEmpty(json)) return;

        var data = JsonUtility.FromJson<SaveWrapper>(json);
        foreach (var npc in activeNPCs) Destroy(npc);
        activeNPCs.Clear();

        foreach (var entry in data.npcs)
        {
            GameObject npc = SpawnNPC(entry.position);
            npc.GetComponent<NPCController>().SetPersona(entry.characterName, entry.characterID);
        }
    }

    #endregion
}

#region Serializable Save Data

[Serializable]
public class NPCData
{
    public string characterID;
    public string characterName;
    public Vector3 position;
}

[Serializable]
public class SaveWrapper
{
    public List<NPCData> npcs;
}

#endregion
