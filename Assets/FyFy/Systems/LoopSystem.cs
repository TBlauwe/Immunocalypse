using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

public class LoopSystem : FSystem {
    private Family _StartTriggerGO = FamilyManager.getFamily(new AllOfComponents(typeof(StartLoopTrigger)));
    private Family _InTriggerGO = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered3D), typeof(InLoopTrigger)));
    private Family _EndTriggerGO = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered3D), typeof(EndLoopTrigger)));

    public LoopSystem(){
        foreach (GameObject triggerVolume in _StartTriggerGO)
        {
            StartLoopTrigger triggerComp = triggerVolume.GetComponent<StartLoopTrigger>();
            triggerComp.deckPool.Shuffle<GameObject>();
        }
    }

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject triggerVolume in _StartTriggerGO)
        {
            StartLoopTrigger triggerComp = triggerVolume.GetComponent<StartLoopTrigger>();
            if(triggerComp.deckPool.Count > 0)
            {
                triggerComp.cooldownDeck -= Time.deltaTime;
                if(triggerComp.cooldownDeck <= 0)
                {
                    triggerComp.cooldownDeck = triggerComp.deckRate;
                    GameObject prefab = triggerComp.deckPool.Pop<GameObject>();

                    // Instanciate the GameObject
                    GameObject go = Object.Instantiate(prefab);
                    go.SetActive(true);

                    // Bind it to FYFY
                    GameObjectManager.bind(go);

                    // Set GameObject's position
                    go.transform.position = randomStartPosition(triggerVolume);
                }
            }
        }

        foreach (GameObject triggerVolume in _InTriggerGO)
        {
            InLoopTrigger triggerComp = triggerVolume.GetComponent<InLoopTrigger>();
            Triggered3D triggered = triggerVolume.GetComponent<Triggered3D>();
            foreach (GameObject go in triggered.Targets) {
                MoveToward moveToward = go.GetComponent<MoveToward>();
                if(moveToward && moveToward.target != triggerComp.target.position && go.layer != 11) // Immuno layer
                {
                    moveToward.target = triggerComp.target.position;
                }
            }
        }

        foreach (GameObject triggerVolume in _EndTriggerGO)
        {
            EndLoopTrigger triggerComp = triggerVolume.GetComponent<EndLoopTrigger>();
            Triggered3D triggered = triggerVolume.GetComponent<Triggered3D>();
            foreach (GameObject go in triggered.Targets) {
                go.transform.position = randomStartPosition(triggerComp.teleport);
            }
        }
    }

    public Vector3 randomStartPosition(GameObject start)
    {
        float radius = start.GetComponent<SphereCollider>().radius;
        float x = start.transform.position.x + Random.Range(-radius, radius);
        float y = start.transform.position.y;
        float z = start.transform.position.z;
        return new Vector3(x, y, z);
    }
}