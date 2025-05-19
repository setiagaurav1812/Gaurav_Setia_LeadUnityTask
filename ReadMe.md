# Convai Unity WebGL NPC Interaction System

Demo Link : 
https://cheery-rolypoly-cff015.netlify.app/

This system provides a modular and extensible setup to integrate Convai-powered NPCs in a Unity WebGL project. It includes runtime persona switching, undo/redo support, spawn control, and UI integration.

## 🚀 Features
	•	Dynamic NPC Spawning
	•	Persona Switching (WebGL-safe via Component Rebuild)
	•	Undo/Redo System
	•	Scene Save/Load Persistence- not implemented completely due to shortage of time.
	•	UI Buttons for NPC Management
	•	FPS Display UI

## 📁 Folder Structure
	•	NPCSpawner.cs — Manages NPC instantiation and removal
	•	NPCController.cs — Handles individual NPC logic and persona
	•	ConvaiChatService.cs — Connects controller to Convai backend via INPCChatAgent
	•	UIManager.cs — Handles button clicks and UI logic
	•	CommandManager.cs — Manages undo/redo stack
	•	ICommand.cs — Base interface for commands
	•	SpawnNPCCommand.cs — Implements command for NPC spawn
	•	INPCChatAgent.cs — Interface for NPC chat behavior
	•	INPCService.cs — Interface for NPC spawning/removal

## 🧠 Design Principles
	•	SOLID and modular architecture
	•	Command Pattern for undo/redo
	•	Scriptable Architecture using interfaces and manager classes
	•	WebGL Compatibility (avoids microphone APIs, supports persona swap via component recreation)

## ✅ Usage Flow
	1	Spawn NPC: Click Add NPC button
	2	Talk to NPC: Input and interact
	3	Switch Persona: Use CIA or AIRA buttons
	4	Undo/Redo: Step through NPC spawn/remove history
	5	Save/Load: Automatically persists via PlayerPrefs

## 🛠️ Setup Requirements
	•	Unity 2022.3+
	•	Convai WebGL SDK imported
	•	Assigned prefab with ConvaiNPC + Animator + AudioSource
	•	Ground Plane and Room Bounds assigned
	•	UI Buttons connected to UIManager
## 🎮 UI Controls Overview
The scene includes a set of on-screen buttons used to control NPC interactions and system state:
| **Button**     | **Function**                                                                |
| -------------- | --------------------------------------------------------------------------- |
| **Add NPC**    | Spawns a new NPC at a valid point near the player using the spawner logic.  |
| **Remove NPC** | Removes the most recently added NPC using the undo system.                  |
| **CIA**        | Switches the active NPC's persona to **CIA** using its character ID.        |
| **AIRA**       | Switches the active NPC's persona to **AIRA** using its character ID.       |
| **Undo**       | Undoes the last NPC spawn or removal using the command pattern.             |
| **Redo**       | Redoes the last undone action, re-adding or removing an NPC as appropriate. |

These buttons are connected via UIManager.cs and respond to user input during runtime. Persona buttons (CIA, AIRA) are dynamically enabled or disabled based on the active NPC's current persona.

## ✅ Optimizations Implemented
To ensure smooth performance and efficient delivery in the WebGL build, the following optimizations were applied:
	1	Gzip Compression
	◦	The project is built with Gzip compression enabled to reduce download size and loading time on supported web servers.
	◦	This significantly improves initial load performance for browser users.
	2	Static Batching
	◦	Static batching is enabled in Player Settings, and static flags are applied to all non-movable scene objects (e.g., environment geometry).
	◦	This helps reduce draw calls and CPU overhead by combining static meshes at build time.
	3	Baked Lighting
	◦	The environment uses baked global illumination to avoid expensive real-time lighting calculations.
	◦	All lighting for static objects is precomputed, allowing faster rendering in WebGL builds and reducing GPU load.
These optimizations collectively help ensure the project runs smoothly even within the performance constraints of WebGL platforms.

## 🎨 Assets Used in Project
	1	Inspector Debug Utility: NaughtyAttributes
	2	Environment: Furnished Cabin
	3	UI Theme: Sci-Fi GUI Skin

## 🌐 WebGL Notes
	•	Persona switching is handled by destroying and re-adding ConvaiNPC
	•	Mic input is not used (Text input and JS interop only)
	•	Avoid using PlayerPrefs for sensitive data — extend with cloud sync if needed

## ✨ Contributors
Built with Edifier
