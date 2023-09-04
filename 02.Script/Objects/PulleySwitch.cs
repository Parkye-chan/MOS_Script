using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

namespace Tools
{

    public class PulleySwitch : MonoBehaviour
    {

        public Transform endPos;
        protected MovingPlatform Pulley;
        protected Health health;
        protected int curHealth;
        protected bool isActive = false;
        void Start()
        {
            Pulley = transform.GetComponentInChildren<MovingPlatform>();
            Pulley.MoveTowardsEnd();
            Pulley.ToggleMovementAuthorization();
            health = GetComponent<Health>();
            curHealth = health.CurrentHealth;
        }


        private void Update()
        {
            if (curHealth != health.CurrentHealth && !isActive)
            {
                PulleyActive();
            }
            else
                return;
        }

        protected void PulleyActive()
        {
            health.CurrentHealth = health.MaximumHealth;
            curHealth = health.MaximumHealth;
            StartCoroutine(PulleyMove());
        }

        protected void ReturnProcess()
        {
            Pulley.ScriptActivated = false;
            isActive = true;
            Pulley.ResetEndReached();
            StartCoroutine(ReturnPos());
        }

        public void MasterReturnProcess()
        {
            if (!isActive)
            {
                Pulley.ScriptActivated = false;
                isActive = true;
                Pulley.MoveTowardsStart();
            }
        }

        IEnumerator PulleyMove()
        {
            float Dist = (Pulley.transform.localPosition - endPos.localPosition).sqrMagnitude;

            if (Dist < 0.01f)
            {
                ReturnProcess();
            }
            else
            {
                isActive = true;
                Pulley.ToggleMovementAuthorization();

                yield return new WaitForSecondsRealtime(1.0f);

                Pulley.ToggleMovementAuthorization();
                isActive = false;
            }
        }

        IEnumerator ReturnPos()
        {
            yield return new WaitForSecondsRealtime(5.0f);
            
            Pulley.ScriptActivated = true;
            Pulley.MoveTowardsEnd();
            Pulley.ToggleMovementAuthorization();
            isActive = false;
        }

    }
}
