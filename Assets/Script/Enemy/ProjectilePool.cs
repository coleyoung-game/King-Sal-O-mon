using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // 풀링할 탄환 종류를 저장할 Dictionary
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    // 싱글톤 패턴 적용 (씬에 하나만 존재하도록)
    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 탄환 생성 및 풀링
    public void CreatePool(GameObject projectilePrefab, int poolSize)
    {
        string poolKey = projectilePrefab.name; // 탄환 이름으로 풀 구분

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform);
                projectile.SetActive(false); // 비활성화 상태로 생성
                poolDictionary[poolKey].Enqueue(projectile);
            }
        }
    }

    // 풀에서 탄환 가져오기
    public GameObject GetProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation)
    {
        string poolKey = projectilePrefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            // 풀에 사용 가능한 탄환이 있는 경우
            if (poolDictionary[poolKey].Count > 0)
            {
                GameObject projectile = poolDictionary[poolKey].Dequeue();
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.SetActive(true);
                return projectile;
            }
        }

        // 풀이 없거나 사용 가능한 탄환이 없는 경우, 새로 생성
        return Instantiate(projectilePrefab, position, rotation);
    }

    // 사용한 탄환 풀에 반환
    public void ReturnProjectile(GameObject projectile)
    {
        string poolKey = projectile.name.Replace("(Clone)", ""); // "(Clone)" 제거

        if (poolDictionary.ContainsKey(poolKey))
        {
            projectile.SetActive(false);
            poolDictionary[poolKey].Enqueue(projectile);
        }
        else
        {
            Destroy(projectile); // 풀에 없는 탄환은 파괴
        }
    }
}