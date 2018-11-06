using UnityEngine;
using FYFY;
using UnityEngine.UI;
using FYFY_plugins.PointerManager;

public class GameInterfaceDisplayer : FSystem {
    private readonly Family _hidables = FamilyManager.getFamily(
        new AllOfComponents(typeof(Hidable), typeof(RectTransform)),
        new AnyOfTags("cardPanel")
    );

	protected override void onProcess(int familiesUpdateCount) {

        foreach (GameObject go in _hidables)
        {
            Hidable hidable = go.GetComponent<Hidable>();
            Button toggler = hidable.toggleButton;

            // Pointer over the toggle button
            if (toggler.GetComponent<PointerOver>() != null)
            {
                // Switch state if button has been pressed
                if (Input.GetMouseButton(0))
                {
                    RectTransform transform = go.GetComponent<RectTransform>();
                    RectTransform buttonTransform = toggler.GetComponent<RectTransform>();

                    // Show menu
                    if (hidable.hidden)
                    {
                        hidable.hidden = false;
                        transform.localPosition = new Vector3(
                            transform.localPosition.x,
                            transform.localPosition.y + transform.sizeDelta.y - buttonTransform.sizeDelta.y
                        );
                    } else // Hide it (except for the button)
                    {
                        hidable.hidden = true;
                        transform.localPosition = new Vector3(
                            transform.localPosition.x,
                            transform.localPosition.y - transform.sizeDelta.y + buttonTransform.sizeDelta.y
                        );
                    }
                }
            }
        }
	}
}