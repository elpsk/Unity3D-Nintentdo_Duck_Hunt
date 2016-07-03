using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveDuck : MonoBehaviour
{
	public GameObject dogWithDuck;

	private Vector2 position;
	private int inverterX = -1;
	private int inverterY = -1;
	int dogSleepTime = 10;

	void Start ()
	{
		// hide dog
		dogWithDuck.transform.position = new Vector2 (5, -8);

		// stop emitter
		gameObject.GetComponent<ParticleSystem> ().enableEmission = false;

		// start duck animation
		InvokeRepeating ("moveDuck", 0.5f, 0.2f);

		// randomly change ipsilon value
		InvokeRepeating ("changeIpsilon", 1.0f, Random.Range (1.0f, 3.0f));
	}

	// Invert che IPSILON value
	void changeIpsilon ()
	{
		inverterY = inverterY != 0 ? 0 : -1;
	}

	// duck animation
	void moveDuck ()
	{
		position = new Vector2 (1 * inverterX, inverterY);

		int ics = (int)GetComponent<Rigidbody2D> ().transform.position.x;
		int ips = (int)GetComponent<Rigidbody2D> ().transform.position.y;

		// boundaries control
		// LEFT   = -10
		// RIGTH  = 10
		// TOP    = 8
		// BOTTOM = -2
		if (ics <= -10) {
			inverterX = 1;
		} else if (ics >= 10) {
			inverterX = -1;
		}
		if (ips <= -2) {
			inverterY = 1;
		} else if (ips >= 8) {
			inverterY = -1;
		}

		// translate, not position!! To add position to previous position
		GetComponent<Rigidbody2D> ().transform.Translate (position);

		// animator parameter
		GetComponent<Animator> ().SetInteger ("X", (int)position.x);
	}

	// fire!
	void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)) {
			// stop all animations
			CancelInvoke ();

			// start duck-dead animation
			InvokeRepeating ("duckFired", 0.5f, 0.1f);
		}
	}

	// duck dead animation
	void duckFired ()
	{
		// start emitter
		gameObject.GetComponent<ParticleSystem> ().enableEmission = true;

		position = new Vector2 (0, -1);
		int ips = (int)GetComponent<Rigidbody2D> ().transform.position.y;

		GetComponent<Rigidbody2D> ().transform.Translate (position);
		GetComponent<Animator> ().SetInteger ("Y", (int)position.y);

		if (ips <= -3) {
			// hide duck
			gameObject.SetActive (false);
			CancelInvoke ();

			// prepare dog animation
			inverterY = 1;
			position = new Vector2 (GetComponent<Rigidbody2D> ().transform.position.x, -8);
			dogWithDuck.transform.position = position;
			InvokeRepeating ("animateDogWithDuck", 1.0f, 0.1f);
		}
	}

	void animateDogWithDuck ()
	{
		position = new Vector2 (0, 1 * inverterY);

		float ips = dogWithDuck.transform.position.y;
		dogWithDuck.transform.Translate (position);

		// dog max ipsilon translate
		if (ips >= -5) {
			if (dogSleepTime-- <= 0) {
				inverterY = -1;
			} else {
				inverterY = 0;
			}
		}

		// go home and start again
		if (ips <= -9) {
			dogWithDuck.SetActive (false);
			CancelInvoke ();

			// restart level
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	}

}
