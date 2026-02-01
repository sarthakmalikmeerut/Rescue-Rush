using System;
using UnityEngine;

public class OrderingLayerFix : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera cam;
    public float distFromTop;

    [SerializeField] float multiFactor = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // this takes game object's world position to viewport's position
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        
        // This is important
        // this gets the exact top point on the viewport on which below is the game object.
        Vector3 top = cam.ViewportToWorldPoint(new Vector3(viewportPos.x, 1f, cam.nearClipPlane));
        
        // this calculates the distance from the top point on viewport and the game object position
        distFromTop = Vector3.Distance(transform.position, top);

        // finally the larger is the distance the larger is the sorting order.
        // and it is totally dynamic based on the object relative position W.R.T viewport top point(also dynamic)
        int order = (int)(distFromTop * multiFactor);
        spriteRenderer.sortingOrder = order;
    }
}
