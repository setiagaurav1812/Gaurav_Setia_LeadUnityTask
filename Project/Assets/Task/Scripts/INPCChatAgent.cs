/// <summary>
/// Interface for controlling NPC interactions such as assigning personas, initiating and ending conversations, and sending text.
/// </summary>
public interface INPCChatAgent
{
    /// <summary>
    /// Assigns a persona to the associated NPC using name and character ID.
    /// </summary>
    void AssignPersona(string name, string characterID);

    /// <summary>
    /// Starts a conversation with the NPC.
    /// </summary>
    void StartConversation();

    /// <summary>
    /// Ends the current conversation with the NPC.
    /// </summary>
    void StopConversation();

    /// <summary>
    /// Sends a message to the NPC.
    /// </summary>
    void SendText(string message);
}