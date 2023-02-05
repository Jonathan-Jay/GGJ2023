using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]	Vector3 offset;
	[SerializeField]	float distanceScaler = 2f;
	[SerializeField]	float minWidth = 10f;
	[SerializeField]	float speed = 10f;
	public Transform fallback;
	public List<Transform> following = new List<Transform>();
	public List<Color> colors = new List<Color>();
	static public List<Color> colours = new List<Color>();

	[HideInInspector]
	public int playerCount = 0;

	private void Awake() {
		colours = colors;
	}

	private void LateUpdate() {
		if (following.Count == 0 || playerCount == 0) {
			if (fallback) {
				transform.position = Vector3.MoveTowards(transform.position, fallback.position + offset, speed * 5f * Time.deltaTime);
			}

			return;
		}

		float minx = float.PositiveInfinity, maxx = float.NegativeInfinity;
		Vector3 sum = Vector3.zero;
		for (int i = 0; i < following.Count;) {
			if (following[i] == null || following[i].gameObject.layer == 10) {
				following.RemoveAt(i);
				continue;
			}
			if (!following[i].GetComponent<MainBnuuy>()) {
				sum += following[i].position / (float)playerCount;
			}
			else { 
				sum += following[i].position * (following.Count * 0.5f + 1f);
			}
			if (following[i].position.x > maxx)
				maxx = following[i].position.x;
			if (following[i].position.x < minx)
				minx = following[i].position.x;
			++i;
		}

		//if something gets killed in the loop
		if (following.Count <= 0) return;

		//get average
		sum /= following.Count * 1.5f;

		//figure out distance ratio
		offset.z = -Mathf.Max(minWidth, (maxx - minx) * distanceScaler);

		transform.position = Vector3.MoveTowards(transform.position, sum + offset, speed * Time.deltaTime);
	}
}
