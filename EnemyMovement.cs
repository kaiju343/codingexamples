using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public float speed;

	private EnemyMovementRail myRail;

	private int currentSeg;
	private float transition;
	private bool isCompleted;

	public bool Catmull = false;

	void OnEnable()
	{
		myRail = gameObject.GetComponentInChildren<EnemyMovementRail> ();
		currentSeg = 0;
		transition = 0;
		isCompleted = false;
	}

	public void CompletedRail()
	{
		isCompleted = true;
	}
	
	void FixedUpdate () {
		
		if (isCompleted)
			gameObject.SetActive (false);
		
		if (!myRail)
			return;
		if (!isCompleted)
			Move ();
	}

	float yTilt;
	float xDelta;
	float smoothVelo;
	private void Move()
	{
		transition += Time.deltaTime * 1 / speed;

		if (transition > 1) {
			transition = 0;
			currentSeg++;
		} else if (transition<0) {
			transition = 1;
			currentSeg--;
		}


		if (!Catmull)
			transform.position = myRail.LinearPosition (currentSeg, transition, gameObject.GetComponent<EnemyMovement> ());
		else
			transform.position = myRail.CatmullPosition (currentSeg, transition,gameObject.GetComponent<EnemyMovement>());

		// check shoot


		if (xDelta > transform.position.x)
			yTilt = 12.5f;
		else if (xDelta < transform.position.x)
			yTilt = -12.5f;
		else
			yTilt = 0;


		yTilt = Mathf.SmoothDampAngle(gameObject.transform.rotation.eulerAngles.y, yTilt, ref smoothVelo,0.3f);

		transform.rotation = Quaternion.Euler (0,yTilt, 0);
		
		xDelta = transform.position.x;
	}
}

