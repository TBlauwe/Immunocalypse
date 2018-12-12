using UnityEngine;
using FYFY;

/// <summary>
///     Manage Freezed entities.
/// </summary>
public class FreezeSystem : FSystem {

    // All freezed entities
    private readonly Family _freezed = FamilyManager.getFamily(new AllOfComponents(typeof(Freeze)));

    /// <summary>
    /// Loop over all freezed entites and decrease cooldown. If it reaches 0, then remove the component Freeze.
    /// </summary>
    /// <param name="currentFrame"></param>
	protected override void onProcess(int currentFrame) {
        foreach (GameObject goFreezed in _freezed)
        {
            foreach (Freeze freeze in goFreezed.GetComponents<Freeze>())  // Possibly multiple freeze
            {
                freeze.remaining -= Time.deltaTime;

                if (freeze.remaining <= 0)
                {
                    GameObjectManager.removeComponent(freeze);
                }
            }
        }
	}
}