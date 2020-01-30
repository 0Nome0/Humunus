using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Toolkit;
using NerScript;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab = null;
    [SerializeField] private bool autoDestroy = true;

    private Pool<ParticleSystem> pool = null;

    private void Start()
    {
        ParticleSystem Crete(int i)
        {
            return Instantiate(particlePrefab).GetComponent<ParticleSystem>();
        }
        void Initialize(ParticleSystem particle)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            if(autoDestroy)
            {
                AutoDestroy(particle);
            }
        }
        void Finalize(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }

        pool = new Pool<ParticleSystem>(Crete, Initialize, Finalize);
    }

    public void Effect(Vector3 position)
    {
        ParticleSystem particle = pool.Get();
        particle.transform.position = position;
    }

    private void AutoDestroy(ParticleSystem particle)
    {
        Observable
           .Timer(TimeSpan.FromSeconds(particle.main.duration))
           .TakeUntilDestroy(gameObject)
           .Subscribe(_ => { pool.Used(particle); });
    }


}
