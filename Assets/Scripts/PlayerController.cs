using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float padding;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250f;

	private bool hasStarted = false;

	float xmin;
	float xmax;

	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}

	void Fire(){
		Vector3 offset = new Vector3(0, 1, 0);
		GameObject beam = Instantiate(projectile, transform.position+offset, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);

	}
	// Update is called once per frame
	void Update () {

		MoveShip();
		LaserShoot();
        if (ScoreKeeper.score > 600)
        {
            WinGame();
        }
	}

	void MoveShip(){

		if (Input.GetKey(KeyCode.LeftArrow)){
			//transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			//transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		}

	//restrict the player to the gamespace
	float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
	transform.position = new Vector3(newX, transform.position.y, transform.position.z);
}

	void LaserShoot(){

		if (Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			//Debug.Log("Player Collided with missile");
			health -= missile.GetDamage();
			missile.Hit();
			if(health <= 0){
                Die();
			}
		}
	}

    void Die() {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Defeat");
        Destroy(gameObject);
    }

    void WinGame()
    {
        LevelManager lvl = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lvl.LoadLevel("Win Screen");
        Destroy(gameObject);
    }

	void OnCollisionEnter2D(Collision2D collision){
		if(hasStarted){
            AudioSource audio = GetComponent<AudioSource>();
			audio.Play();
			audio.Play(44100);
		}
	}

}
