using UnityEngine;
using FYFY;

public class TCellSystem : FSystem {
    private readonly Family _tCells = FamilyManager.getFamily(
        new AllOfComponents(typeof(TCell))
    );

    // Use to process your families.
    protected override void onProcess(int familiesUpdateCount) {
        // Nothing yet
    }
}