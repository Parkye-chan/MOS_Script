using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class TrapRoomRespawner : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject Effect;

    private TrapRoom trapRoom;
    private bool Dead=true;
    private Character _character;
    private DamageOnTouch damage;
    private AIBrain brain;

    void Start()
    {
        trapRoom = GetComponentInParent<TrapRoom>();
        _character = Enemy.GetComponent<Character>();
        damage = Enemy.GetComponent<DamageOnTouch>();
        brain = Enemy.GetComponent<AIBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Enemy.activeSelf && !Dead)
        {
            Dead = true;
            trapRoom.KillCount--;
            return;
        }
    }

    public void EnemySpawn()
    {
        Dead = false;
        if (Effect)
        {
         var temp = Instantiate(Effect, transform.position, transform.rotation);
            Destroy(temp, 2.0f);
        }

        Enemy.SetActive(true);
        Enemy.transform.position = this.transform.position;
        _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
        damage.enabled = true;
        brain.enabled = true;
        trapRoom.KillCount++;
    }


}
