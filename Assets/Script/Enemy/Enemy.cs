using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Canvas gameCanvas;
    public Transform[] firePoints; // ����ü �߻� ��ġ �迭

    public GameObject ProjectilePrefab;  // �Ϲ� źȯ ������
    public int poolSize = 10;                  // Ǯ ũ��

    public float fireRate = 1f; // �߻� �ֱ� (��)
    public Transform playerTransform;

    private void Start()
    {
        if (ProjectilePrefab == null)
            Debug.Log(gameObject.name + "Ȯ���ϼ��� null��");

        // Ǯ ����
        ProjectilePool.Instance.CreatePool(ProjectilePrefab, poolSize);
        StartCoroutine(ShootProjectile());
    }

    // ����ü �߻� �ڷ�ƾ
    IEnumerator ShootProjectile()
    {
        while (true) // ����ؼ� �߻�
        {
            if (firePoints == null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile(ProjectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
                yield return new WaitForSeconds(fireRate); // fireRate��ŭ ���
            }
            else
            { 
                foreach (Transform firePoint in firePoints) // �� �߻� ��ġ���� ����ü ����
                {
                    GameObject projectile = ProjectilePool.Instance.GetProjectile(ProjectilePrefab, firePoint.position, firePoint.rotation);
                }
                yield return new WaitForSeconds(fireRate); // fireRate��ŭ ���
            }
        }
    }
}