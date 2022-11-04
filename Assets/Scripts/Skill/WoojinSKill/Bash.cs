using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
public class Bash : MonoBehaviourPun, SkillMethod
{
    // 캐릭터의 움직임을 빠르게하여 특정위치까지 가게 하고 스킬을 소환하는 타입
    // 1. 소환할 스킬 스크립트 를 입력 (컨트롤f -> One)
    // 2. 스킬이 히트할때의 스크립트를 입력 (컨트롤f -> Two)
    // 3. x초 뒤에 삭제 (Three)
    // 4. 스킬 쿨타임 넘겨주기 (Four)
    // 5. 스킬 범위 설정 (Five)
    // 6. 사운드 설정 (Six)

    // Skill Range Picture
    private GameObject skilla;
    // NavMesh
    private NavMeshAgent navMeshAgent;
    private RectTransform myskillRangerect = null;
    private Vector3 canSkill;
    private Vector3 desiredDir;
    private bool dashAttack = false;
    private bool skillCool = false;
    private bool skillClick = false;
    // Five
    private int skillRange = 25;

    // Start is called before the first frame update
    private void Start()
    {
        // Using components
        UseComponent();
    }

    private void Update()
    {
        // use on click
        if (skillClick == true)
        {
            // Get the mouse's Pos value
            Vector3 mousePos = Input.mousePosition;
            Vector3 target;

            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

          //  skilla.transform.position = target;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 30f);

            canSkill = hit.point;
            canSkill.y = transform.position.y;
        }
        if (dashAttack == true)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;

            navMeshAgent.SetDestination(desiredDir);
        }
    }

    // Organize the components to use
    public void UseComponent()
    {
        skilla = GetComponent<PlayerInfo>().skilla;
        myskillRangerect = GetComponent<PlayerInfo>().myskillRangerect;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void ResetCooltime()
    {
        skillCool = false;
        Debug.Log("스킬쿨끝");
    }
    public void SkillFire()
    {
        // skill cooldown is not loading
        if (skillCool == false)
        {
            // clicked
            if (skillClick == false)
            {
                // Turn on the skill range picture
               // skilla.SetActive(true);
               // myskillRangerect.gameObject.SetActive(true);
              //  myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);

                skillClick = true;
            }

            else
            {
                skillClick = false;
               // myskillRangerect.gameObject.SetActive(false);
              //  skilla.SetActive(false);
            }
        }
    }
    public void SkillClick(Vector3 Pos)
    {
        if (skillClick == true)
        {
            skillClick = false;
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);
            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2) return;

            RaycastHit hit;
            desiredDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Pos);
            int mask = 1 << LayerMask.NameToLayer("Terrain");
            Physics.Raycast(Camera.main.ScreenPointToRay(Pos), out hit, 30f, mask);

            Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 1f);

            if (hit.collider.tag == "Ground" || hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)
            {
                GetComponent<Animator>().SetTrigger("isSkill1");
                transform.LookAt(desiredDir);
                GetComponent<PlayerInfo>().Stay(0.5f);
                StartCoroutine(Attack(desiredDir, 0.2f));
                // Turn on the cooldown so that it cannot be used again
                skillCool = true;
                Debug.Log("스킬사용");
                // UIMgr SkillCool send  //Four
                GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 15);
            }
        }
    }
    IEnumerator Fire(GameObject skill)
    {
        // skill range
        for (int i = 0; i < 20; i++)
        {
            skill.transform.Translate(0, 0, -0.5f);
            yield return null;
        }
        yield break;
    }
    IEnumerator Attack(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        // speed of character movement
        navMeshAgent.speed = 40f;
        dashAttack = true;
        // One
        GameObject b = PhotonNetwork.Instantiate("Bash", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        // Two
        b.AddComponent<BashHit>();//이펙트에 히트 스크립트를 넣습니다.
       
       
        b.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
        b.transform.LookAt(desiredDir);
        b.transform.Rotate(0, -180, 0);

        StartCoroutine(Fire(b));
        // Six
        //  sound = a.GetComponent<AudioSource>();
        //  sound.Play();

        //Three
        GameMgr.Instance.DestroyTarget(b, 1f);
        yield return new WaitForSeconds(0.5f);
        dashAttack = false;
        navMeshAgent.speed = 5f;

        yield break;
    }
}
