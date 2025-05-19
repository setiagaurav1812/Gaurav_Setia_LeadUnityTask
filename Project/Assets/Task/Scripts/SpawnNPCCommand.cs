using UnityEngine;

/// <summary>
/// Command to spawn an NPC with undo and redo capabilities.
/// </summary>
public class SpawnNPCCommand : ICommand
{
    #region Private Fields

    private INPCService npcService;
    private Vector3 position;
    private string characterID;
    private string characterName;
    private int prefabIndex;
    private GameObject spawnedNPC;

    #endregion

    #region Constructor

    public SpawnNPCCommand(INPCService npcService, Vector3 position, string characterID, string characterName, int prefabIndex)
    {
        this.npcService = npcService;
        this.position = position;
        this.characterID = characterID;
        this.characterName = characterName;
        this.prefabIndex = prefabIndex;
    }

    #endregion

    #region ICommand Implementation

    public void Execute()
    {
        spawnedNPC = npcService.SpawnNPC(position, characterID, characterName, prefabIndex);
    }

    public void Undo()
    {
        npcService.RemoveNPC(spawnedNPC);
    }

    public void Redo()
    {
        Execute();
    }

    #endregion
}