using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseHandler : MonoBehaviour
{
	public Canvas pauseScreen;
	public PlayerInputManager players;
	List<PlayerInput> playerList = new List<PlayerInput>();
	private void Awake() {
		pauseScreen.gameObject.SetActive(false);

		players.onPlayerJoined += player => {
			playerList.Add(player);
			player.currentActionMap.asset.FindAction("Pause").started += ctx => Pause(player);
			InputActionMap ui = player.actions.FindActionMap("UI");
			ui.FindAction("Unpause").started += ctx => Unpause(player);
			ui.FindAction("Quit").started += ctx => Quit(player);
		};

		players.onPlayerLeft += player => {
			playerList.Remove(player);
			Debug.Log("Player Left");
		};
	}

	PlayerInput pausingPlayer;

	void Pause(PlayerInput caller) {
		pausingPlayer = caller;
		players.DisableJoining();

		pauseScreen.gameObject.SetActive(true);

		foreach (PlayerInput p in playerList) {
			p.SwitchCurrentActionMap("UI");
		}
	}

	void Unpause(PlayerInput caller) {
		if (pausingPlayer != caller)	return;
		players.EnableJoining();
		pausingPlayer = null;

		pauseScreen.gameObject.SetActive(false);

		foreach (PlayerInput p in playerList) {
			p.SwitchCurrentActionMap("Player");
		}
	}

	void Quit(PlayerInput caller) {
		if (pausingPlayer != caller)	return;

		Destroy(caller.gameObject);
		Unpause(caller);
	}
}
