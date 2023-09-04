using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class Gate : MonoBehaviour
{
    protected Transform Player;
    protected Animator animator;
    protected bool isOpen = false;
    protected Animator potalAnim;
    protected Teleporter teleporter;
    private SpriteRenderer sprite;
    [SerializeField]
    public string GateName;
    public bool DestinaionOn = false;
    public bool GateMeet = false;
    public GameObject potal;
    public float Radius;
    public GateManager gateManager;
    public int StageNum;
    public SpriteRenderer[] Pattens;

    private void Awake()
    {
        if (GateName == "")
            GateName = gameObject.name;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(potal)
        potalAnim = potal.GetComponent<Animator>();
        teleporter = GetComponent<Teleporter>();
        sprite = GetComponent<SpriteRenderer>();
        if(sprite)
        {
            for (int i = 0; i < Pattens.Length; i++)
            {
                Pattens[i].flipX = sprite.flipX;
            }
        }
    }

    void Update()
    {

        PlayerCheck();
        if (isOpen)
            OpenGateManager();
    }


    protected void PlayerCheck()
    {
        if (!isOpen && Radius >= Vector3.Distance(transform.position, GameManager.Instance.PersistentCharacter.transform.position))
        {
            isOpen = true;
            animator.SetBool("Open", true);
            potal.SetActive(true);
            potalAnim.SetBool("Open", true);
           
        }
        else if (isOpen && Radius < Vector3.Distance(transform.position, GameManager.Instance.PersistentCharacter.transform.position))
        {
            isOpen = false;
            animator.SetBool("Open", false);
            potalAnim.SetBool("Open", false);
            StartCoroutine(potaldisable());
        }
    }

    IEnumerator potaldisable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        potal.SetActive(false);
    }

    protected void OpenGateManager()
    {
        if (gateManager)
        {

            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5)) && !DestinaionOn)
            {
                gateManager.EnablePanel(this.gameObject.GetComponent<Teleporter>());
            }
            /*
            else if (Input.GetKeyDown(KeyCode.UpArrow) && DestinaionOn)
            {
                DestinaionOn = false;
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GateMeet)
                return;
            else
            {
                GateMeet = true;
                if (gateManager)
                {
                    gateManager.GateUpdate();
                    PlayerInfoManager.instance.GateStateSave();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
