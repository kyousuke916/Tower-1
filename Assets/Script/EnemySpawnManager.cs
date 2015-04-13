﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform m_MonsterTs = null;
    
    [SerializeField]
    private Transform m_SpawnTs = null;

    [SerializeField]
    private Transform m_TargetTs = null;

    [SerializeField]
    private Vector2 m_SpawnOffset = Vector2.zero;

    [SerializeField, Tooltip("每波間隔時間")]
    private float m_SpawnInterval = 2f;

    [SerializeField, Tooltip("每波怪物出生最小數量")]
    private int m_SpawnNumMin = 1;

    [SerializeField, Tooltip("每波怪物出生最大數量")]
    private int m_SpawnNumMax = 3;

    private Coroutine mSpwanCoroutine = null;

    private Queue<MonsterAI> mAliveMonsterQueue = new Queue<MonsterAI>();

    void Awake()
    {
        
    }

    private void StartSpawnEnemy()
    {
        mSpwanCoroutine = StartCoroutine(CoStartSpawnEnemy());
    }

    private IEnumerator CoStartSpawnEnemy()
    {
        while (true)
        {
            CreateMultiEnemy();

            yield return new WaitForSeconds(m_SpawnInterval);
        }
    }

    private void CreateMultiEnemy()
    {
        int num = Random.Range(m_SpawnNumMin, m_SpawnNumMax);
        for (int i = 0; i < num; i++)
            CreateEnemy();
    }

    private void StopSpawnEnemy()
    {
        if (mSpwanCoroutine != null)
        {
            StopCoroutine(mSpwanCoroutine);
            mSpwanCoroutine = null;
        }
    }

    private void CreateEnemy()
    {
        var prefab = Resources.Load<GameObject>("Monster/tufu");
        var enemyGo = Instantiate(prefab) as GameObject;
        var enemyTs = enemyGo.transform;
        enemyTs.SetParent(m_MonsterTs);
        enemyTs.position = GetSpawnPosition();
        enemyTs.rotation = m_SpawnTs.rotation;

        var monsterAI = enemyTs.GetComponent<MonsterAI>();
        monsterAI.SetTarget(m_TargetTs);

        mAliveMonsterQueue.Enqueue(monsterAI);
    }
    
    private Vector3 GetSpawnPosition()
    {
        var origin = m_SpawnTs.position;
        var pos = m_SpawnTs.rotation * new Vector3(Random.Range(-m_SpawnOffset.x, m_SpawnOffset.x), 0f, Random.Range(-m_SpawnOffset.y, m_SpawnOffset.y));

        return origin + pos;
    }

    private void MonsterAllDie()
    {
        while (mAliveMonsterQueue.Count > 0)
        {
            MonsterAI monster = mAliveMonsterQueue.Dequeue();
            monster.Damage(999999);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("CreateEnemy")) CreateEnemy();
        if (GUILayout.Button("StartSpawnEnemy")) StartSpawnEnemy();
        if (GUILayout.Button("MonsterAllDie")) MonsterAllDie();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) CreateEnemy();
    }
}
