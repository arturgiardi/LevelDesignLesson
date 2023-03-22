using UnityEngine;

public class OrderInLayerUpdater : MonoBehaviour {
    SpriteRenderer sr;
	Canvas canvas;
	[SerializeField] int _offset = 0;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
		canvas = GetComponent<Canvas>();
	}
	
	void Update () {
        if(sr)
			sr.sortingOrder = -(int)Mathf.Ceil(transform.position.y*10) + _offset;
		else if (canvas)
			canvas.sortingOrder = (-(int)Mathf.Ceil(transform.position.y * 10));
	}
}
