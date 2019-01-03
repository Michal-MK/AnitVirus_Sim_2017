﻿using UnityEngine;
using System.Collections;

public class ElecticalBlock : Enemy {
	public GameObject killerBlock;
	public GameObject warnSign;
	private Animator selfAnim;
	private BoxCollider2D selfCol;
	private RectTransform room1BG;

	public float timeKillerBlocksActive = 1.8f;
	public float timeCycleIdle = 0.2f;
	public float timeWarning = 1.5f;

	private bool despawn = false;

	private void Start() {
		enemyClass = EnemyClass.TOUCH;
		isDestroyable = false;
		selfAnim = killerBlock.GetComponent<Animator>();
		selfCol = killerBlock.GetComponent<BoxCollider2D>();
		room1BG = MapData.script.GetRoom(1).background;
		StartCoroutine(Cycle());
	}

	public IEnumerator Cycle() {
		while (!despawn) {
			selfCol.enabled = false;
			warnSign.SetActive(false);
			killerBlock.SetActive(true);

			yield return new WaitForSeconds(0.2f);
			selfCol.enabled = true;

			yield return new WaitForSeconds(timeKillerBlocksActive);
			selfAnim.SetTrigger("Despawn");
			selfCol.enabled = false;

			yield return new WaitForSeconds(timeCycleIdle);
			killerBlock.SetActive(false);
			Vector3 pos = GetNewPosition();
			killerBlock.transform.parent.position = pos;
			warnSign.SetActive(true);
			yield return new WaitForSeconds(timeWarning);
		}
		gameObject.SetActive(false);
		despawn = false;
	}

	private Vector3 GetNewPosition() {
		Vector3 killerblockpos = M_Player.player.transform.position;
		while (Vector2.Distance(M_Player.player.transform.position, killerblockpos) < 12) {

			float x = Random.Range(-room1BG.sizeDelta.x / 2 + 2, room1BG.sizeDelta.x / 2 - 2);
			float y = Random.Range(-room1BG.sizeDelta.y / 2 + 2, room1BG.sizeDelta.y / 2 - 2);
			killerblockpos = new Vector2(x, y);
		}
		return killerblockpos;
	}

	public void DespawnElBlock() {
		despawn = true;
	}
}