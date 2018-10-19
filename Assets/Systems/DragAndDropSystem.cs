using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class DragAndDropSystem : FSystem {
    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable), typeof(Card))
    );

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _cards)
        {
            Dragable dragable = go.GetComponent<Dragable>();
            if (dragable.isDragged)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    go.transform.position = Input.mousePosition;
                }
                else
                {
                    dragable.isDragged = false;
                }
            }
            else
            {
                if (go.GetComponent<PointerOver>() != null)
                {
                    GameObject duplicate = Object.Instantiate(go);
                    GameObjectManager.bind(duplicate);

                    duplicate.GetComponent<Dragable>().isDragged = true;
                }
            }
        }
	}
}