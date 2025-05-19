using UnityEngine;

/// <summary>
/// Interface for managing NPC lifecycle operations like spawn, remove, and undo/redo actions.
/// </summary>
public interface INPCService
{
    /// <summary>
    /// Spawns an NPC at the given position with optional character ID, name, and prefab index.
    /// </summary>
    /// <param name="position">The position to spawn the NPC.</param>
    /// <param name="characterID">The character ID for the NPC persona (optional).</param>
    /// <param name="characterName">The character name for the NPC persona (optional).</param>
    /// <param name="prefabIndex">Index of the prefab to use. Defaults to random selection (-99).</param>
    /// <returns>The GameObject instance of the spawned NPC.</returns>
    GameObject SpawnNPC(Vector3 position, string characterID = null, string characterName = null, int prefabIndex = -99);

    /// <summary>
    /// Removes the specified NPC GameObject from the scene.
    /// </summary>
    /// <param name="npc">The NPC GameObject to remove.</param>
    void RemoveNPC(GameObject npc);

    /// <summary>
    /// Removes the last spawned NPC from the active stack.
    /// </summary>
    void RemoveFromStack();

    /// <summary>
    /// Spawns an NPC and registers the action for undo support.
    /// </summary>
    /// <param name="position">The position to spawn the NPC.</param>
    /// <param name="characterID">The character ID for the NPC persona (optional).</param>
    /// <param name="characterName">The character name for the NPC persona (optional).</param>
    /// <param name="prefabIndex">Index of the prefab to use. Defaults to random selection (-99).</param>
    void SpawnWithUndo(Vector3 position, string characterID = null, string characterName = null, int prefabIndex = -99);

    /// <summary>
    /// Undoes the last NPC-related action.
    /// </summary>
    void UndoLastAction();

    /// <summary>
    /// Redoes the previously undone NPC-related action.
    /// </summary>
    void RedoLastAction();
} 
