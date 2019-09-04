using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementRail : MonoBehaviour {
	public Transform[] nodes;
	private Vector3[] nodesPos;
	public bool mirrored = false;

	// Use this for initialization
	void Start () {
		
		nodes = gameObject.GetComponentsInChildren<Transform>();

		nodesPos = new Vector3[nodes.Length];
		for (int i = 0; i < nodes.Length; i++) {
			nodesPos [i] = nodes [i].position;
		}

		if (mirrored)
			MirrorTracks ();
	}

	void OnEnable()
	{
		if (mirrored)
			MirrorTracks ();
	}

	public void MirrorTracks()
	{
		for (int i = 0; i < nodesPos.Length; i++) {
			nodesPos [i] = new Vector3 (nodesPos [i].x * -1, nodesPos [i].y, nodesPos [i].z);
		}
	}
	
	public Vector3 LinearPosition(int seg,float ratio, EnemyMovement enemy){
		
		if (seg == nodesPos.Length - 1) {
			enemy.CompletedRail ();
			return new Vector3(0,-25,0);
		}

		if (nodes [seg].tag == "EnemyAttack")
			enemy.gameObject.GetComponent<Enemy> ().EnemyShoot (seg);
	
		Vector3 p1 = nodesPos [seg];
		Vector3 p2 = nodesPos [seg + 1];
		return Vector3.Lerp (p1, p2, ratio);
	}
		
	public Vector3 CatmullPosition(int seg,float ratio, EnemyMovement enemy){
		if (seg == nodesPos.Length - 1) {
			enemy.CompletedRail ();
			return new Vector3(0,-25,0);
		}

		if (nodes [seg].tag == "EnemyAttack")
			enemy.gameObject.GetComponent<Enemy> ().EnemyShoot (seg);

		Vector3 p1, p2, p3, p4;
		if (seg == 0) {
			p1 = nodesPos [seg];
			p2 = p1;
			p3 = nodesPos [seg + 1];
			p4 = nodesPos [seg + 2];
		} else if (seg == nodes.Length - 2) {
			p1 = nodesPos [seg - 1];
			p2 = nodesPos [seg];
			p3 = nodesPos [seg + 1];
			p4 = p3;
		} else {
			p1 = nodesPos [seg - 1];
			p2 = nodesPos [seg];
			p3 = nodesPos [seg + 1];
			p4 = nodesPos [seg + 2];
		}
		float t2 = ratio * ratio;
		float t3 = t2 * ratio;
		float x = 0.5f * ((2.0f * p2.x) 
			+ (-p1.x + p3.x) 
			* ratio + (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) 
			* t2 + (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) 
			* t3);
		float y = 0.5f * ((2.0f * p2.y) 
			+ (-p1.y + p3.y) 
			* ratio + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y) 
			* t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y) 
			* t3);
		return new Vector3 (x, y, 0);
	}
}
