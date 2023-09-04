using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class ForcePush : MonoBehaviour
{

    public Transform Point;
    public float WalkSpeed = 8;
    public GameObject EntrancePos;
    public GameObject ExitPos;
    public float ExitTime;
    public float StopDist = 0.1f;

    protected bool moveOn = false;
    protected Transform player;
    protected Character character;
    protected CharacterHorizontalMovement horizontalMovement;
    protected MMStateMachine<CharacterStates.CharacterConditions> _condition;
    protected MMStateMachine<CharacterStates.MovementStates> _movement;
    protected CharacterDash dash;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (moveOn && Point)
        {

            player.GetComponentInChildren<Animator>().SetBool("TestWalk", true);
            float Dist = (player.position - Point.transform.position).sqrMagnitude;

            Vector3 dir = (Point.transform.position - player.position).normalized;
            player.position += dir * WalkSpeed * Time.deltaTime;

            Debug.Log(Dist);
            if (Dist < StopDist)
            {
                moveOn = false;
                player.GetComponentInChildren<Animator>().SetBool("Dashing", false);
                player.GetComponentInChildren<Animator>().SetBool("TestWalk", false);
                StartCoroutine(ExitClose());
            }

        }
    }

    private void OnDisable()
    {
        if (character)
        {
            if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)
                _condition.ChangeState(CharacterStates.CharacterConditions.Normal);

            moveOn = false;
            player.GetComponentInChildren<Animator>().SetBool("Dashing", false);
            player.GetComponentInChildren<Animator>().SetBool("TestWalk", false);
            dash.enabled = true;
        }
    }

    private void MoveFunc(Collider2D Target)
    {
        character = Target.GetComponent<Character>();
        dash = Target.GetComponent<CharacterDash>();
        dash.enabled = false;
        horizontalMovement = Target.GetComponent<CharacterHorizontalMovement>();
        horizontalMovement.ReadInput = false;
        _condition = character.ConditionState;
        _movement = character.MovementState;
        _condition.ChangeState(CharacterStates.CharacterConditions.Frozen);

        player = Target.transform;
        moveOn = true;
    }

    IEnumerator ExitClose()
    {
        yield return new WaitForSecondsRealtime(ExitTime);
        if (ExitPos)
        {
            horizontalMovement.ReadInput = true;
            _condition.ChangeState(CharacterStates.CharacterConditions.Normal);
            ExitPos.SetActive(false);
            EntrancePos.SetActive(true);
            dash.enabled = true;
            this.gameObject.SetActive(false);
        }
        else
            yield return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ExitPos)
                ExitPos.SetActive(true);

            MoveFunc(collision);
        }
    }
}
