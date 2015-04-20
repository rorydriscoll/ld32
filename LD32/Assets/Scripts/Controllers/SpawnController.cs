using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct RandomInRange
{
    // Inclusive!

    public int min;
    public int max;

    public int Value
    {
        get { return Random.Range(min, max + 1); }
    }
}

[System.Serializable]
public class Wave
{
    public int[] types;

    public RandomInRange groupsPerWave;
    public RandomInRange enemiesPerGroup;
}

public class SpawnController : MonoBehaviour
{
    public AnimationCurve spawnDelayCurve;
    public AnimationCurve groupDelayCurve;
    public AnimationCurve moveSpeedCurve;
    public Wave[] waves;
    public GameObject spawnObject;
    public int spawnCount = 0;
    public int currentWaveID = 1;
    public bool isActive = false;

    // private
    private GameController gameController;
    private float moveSpeed;
    private float spawnDelay;
    private float groupDelay;

    private Wave CurrentWave
    {
        get { return waves[Mathf.Min(currentWaveID, waves.Length - 1)]; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + transform.rotation * new Vector3(0, 1, 5), new Vector3(transform.localScale.x, 2, 10));
    }

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");
        if (gameControllerObj != null)
            gameController = gameControllerObj.GetComponent<GameController>();
        else
            Debug.Log("Spawn controller cannot find GameController!");
    }

    Identifier PickType()
    {
        int typeID = Mathf.Clamp(CurrentWave.types[Random.Range(0, CurrentWave.types.Length)], 0, Identifier.kNumPermutations - 1);

        return new Identifier(typeID);
    }

    void SpawnHazard(GameObject obj)
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-transform.localScale.x * 0.5f, transform.localScale.x * 0.5f), 0, 0);
        GameObject enemyObject = Instantiate(obj, spawnPos, transform.rotation) as GameObject;

        if (enemyObject.GetComponent<EnemyBehavior>())
            enemyObject.GetComponent<EnemyBehavior>().SetTypeSpeedAndController(PickType(), moveSpeed, this, gameController);
        if (enemyObject.GetComponent<FriendlyBehavior>())
            enemyObject.GetComponent<FriendlyBehavior>().SetTypeSpeedAndController(PickType(), moveSpeed, this, gameController);
        //Debug.Log ("enemyObject = " + enemyObject.transform.position);
        ++spawnCount;
        Debug.Log("Active spawn enemy count=" + spawnCount);
    }
    // Update is called once per frame
    void Update()
    {

    }
    // count to spawn in the wave and the number of enemey types
    private void KickWave(float moveSpeed, float spawnSpeed)
    {

    }
    public void KickWave(int waveID)
    {
        isActive = true;
        currentWaveID = waveID;
        groupDelay = groupDelayCurve.Evaluate(waveID);
        spawnDelay = spawnDelayCurve.Evaluate(waveID);
        moveSpeed = moveSpeedCurve.Evaluate(waveID);

        StartCoroutine(SpawnMain());
    }
    public void SetPlayerDead()
    {
        if (!gameController.IsGameOver())
            gameController.SetGameOver();
    }

    public bool IsPlayerDead()
    {
        return gameController.IsGameOver();
    }

    IEnumerator SpawnMain()
    {
        int groupsPerWave = CurrentWave.groupsPerWave.Value;
        int enemiesPerGroup = CurrentWave.enemiesPerGroup.Value;

        Debug.Log("Wave " + currentWaveID + ": " + groupsPerWave + " groups with " + enemiesPerGroup + " enemies per group");

        for (int i = 0; i < groupsPerWave && !gameController.IsGameOver(); i++)
        {
            // group delay
            if (i != 0)
                yield return new WaitForSeconds(Random.Range(groupDelay * 0.8f, groupDelay * 1.2f));
            // spawn group
            Debug.Log("Wave " + currentWaveID + " Spawn Group idx " + i + " count=" + groupsPerWave);
            for (int j = 0; j < enemiesPerGroup && !gameController.IsGameOver(); j++)
            {
                SpawnHazard(spawnObject);
                yield return new WaitForSeconds(Random.Range(spawnDelay * 0.8f, spawnDelay * 1.2f));
            }
            Debug.Log("Wave " + currentWaveID + " Done Spawn Group idx " + i + " count=" + groupsPerWave);
        }
        Debug.Log("***** Wave " + currentWaveID + " DONE *****");
        isActive = false;
        gameController.SetKickWave();
    }
}
