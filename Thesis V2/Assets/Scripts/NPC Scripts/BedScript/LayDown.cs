using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LayDown : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public bool occupied = false;
    string occupantName;
    Vector3 previousPosition;

    [SerializeField] private TextAsset virusJson;
    [SerializeField] private GameObject visualCue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!occupied) return;

            visualCue.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        NPCAnimBehavior(other.gameObject);

        EnterDialogue(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player")) return;

        visualCue.SetActive(false);
    }


    private void layDownTrigger(Animator animator, GameObject npc, GameObject bed)
    {
        animator.SetTrigger("LayDown");
        npc.GetComponent<NPCAnimScript>().isLayingDown = true;

        npc.GetComponent<BoxCollider>().enabled = false;
        previousPosition = new Vector3(0, npc.transform.position.y, 0);
        npc.transform.position = new Vector3(bed.transform.position.x, 1, bed.transform.position.z + 2);
        npc.transform.forward = bed.transform.forward;

        occupantName = npc.name;
        occupied = true;
        virusJson = InkManager.instance.getVirusDialogue();
    }

    private void standUpTrigger(Animator animator, GameObject npc)
    {
        animator.SetTrigger("StandUp");
        npc.transform.position += npc.transform.forward * 11;
        npc.transform.position = new Vector3(npc.transform.position.x, previousPosition.y, npc.transform.position.z);

        npc.GetComponent<NPCAnimScript>().isLayingDown = false;
        npc.GetComponent<DialogueAction>().inkJson = InkManager.instance.getRandomInk(npc.GetComponent<DialogueAction>().isMale);

        npc.GetComponent<CapsuleCollider>().enabled = true;

        occupantName = "";
        occupied = false;

        npc.GetComponent<Unit>().target = UnitTargetManager.GetInstance().getAnyGameObjectTarget(npc.GetComponent<Unit>().floor, npc).transform.position;
    }

    private void NPCAnimBehavior(GameObject other)
    {
        if (other.tag != "npc") return;

        if (!other.GetComponent<Unit>().target.Equals(transform.position)) return;

        GameObject bed = gameObject;

        bool npcLayingDown = other.GetComponent<NPCAnimScript>().isLayingDown;
        bool npcIsSick = other.GetComponent<NPCAnimScript>().isSick;

        if (!(npcIsSick ^ (npcLayingDown && occupied))) return;
        if (npcLayingDown != occupied) return;
        if (npcIsSick && !npcLayingDown) layDownTrigger(other.GetComponent<Animator>(), other.gameObject, bed);
        else if (!npcIsSick) standUpTrigger(other.GetComponent<Animator>(), other.gameObject);
    }

    private void EnterDialogue(GameObject other)
    {
        if (!other.tag.Equals("Player")) return;
        visualCue.SetActive(true);
        // if (!visualCue.activeInHierarchy) return;
        AssigningBottleWithMeds.instance.npcPatient = occupantName;
        if (!InputManager.getInstance().GetInteractPressed()) return;
        if (!occupied) return;

        DialogueManagaer.GetInstance().EnterDialogueMode(virusJson);
    }

    public void LoadData(GameData data)
    {
        if (data.virusJsonData.TryGetValue(id, out virusJson))
        {
            virusJson = null;
        }

        string occupantOutPut;
        if (data.occupantLDNameData.TryGetValue(id, out occupantOutPut)){
            if (occupantOutPut == "") return;
            GameObject.Find(occupantOutPut).GetComponent<NPCAnimScript>().isLayingDown = false;
            layDownTrigger(GameObject.Find(occupantOutPut).GetComponent<Animator>(), GameObject.Find(occupantOutPut), this.gameObject);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.virusJsonData.ContainsKey(id))
        {
            data.virusJsonData.Remove(id);
        }
        data.virusJsonData.Add(id, virusJson);

        if (data.occupantLDNameData.ContainsKey(id)){
            data.occupantLDNameData.Remove(id);
        }
        data.occupantLDNameData.Add(id, occupantName);
    }
}
