using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    private MonsterFOV FOV;
    private bool isActive = false;
    public GameObject DamageZone;
    public GameObject Flame;
    void Start()
    {
        FOV = GetComponent<MonsterFOV>();    
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (FOV.GroundCheck() && !isActive)
        {
            isActive = true;
            StartCoroutine(Activation());
        }
        else
            return;
    }

    IEnumerator Activation()
    {
        DamageZone.SetActive(true);
        Flame.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        DamageZone.SetActive(false);
    }


}
