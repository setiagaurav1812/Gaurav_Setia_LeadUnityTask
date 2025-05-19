using Convai.Scripts.Runtime.Core;
using UnityEngine;

/// <summary>
/// Chat service handler for a ConvaiNPC to manage persona switching and messaging.
/// </summary>
public class ConvaiChatService : INPCChatAgent
{
    #region Private Variables

    private string personaID;
    private ConvaiNPC npc;

    #endregion

    #region Constructor

    public ConvaiChatService(ConvaiNPC npc)
    {
        this.npc = npc;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Assigns a new persona to the associated ConvaiNPC by destroying and reattaching the component.
    /// </summary>
    public void AssignPersona(string name, string characterID)
    {
        if (npc == null || ConvaiGRPCWebAPI.Instance == null)
        {
            Debug.Log("Returning as either instance is null or npc is null");
            return;
        }

        personaID = characterID;

        GameObject target = npc.gameObject;
        var oldNpc = npc.GetComponent<ConvaiNPC>();
        if (oldNpc != null)
            NPCSpawner.instance.DestroyNPC(oldNpc);

        var newNpc = target.AddComponent<ConvaiNPC>();
        npc = newNpc;
        newNpc.characterID = characterID;
        newNpc.characterName = name;
        newNpc.sessionID = "-1";

        Debug.Log($"Setting character id {characterID} --- character name : {name}");

        NPCSpawner.OnPersonaChangedAction?.Invoke(newNpc);
    }

    public void StartConversation() { /* Extend as needed */ }

    public void StopConversation() { /* Extend as needed */ }

    public void SendText(string message)
    {
        if (npc != null && !string.IsNullOrWhiteSpace(message))
        {
            // Extend here with proper text sending
        }
    }

    #endregion
} 
