using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float health = 150f; 
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 200;


	private ScoreKeeper scoreKeeper;
	private bool hasStarted = false;


	void Start(){
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void Update () {

		float probability = Time.deltaTime * shotsPerSeconds;
		if(Random.value < probability){
			Fire();
		}
	}

	void Fire(){
		Vector3 startPosition = transform.position + new Vector3(0,-1,0);
		GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0){
				Destroy(gameObject);
				scoreKeeper.Score(scoreValue);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(hasStarted){
			AudioSource audio = GetComponent<AudioSource>();
			audio.Play();
			audio.Play(44100);
		}
	}
}
