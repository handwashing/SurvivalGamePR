using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //현재 소유하고 있는 총

    private float currentFireRate; 

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire(); //발사 시도
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //currentFireRate가 0보다 클 경우, 1초에 1씩 감소 
                                               // 0이 되면 발사할 수 있는 상태가 됨
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        //발사 후 연사속도 재계산이 이루어지도록 currentFireRate에 새로 대입
        Shoot(); //발사
    }

    private void Shoot()
    {
        Debug.Log("총알 발사함");
    }
}
