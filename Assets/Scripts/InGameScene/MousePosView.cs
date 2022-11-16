using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosView : MonoBehaviour
{

    [SerializeField] GameObject MouseClickPosOjb;
    [SerializeField] private int count;
    List<GameObject> clickPool = new List<GameObject>();
    public GameObject click;
    public GameObject newClick;
    public GameObject reCycle;
    // Start is called before the first frame update
    void Start()
    {
        MouseClickPosOjb = new GameObject("MouseClickPosOjb");
    }
  
    // Update is called once per frame
    void Update()
    {
        if (GameMgr.Instance.GameState == false) return;
        count = clickPool.Count;
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f))
            {
                for (int i = 0; i < clickPool.Count; i++)
                {
                    if (clickPool[i].activeSelf == false)
                    {
                        reCycle = clickPool[i];
                        reCycle.transform.position = hit.point + new Vector3(0f, 0.1f, 0f);
                        reCycle.transform.rotation = Quaternion.LookRotation(Vector3.up);
                        break;
                    }
                }
                if (reCycle != null)
                {
                    reCycle.SetActive(true);
                    StartCoroutine(ClickPosSet(reCycle));
                    reCycle = null;
                }
                else
                {
                    newClick = Instantiate(click, hit.point + new Vector3(0f, 0.1f, 0f), Quaternion.LookRotation(Vector3.up));
                    newClick.transform.SetParent(MouseClickPosOjb.transform);
                    clickPool.Add(newClick);
                    StartCoroutine(ClickPosSet(newClick));
                }
            }
        }
    }

    IEnumerator ClickPosSet(GameObject eff)
    {
        yield return new WaitForSeconds(0.8f);
        eff.SetActive(false);
    }
}
