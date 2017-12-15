﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData : MonoBehaviour {

	public GameObject[] doors;
	public static MapData script;

	private void Awake() {
		script = this;
	}

	private void Start() {
		int order = 0;
		for (int i = 0; i < doors.Length - 1; i += 2) {
			new Door(doors[i], order);
			new Door(doors[i + 1], order);
			order++;
		}
	}

	public void OpenDoor(int order) {
		foreach (Door d in Door.getDoors) {
			if (d.getDoorOrder == order) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(false);
				}
			}
		}
	}

	public void CloseDoor(int order) {
		foreach (Door d in Door.getDoors) {
			if (d.getDoorOrder == order) {
				foreach (GameObject g in d.getDoor) {
					g.SetActive(false);
				}
			}
		}
	}

	public void Progress() {
		switch (M_Player.gameProgression) {
			case 1: {
				OpenDoor(0);
				Canvas_Renderer.script.DisplayDirection(Directions.RIGHT);
				break;
			}
			case 2: {
				OpenDoor(1);
				Canvas_Renderer.script.DisplayDirection(Directions.TOP);
				break;
			}
			case 3: {
				OpenDoor(2);
				CloseDoor(0);
				Canvas_Renderer.script.DisplayDirection(Directions.BOTTOM);
				break;
			}
		}
		if (CameraMovement.script != null) {
			CameraMovement.script.RaycastForRooms();
		}
	}

	private void OnDestroy() {
		script = null;
	}
}