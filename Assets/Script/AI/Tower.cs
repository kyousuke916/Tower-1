﻿using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
	public float searchRadius;
	public float speed;
	public int damage;
	public int cost;
	
	public GameObject shot;
	public float fireRate;
	
	private float nextFire;
	
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Monster"));
		
		int minHp = 0;
		int LockIdx = -1;
		for (int i=0; i<hitColliders.Length; i++) {
			Collider tmpCollider = hitColliders[i];
			int checkHp = tmpCollider.gameObject.GetComponent<MonsterAI>().getHp();
			if( (checkHp < minHp || minHp == 0) && checkHp > 0){
				minHp = tmpCollider.gameObject.GetComponent<MonsterAI>().getHp();
				LockIdx = i;
			}
		}

		Vector3 ShotSpwan = transform.position + new Vector3(0,1,0);
		
		if (LockIdx >= 0) {
			Collider LockCollider = hitColliders [LockIdx];
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				GameObject Bullet = (GameObject)Instantiate (shot, ShotSpwan, transform.rotation);
				Vector3 direction = (LockCollider.gameObject.transform.position - ShotSpwan).normalized;
				Bullet.GetComponent<Rigidbody> ().velocity = direction * speed;
				LockCollider.gameObject.GetComponent<MonsterAI> ().Damage(damage);
			}
		}
	}
}