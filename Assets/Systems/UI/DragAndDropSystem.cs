using UnityEngine;
using FYFY;
using FYFY_plugins.PointerManager;
using FYFY_plugins.TriggerManager;

public class DragAndDropSystem : FSystem {
    private readonly Family _entities = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable)), new NoneOfComponents(typeof(Card))
    );

    private readonly Family _cards = FamilyManager.getFamily(
        new AllOfComponents(typeof(Dragable), typeof(Card))
    );

    private readonly Family _holders = FamilyManager.getFamily(
        new AllOfComponents(typeof(CardHolder))
    );

    private readonly Family _player = FamilyManager.getFamily(new AllOfComponents(typeof(Player)));

    protected override void onProcess(int familiesUpdateCount) {

        foreach (GameObject go in _entities)
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
                    point.y = 0;
                    go.transform.position = point;
                }
                else
                {
                    dragable.isDragged = false;
                    GameObjectManager.removeComponent<Dragable>(go);
                }
            }
        }

        foreach (GameObject go in _cards)
        {
            Dragable dragable = go.GetComponent<Dragable>();

            if (dragable.isDragged)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = new Vector2
                    {
                        // Get the mouse position from Input.
                        // Note that the y position from Input is inverted.
                        x = Input.mousePosition.x,
                        y = Input.mousePosition.y
                    };

                    go.transform.position = mousePos;
                }
                else
                {
                    // No more drag
                    dragable.isDragged = false;

                    // Setup variables
                    GameObject selectedHolder = null;
                    float minDistance = float.MaxValue;

                    // Look for the closest holder
                    foreach (GameObject holder in _holders)
                    {
                        float currentDistance = (go.transform.position - holder.transform.position).magnitude;
                        if (currentDistance < minDistance)
                        {
                            selectedHolder = holder;
                            minDistance = currentDistance;
                        }
                    }

                    // Set parent
                    go.transform.SetParent(selectedHolder.transform);
                    go.transform.SetAsFirstSibling();

                    // Change the card's deck
                    Player player = _player.First().GetComponent<Player>();
                    if (player.globalDeck.Contains(go))
                    {
                        player.globalDeck.Remove(go);
                        player.levelDeck.Add(go);
                    }
                    else
                    {
                        player.levelDeck.Remove(go);
                        player.globalDeck.Add(go);
                    }
                }
            }
        }
    }
}