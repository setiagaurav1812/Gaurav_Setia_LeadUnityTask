using System;
using Convai.Scripts.Runtime.Core;
using UnityEngine;

/// <summary>
/// Controls an individual NPC by managing its chat agent and current persona.
/// </summary>
[DisallowMultipleComponent]
public class NPCController : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// The current persona ID assigned to this NPC.
    /// </summary>
    public string currentPersonaID { get; private set; }

    /// <summary>
    /// Event invoked when an active NPC is destroyed.
    /// </summary>
    public static Action ActiveEnemyDestroy;

    #endregion

    #region Private Fields

    private INPCChatAgent chatAgent;
    private ConvaiNPC convai;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the NPC with a provided chat agent.
    /// </summary>
    /// <param name="chatAgent">The agent to handle NPC chat logic.</param>
    public void Initialize(INPCChatAgent chatAgent)
    {
        this.chatAgent = chatAgent;

        if (this.chatAgent != null)
        {
            Debug.Log("Chat agent is not null");
        }
        else
        {
            Debug.Log("Chat agent is null");
        }

        convai = GetComponent<ConvaiNPC>();
    }

    #endregion

    #region Persona Handling

    /// <summary>
    /// Assigns a new persona to the NPC.
    /// </summary>
    /// <param name="name">The new character name.</param>
    /// <param name="personaID">The new character ID.</param>
    public void SetPersona(string name, string personaID)
    {
        currentPersonaID = personaID;
        chatAgent.AssignPersona(name, personaID);
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Starts a conversation with the NPC.
    /// </summary>
    public void BeginConversation()
    {
        chatAgent.StartConversation();
    }

    /// <summary>
    /// Ends the current conversation with the NPC.
    /// </summary>
    public void EndConversation()
    {
        chatAgent.StopConversation();
    }

    /// <summary>
    /// Sends a message to the NPC.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessageToNPC(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            chatAgent.SendText(message);
        }
    }

    #endregion
} 
