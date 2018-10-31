using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;

public class DragAndDropSystem : FSystem {
    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable))
    );

	protected override void onProcess(int familiesUpdateCount) {
        foreach (GameObject go in _cards)
        {
            Dragable dragable = go.GetComponent<Dragable>();

            if (dragable.isDragged)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = new Vector2();
                    Vector3 point = new Vector3();

                    // Get the mouse position from Input.
                    // Note that the y position from Input is inverted.
                    mousePos.x = Input.mousePosition.x;
                    mousePos.y = Input.mousePosition.y;

                    point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
                    go.transform.position = point;
                }
                else
                {
                    dragable.isDragged = false;
                    GameObjectManager.removeComponent<Dragable>(go);
                }
            }
        }
	}
}