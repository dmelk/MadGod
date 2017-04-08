using UnityEngine;
using System.Collections;

public class RepeatedSprite : MonoBehaviour {

	void Awake() {
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		Vector2 spriteSize = new Vector2 (sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

		GameObject childPrefab = new GameObject ();
		SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer> ();
		childPrefab.transform.position = transform.position;
		childSprite.sprite = sprite.sprite;
		childSprite.sortingLayerID = sprite.sortingLayerID;

		GameObject child;
		int length = (int)Mathf.Round (sprite.bounds.size.x);
		int height = (int)Mathf.Round (sprite.bounds.size.y);
		float startX = -1 * spriteSize.x * (length - 1) / 2;
		float startY = spriteSize.y * (height - 1) / 2;
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < height; j++) {
				child = Instantiate (childPrefab) as GameObject;
				child.transform.position = transform.position + new Vector3 (startX + spriteSize.x * i, startY - spriteSize.y * j, 0);
				child.transform.parent = transform;
			}
		}
		childPrefab.transform.parent = transform;

		sprite.enabled = false;
	}

}
