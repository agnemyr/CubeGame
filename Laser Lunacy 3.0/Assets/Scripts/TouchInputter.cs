
using UnityEngine;

public class TouchInputter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Touch supported: " + Input.touchSupported);
        //Debug.Log("Multiouch enabled: " + Input.multiTouchEnabled);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Touch count: " + Input.touchCount);
        //Debug.Log("Mouse down: " + Input.GetMouseButtonDown(0));
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            /*var worldHitPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
            Debug.Log("Hit position: " + worldHitPos);
            var colliders = Physics.RaycastAll(worldHitPos, Vector3.down);*/
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
            /*foreach (var collider in colliders)
            {
                Debug.Log(collider.transform.gameObject.name + " hit!");
                if (collider.transform.gameObject.CompareTag("Cube"))
                    Debug.Log("Cube hit");
            }*/

        }
    }
}
