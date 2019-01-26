using UnityEngine;

public class PlayerZone : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)]
	private int zoneIndex;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.GetComponent<Player>();
		player.CurrentZoneIndex = zoneIndex;
	}
}