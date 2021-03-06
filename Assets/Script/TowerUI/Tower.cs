﻿using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour, ICannon {
	private BulletManager BulletPool;

	public int level;
	public float searchRadius;
	public CannonPlatform useCannon;
	
	private float nextFire;
	private float startFire;
	private float startRate = 1.0F;

	public float Speed { get { return fort_list[level].Speed;}  }
	public int Damage { get { return fort_list[level].Damage;}  }
	public int Cost { get {	return fort_list[level+1].Cost; } }
	public int Price { get {return fort_list[level].Price;} }
	public int Level { get { return level; } }
	public string towerName { get { return fort_list[level].towerName; } }

	private MonsterAI lockMonster;
	public Fort[] fort_list;
	private Vector3 ShotSpwan;
	private UIManager uiManager;

	public void setUseCannon(CannonPlatform selectCannon){
		useCannon = selectCannon;
	}

	void set_fort_enable(){

		for (int i = 0; i < fort_list.Length; i++){
			Fort fort = fort_list[i];
			if( fort ){
				if (i == level) {
					fort.gameObject.SetActive(true);
				} else {
					fort.gameObject.SetActive(false);
				}
			}
		}
	}

	public bool level_up(){
		if (level + 1 >= fort_list.Length)
			return false;

		level++;
		set_fort_enable ();
		return true;
	}

	void Awake()
	{
		GameObject gameMgr = GameObject.Find ("GameManager");
		BulletPool = gameMgr.GetComponent<BulletManager>();
		uiManager = gameMgr.GetComponent<UIManager> ();
		
		set_fort_enable ();
	}

	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Monster"));
		
		int minHp = 0;
		int LockIdx = -1;
		for (int i=0; i<hitColliders.Length; i++) {
			Collider tmpCollider = hitColliders[i];
			var monster = tmpCollider.gameObject.GetComponent<MonsterAI>();
			if( monster ){
				int checkHp = monster.getHp();
				if( (checkHp < minHp || minHp == 0) && checkHp > 0){
					minHp = monster.getHp();
					if( lockMonster == null || lockMonster.GetInstanceID() != monster.GetInstanceID() )
						startFire = Time.time + startRate;
					
					lockMonster = monster;
					LockIdx = i;
				}
			}
		}

		if (LockIdx >= 0) {
			Collider LockCollider = hitColliders [LockIdx];
			if (Time.time > nextFire && Time.time > startFire) {
				// 根據等級取得目前砲台資料
				Fort fort = fort_list[level];

				nextFire = Time.time + fort.FireRate;
				var Bullet = BulletPool.Obtain("basic");
				Bullet.SetPosition(ShotSpwan);
				Bullet.SetRotation(transform.rotation);
				Bullet.SetEnable();

				Vector3 direction = (LockCollider.gameObject.transform.position - fort.transform.position).normalized;
				Bullet.SetVelocity(direction, fort.Speed);
				Bullet.SetDamage(fort.Damage);
				//LockCollider.gameObject.GetComponent<MonsterAI>().Damage(damage);
			}
		}
	}

	void FixedUpdate() {
		if (!lockMonster)
			return;
		if (lockMonster.getHp () <= 0)
			return;

		if (Vector3.Distance (lockMonster.transform.position, transform.position) > searchRadius)
			return;
		// 根據等級取得目前砲台資料
		Fort fort = fort_list[level];

		Vector3 targetDir = lockMonster.transform.position - fort.transform.position;
		Vector3 newDir = Vector3.RotateTowards( fort.transform.forward, targetDir, 0.05F, 0.0F );				
		fort.transform.rotation = Quaternion.LookRotation(newDir);
		
		ShotSpwan = fort.transform.position + new Vector3(0.0F , 0.5f + level/10 , 0.0F );
	}

	void OnMouseDown(){
		uiManager.setMouseDownTowerPanel (useCannon);
	}

	public void destroy(){
		Destroy (gameObject);
	}
}
