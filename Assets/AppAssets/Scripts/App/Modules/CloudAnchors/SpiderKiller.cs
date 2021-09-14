using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudAnchors
{
    public class SpiderKiller : MonoBehaviour
    {
        [SerializeField] private ParticleSystem spiderDeathParticleSystem;
        [SerializeField] private float additionalDestroyDelay;
        private string deathAnimName;
        private float deathAnimLenght;

        public bool OnDestroy { get; private set; }

        private void DestroyMe()
        {
            Debug.Log($"destroy object: {gameObject.name}");
            Destroy(transform.parent);
        }

        public void OnKillObject()
        {
            if (!OnDestroy)
            {
                OnDestroy = true;

                spiderDeathParticleSystem.Play();

                Invoke("DestroyMe", deathAnimLenght + additionalDestroyDelay);
            }
        }
    }
}