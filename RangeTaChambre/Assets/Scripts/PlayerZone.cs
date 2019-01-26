using UnityEngine;

public class PlayerZone : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)]
	private int zoneIndex;

	private void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.GetComponent<Player>();
		player.CurrentZoneIndex = zoneIndex;

		Debug.Log("Player " + (player.PlayerIndex + 1).ToString() + " is in zone " + (zoneIndex + 1).ToString());
	}
}