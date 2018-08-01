using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public GameObject Player2;
    public Transform player2Transform;
    public GameObject player;
    public Transform target;

    public override void OnStartServer()
    {
        target = transform;
       /* for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(
                Random.Range(-8.0f, 8.0f),
                0.0f,
                Random.Range(-8.0f, 8.0f));

            var spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
           // var player2 = (GameObject)Instantiate(Player2, player2Transform.position, player2Transform.rotation);
            NetworkServer.Spawn(enemy);
            //NetworkServer.SpawnWithClientAuthority(player2,player);
        }*/


        var spawnPosition = new Vector3(
               42,
               0.0f,
               0);

        var spawnRotation = Quaternion.Euler(
            0.0f,
            20.0f,
            0.0f);

        var enemy = (GameObject)Instantiate(enemyPrefab, target.position, target.rotation);

        NetworkServer.Spawn(enemy);
    }
}
