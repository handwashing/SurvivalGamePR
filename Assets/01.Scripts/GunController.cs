using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun; //현재 소유하고 있는 총

    private float currentFireRate; 

    private bool isReload = false; 
    private bool isFineSightMode = false;

    //본래 포지션 값
    [SerializeField]
    private Vector3 originPos;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire(); //발사 시도
        TryReload(); //재장전 시도
        TryFineSight(); //정조준 시도
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //currentFireRate가 0보다 클 경우, 1초에 1씩 감소 
                                               // 0이 되면 발사할 수 있는 상태가 됨
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if ( !isReload)
        {
            if(currentGun.currentBulletCount > 0)
            Shoot(); //발사(발사 전)
        else
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void Shoot() //(발사 후)
    {
        currentGun.currentBulletCount--; //총알 개수 -1
        currentFireRate = currentGun.fireRate; //발사 후 연사속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }

    private void TryReload()
    {//R버튼을 누르면 재장전
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }

    }
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;

            currentGun.anim.SetTrigger("Reload"); //Reload anim 실행


            currentGun.carryBulletCount += currentGun.currentBulletCount; //현재 소유한 탄알에 더해주기 
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }


            isReload = false;
        }

        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }


    }

    private void TryFineSight()
        {
            if(Input.GetButtonDown("Fire2"))
            {
                FineSight();
            }
        }

    private void FineSight()
    {
        isFineSightMode = !isFineSightMode; //위의 FineSigh가 실행될 때마다 알아서 true,false로 바뀌게 함...
        
    }

    private void PlaySE(AudioClip _clip) //총 사운드 재생
        {
            audioSource.clip = _clip;
            audioSource.Play();
        }
}
