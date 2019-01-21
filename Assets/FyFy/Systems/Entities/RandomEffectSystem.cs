using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class RandomEffectSystem : FSystem {

    private Family singletonManager = FamilyManager.getFamily(new AllOfComponents(typeof(RandomEffectManager)));
    private RandomEffectManager manager;

    private readonly Family _RandomGO = FamilyManager.getFamily(
        new AllOfComponents(typeof(RandomEffect)),
        new NoneOfComponents(typeof(Removed))
    );

    private Family _InTriggerGO = FamilyManager.getFamily(
        new AllOfComponents(typeof(Triggered3D), typeof(InLoopTrigger)),
        new NoneOfComponents(typeof(Removed))
    );

    public RandomEffectSystem()
    {
        manager = singletonManager.First().GetComponent<RandomEffectManager>();

        foreach(EEffect effect in System.Enum.GetValues(typeof(EEffect)))
        {
            manager.activeEffects.Add(new TripleEEffectFloatBool(effect, 0.0f, false));
        }
    }

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

        foreach(TripleEEffectFloatBool activeEffect in manager.activeEffects)
        {
            // If there is time and hasn't been activated, activate it
            if(activeEffect.a > 0.0f && activeEffect.b == false)
            {
                activeEffect.b = true;
            }

            // If there no more time and hasn't been deactivated, deactivate it
            if(activeEffect.a <= 0.0f && activeEffect.b == true)
            {
                activeEffect.b = false;
            }

            activeEffect.a -= Time.deltaTime;
            activeEffect.a = Mathf.Clamp(activeEffect.a, 0.0f, Mathf.Infinity);

            float value = 1.0f;
            foreach(TripleEEffectFloatFloat effectSetting in manager.effectSettings)
            {
                if(effectSetting.key == activeEffect.key)
                {
                    value = effectSetting.b;
                    break;
                }
            }

            if(activeEffect.key == EEffect.SPEED_DOWN)
            {
                if(activeEffect.b)
                    speedDown(value);
                else
                    speedDown(1.0f);
            }
            else if(activeEffect.key == EEffect.SPEED_UP)
            {
                if(activeEffect.b)
                    speedUp(value);
                else
                    speedUp(1.0f);
            }
        }

        foreach (GameObject go in _RandomGO)
        {
            RandomEffect randomEffect = go.GetComponent<RandomEffect>();

            if (go.GetComponent<PointerOver>() != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Object.Instantiate(randomEffect.onClickGO, go.GetComponent<Collider>().bounds.center, go.transform.rotation);
                GameObjectManager.addComponent<Removed>(go);

                foreach(TripleEEffectFloatFloat effectSetting in manager.effectSettings)
                {
                    foreach(TripleEEffectFloatBool activeEffect in manager.activeEffects)
                    {
                        if(activeEffect.key == randomEffect.effect && effectSetting.key == randomEffect.effect)
                        {
                            activeEffect.a = effectSetting.a;
                            break;
                        }
                    }
                }

            }
        }
	}

    private void speedUp(float value)
    {
        foreach (GameObject triggerVolume in _InTriggerGO)
        {
            InLoopTrigger triggerComp = triggerVolume.GetComponent<InLoopTrigger>();
            triggerComp.speedMult = triggerComp.initialSpeedMult * value;
        }
    }

    private void speedDown(float value)
    {
        foreach (GameObject triggerVolume in _InTriggerGO)
        {
            InLoopTrigger triggerComp = triggerVolume.GetComponent<InLoopTrigger>();
            triggerComp.speedMult = triggerComp.initialSpeedMult * value;
        }
    }
}