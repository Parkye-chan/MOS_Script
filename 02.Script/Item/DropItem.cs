using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class DropItem : MonoBehaviour
{
    public GameObject item;
    public int RepeatCount = 1;
    public float minForce=1;
    public float maxForce=2;

    private GameObject DropObj;

    public GameObject DropFunc()
    {

        //var temp = Instantiate(item, transform.position, transform.rotation);

        Vector3 dir =
        (this.transform.position- MoreMountains.CorgiEngine.GameManager.Instance.PersistentCharacter.transform.position).normalized;

        for (int i = 0; i < RepeatCount; i++)
        {
            DropObj = Instantiate(item, transform.position, transform.rotation);
            float randomVal = Random.Range(minForce, maxForce);
            float randomHigh = Random.Range(minForce, maxForce);
            DropObj.GetComponent<Rigidbody2D>().AddForce(dir * randomVal * randomHigh, ForceMode2D.Impulse);
        }

        return DropObj;
    }

}
