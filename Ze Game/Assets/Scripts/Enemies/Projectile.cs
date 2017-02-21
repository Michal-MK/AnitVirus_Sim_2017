using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public static float projectileSpeed = 15;
	public bool ready = false;
	public static bool spawnedByAvoidance = false;
	public Rigidbody2D self;
	public bool byBoss = false;

	void OnEnable() {
		if (!byBoss) {
			ready = true;
			self.velocity = transform.rotation * Vector3.down * projectileSpeed;
		}
		if (byBoss) {
			StartCoroutine(BossAttack());
		}
	}

	private IEnumerator BossAttack() {
		yield return new WaitForSeconds(1);
		self.velocity = transform.rotation * Vector3.down * projectileSpeed;
		StopCoroutine(BossAttack());
	}


	private void OnTriggerEnter2D(Collider2D col){

		if (col.tag == "Wall" || col.tag == "Wall/Door") {
			print('A');
			gameObject.SetActive (false);

		}
	}

	private void OnTriggerExit2D(Collider2D col){
		if (col.tag == "BG") { 
			gameObject.SetActive (false);
			print('B');

		}
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.tag == "Wall" || col.transform.tag == "Wall/Door") {
			gameObject.SetActive(false);
			print('C');

		}
	}


	void OnDisable(){
		ready = false;
		byBoss = false;
	}
}
