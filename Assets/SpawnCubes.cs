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
        
    }

    public IEnumerator Spawn()
    {
        if (cubesSpawned > 0)
        {
            cubesSpawned -= 1;

            Vector3 pos = new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(0.5f, 4.5f), Random.Range(-4.5f, 4.5f));
            Quaternion rot = new Quaternion(0, 1, 0, Random.Range(0.0f, 360.0f));
            float scale = Random.Range(1.0f, 3.0f);
            GameObject cube = Instantiate(cubes[Random.Range(0, cubes.Length - 1)], gameObject.transform);//, pos, rot);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale *= scale;
            yield return new WaitForSeconds(6.0f / cubeNum);

            StartCoroutine("Spawn");
        }
        else
        {
            StopAllCoroutines();
        }
       
    }
}
