﻿using UnityEngine;
using System.Collections;

public class AdvanceInGame : MonoBehaviour {
	public int currentStage = 0;
	void Update() {
		if (Input.GetKeyDown(KeyCode.F2)) {
			if (currentStage < 10) {
				currentStage++;
				switch (currentStage) {
					case 1: {
						for (int i = 0; i < 5; i++) {
							FindObjectOfType<Coin>().CoinBehavior(M_Player.player, null);
						}
						MapData.script.OpenDoor(new RoomLink(1, 2));
						FindObjectOfType<Spike>().transform.position = MapData.script.doors[0].transform.position;
						M_Player.player.transform.position = MapData.script.doors[0].transform.position;
						return;
					}
					case 2: {
						FindObjectOfType<BlockScript>().transform.position = FindObjectOfType<PressurePlate>().transform.position;
						MapData.script.OpenDoor(new RoomLink(2.ToString(), 3.ToString()));
						M_Player.player.transform.position = MapData.script.doors[2].transform.position;
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = M_Player.player.transform.position;
						return;
					}
					case 3: {
						FindObjectOfType<Avoidance>().avoidDuration = 2f;
						FindObjectOfType<Avoidance>().StartAvoidance();
						MapData.script.OpenDoor(new RoomLink(2, 3));
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = M_Player.player.transform.position;
						return;
					}
					case 4: {
						Vector3 pos = FindObjectOfType<MazeEntrance>().transform.position;
						Destroy(FindObjectOfType<MazeEntrance>().gameObject);
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos;
						M_Player.player.transform.position = pos;
						StartCoroutine(FindObjectOfType<MazeEscape>().FromMazeTrans());
						return;
					}
					case 5: {
						Vector3 pos = FindObjectOfType<BossEntrance>().transform.position;
						GameObject.Find("Collectibles").transform.Find("Spike").gameObject.SetActive(true);
						FindObjectOfType<Spike>().transform.position = pos + Vector3.down * 10;
						M_Player.player.transform.position = pos + Vector3.down * 10;
						FindObjectOfType<BombScript>().transform.position = pos + Vector3.down * 10;
						return;
					}
					case 6: {
						StartCoroutine(FirstBoss());
						return;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F1)) {
			if (currentStage > 0) {
				currentStage--;

			}
		}
	}
	private IEnumerator FirstBoss() {
		M_Player.player.transform.position = FindObjectOfType<BossEntrance>().transform.position;
		yield return new WaitForSeconds(3);
		GameObject g = FindObjectOfType<BossHealth>().healthIndicator.gameObject;
		yield return new WaitForSeconds(1);
		StartCoroutine(FindObjectOfType<BossHealth>().Death());
		FindObjectOfType<BossBehaviour>().StopAllCoroutines();

		Destroy(FindObjectOfType<BossBehaviour>().gameObject);
		MapData.script.OpenDoor(new RoomLink(5, 6));
		yield return new WaitForSeconds(1);
		Destroy(g);
		yield return new WaitUntil(() => CameraMovement.doneMoving);
		Camera.main.orthographicSize = CameraMovement.defaultCamSize;
	}
}