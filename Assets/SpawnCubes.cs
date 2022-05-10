using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    [SerializeField] private GameObject[] cubes;
    private int cubeNum;
    private int cubesSpawned;
    // Start is called before the first frame update
    void Start()
    {
        cubeNum = (int)Random.Range(100, 150);
        cubesSpawned = cubeNum;
        Debug.Log(cubeNum);
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("FINISH");
        }
    }

    public IEnumerator Spawn()
    {
        if (cubesSpawned > 0)
        {
            cubesSpawned -= 1;

            float x = Random.Range(-4.5f, 4.5f);
            float y = Random.Range(0.5f, 4.5f);
            float z = Random.Range(-4.5f, 4.5f);

            if (x < 2f && x > -2f && y < 2f && y > -2f && z < 2f && z > -2f)
            {
                x = x <= 0f ? -2f : 2f;
                y = y <= 0f ? -2f : 2f;
                z = z <= 0f ? -2f : 2f;
            }

            Vector3 pos = new Vector3(x, y, z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0); //Random.rotation;
            float scale = Random.Range(1.0f, 2.0f);
            GameObject cube = Instantiate(cubes[Random.Range(0, cubes.Length - 1)], gameObject.transform);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale *= scale;
            yield return new WaitForSeconds(6f / cubeNum);

            StartCoroutine("Spawn");
        }
        else
        {
            StopCoroutine("Spawn");
            Debug.Log("ALL CUBES SPAWNED");
        }
       
    }
}
