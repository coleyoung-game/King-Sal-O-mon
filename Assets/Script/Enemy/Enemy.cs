using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Canvas gameCanvas;
    public Transform[] firePoints; // 투사체 발사 위치 배열

    public GameObject ProjectilePrefab;  // 일반 탄환 프리팹
    public int poolSize = 10;                  // 풀 크기

    public float fireRate = 1f; // 발사 주기 (초)
    public Transform playerTransform;

    private void Start()
    {
        if (ProjectilePrefab == null)
            Debug.Log(gameObject.name + "확인하세요 null임");

        // 풀 생성
        ProjectilePool.Instance.CreatePool(ProjectilePrefab, poolSize);
        StartCoroutine(ShootProjectile());
    }

    // 투사체 발사 코루틴
    IEnumerator ShootProjectile()
    {
        while (true) // 계속해서 발사
        {
            if (firePoints == null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile(ProjectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
                yield return new WaitForSeconds(fireRate); // fireRate만큼 대기
            }
            else
            { 
                foreach (Transform firePoint in firePoints) // 각 발사 위치마다 투사체 생성
                {
                    GameObject projectile = ProjectilePool.Instance.GetProjectile(ProjectilePrefab, firePoint.position, firePoint.rotation);
                }
                yield return new WaitForSeconds(fireRate); // fireRate만큼 대기
            }
        }
    }
}