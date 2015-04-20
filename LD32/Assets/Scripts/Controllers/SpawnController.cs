using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveTypes
{
    public int[] types;
}

public class SpawnController : MonoBehaviour
{
    public AnimationCurve hazardCountCurve;
    public AnimationCurve spawnSpeedCurve;
    public AnimationCurve moveSpeedCurve;
    public WaveTypes[] waveTypes;
    public AnimationCurve groupCountCurve;
    public AnimationCurve groupSpawnSpeed;
    public GameObject spawnObject;
    public int spawnCount = 0;
    public int currentWaveID = 1;
    public bool isActive = false;

    // private
    private int numPerGroup;
    private GameController gameController;
    private float speed;
    private float spawnSpeed_;
    private int groupCount;
    private float groupSpawnWait;

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
        WaveTypes available = waveTypes[Mathf.Min(currentWaveID, waveTypes.Length - 1)];

        int typeID = Mathf.Clamp(available.types[Random.Range(0, available.types.Length)], 0, Identifier.kNumPermutations - 1);

        return new Identifier(typeID);
    }
    void SpawnHazard(GameObject obj)
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-transform.localScale.x * 0.5f, transform.localScale.x * 0.5f), 0, 0);
        GameObject enemyObject = Instantiate(obj, spawnPos, transform.rotation) as GameObject;

        if (enemyObject.GetComponent<EnemyBehavior>())
            enemyObject.GetComponent<EnemyBehavior>().SetTypeSpeedAndController(PickType(), speed, this, gameController);
        if (enemyObject.GetComponent<FriendlyBehavior>())
            enemyObject.GetComponent<FriendlyBehavior>().SetTypeSpeedAndController(PickType(), speed, this, gameController);
        //Debug.Log ("enemyObject = " + enemyObject.transform.position);
        ++spawnCount;
        Debug.Log("Active spawn enemy count=" + spawnCount);
    }
    // Update is called once per frame
    void Update()
    {

    }
    // count to spawn in the wave and the number of enemey types
    public void KickWave(int count, float moveSpeed, float spawnSpeed)
    {
        numPerGroup = count;
        speed = moveSpeed;
        spawnSpeed_ = spawnSpeed;

        StartCoroutine(SpawnMain());
    }
    public void KickWave(int waveID)
    {
        isActive = true;
        currentWaveID = waveID;
        int numHazards = (int)hazardCountCurve.Evaluate(waveID);
        float speed = moveSpeedCurve.Evaluate(waveID);
        float spawnSpeed = spawnSpeedCurve.Evaluate(waveID);
        groupCount = (int)groupCountCurve.Evaluate(waveID);
        groupSpawnWait = groupSpawnSpeed.Evaluate(waveID);
        Debug.Log("Kick Wave #" + waveID + " Enemies= " + numHazards + " speed = "
                   + speed + " spawnSpeed = " + spawnSpeed + " groupCount=" + groupCount + " GroupDelay=" + groupSpawnWait);

        KickWave(numHazards, speed, spawnSpeed);
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
        Debug.Log("***** Spawn started, num groups=" + groupCount + " num per group = " + numPerGroup + " total to spawn this wave = " + groupCount * numPerGroup);
        for (int i = 0; i < groupCount && !gameController.IsGameOver(); i++)
        {
            // group delay
            if (i != 0)
                yield return new WaitForSeconds(groupSpawnWait);
            // spawn group
            Debug.Log("Wave " + currentWaveID + " Spawn Group idx " + i + " count=" + groupCount);
            for (int j = 0; j < numPerGroup && !gameController.IsGameOver(); j++)
            {
                SpawnHazard(spawnObject);
                yield return new WaitForSeconds(spawnSpeed_);
            }
            Debug.Log("Wave " + currentWaveID + " Done Spawn Group idx " + i + " count=" + groupCount);
        }
        Debug.Log("***** Wave " + currentWaveID + " DONE *****");
        isActive = false;
        gameController.SetKickWave();
    }
}
