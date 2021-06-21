using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    float count = 925f;
    int indexValue = 0;

    public CarController carController;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < 2; i++)
        {
            if (carController.finish == false)
            {
                Vector3 spawnLocation = new Vector3(0.0f + count, -1.6f, 0);
                count *= 2;
                Instantiate(objectPrefabs[indexValue], spawnLocation, objectPrefabs[indexValue].transform.rotation);
                indexValue++;
            }
        }
    }
}