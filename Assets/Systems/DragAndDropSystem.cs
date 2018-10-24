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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 rayPoint = ray.GetPoint(dragable.distance);
                go.transform.position = rayPoint;

                if (Input.GetMouseButtonUp(0))
                {
                    dragable.isDragged = false;
                }
            }
            else
            {
                if (go.GetComponent<PointerOver>() != null && Input.GetMouseButton(0))
                {
                    dragable.isDragged = true;
                    dragable.distance = Vector3.Distance(go.transform.position, Camera.main.transform.position);
                }
            }
        }
	}
}