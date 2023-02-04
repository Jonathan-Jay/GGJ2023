using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]	Vector3 offset;
	public List<Transform> following = new List<Transform>();

	private void LateUpdate() {
		if (following == null && following.Count == 0)	return;

		float minx = float.PositiveInfinity, maxx = float.NegativeInfinity;
		Vector3 sum = Vector3.zero;
		for (int i = 0; i < following.Count;) {
			if (following[i] == null) {
				following.RemoveAt(i);
				continue;
			}
			sum += following[i].position;
			if (following[i].position.x > maxx)
				maxx = following[i].position.x;
			if (following[i].position.x < minx)
				minx = following[i].position.x;
			++i;
		}

		//get average
		sum /= following.Count;

		//figure out aspect ratio
		//TODO

		transform.position = sum + offset;
	}
}
