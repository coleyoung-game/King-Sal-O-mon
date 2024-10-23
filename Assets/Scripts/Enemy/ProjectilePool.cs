using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // Ǯ���� źȯ ������ ������ Dictionary
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    // �̱��� ���� ���� (���� �ϳ��� �����ϵ���)
    public static ProjectilePool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // źȯ ���� �� Ǯ��
    public void CreatePool(GameObject projectilePrefab, int poolSize)
    {
        string poolKey = projectilePrefab.name; // źȯ �̸����� Ǯ ����

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform);
                projectile.SetActive(false); // ��Ȱ��ȭ ���·� ����
                poolDictionary[poolKey].Enqueue(projectile);
            }
        }
    }

    // Ǯ���� źȯ ��������
    public GameObject GetProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation)
    {
        string poolKey = projectilePrefab.name;

        if (poolDictionary.ContainsKey(poolKey))
        {
            // Ǯ�� ��� ������ źȯ�� �ִ� ���
            if (poolDictionary[poolKey].Count > 0)
            {
                GameObject projectile = poolDictionary[poolKey].Dequeue();
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.SetActive(true);
                return projectile;
            }
        }

        // Ǯ�� ���ų� ��� ������ źȯ�� ���� ���, ���� ����
        return Instantiate(projectilePrefab, position, rotation);
    }

    // ����� źȯ Ǯ�� ��ȯ
    public void ReturnProjectile(GameObject projectile)
    {
        string poolKey = projectile.name.Replace("(Clone)", ""); // "(Clone)" ����

        if (poolDictionary.ContainsKey(poolKey))
        {
            projectile.SetActive(false);
            poolDictionary[poolKey].Enqueue(projectile);
        }
        else
        {
            Destroy(projectile); // Ǯ�� ���� źȯ�� �ı�
        }
    }
}