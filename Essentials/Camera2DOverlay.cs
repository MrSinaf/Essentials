using UnityEngine;

namespace SinafProduction.Essentials
{
	/// <summary>
	/// Gère la caméra overlay au mieux pour la caméra 2D.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class Camera2DOverlay : MonoBehaviour
	{
		private static GameObject local;

		public static void Active(bool value)
		{
			if (local)
				local.SetActive(value);
		}

		private Camera mainCamera;
		private Camera localCamera;

		private void Awake()
		{
			localCamera = GetComponent<Camera>();
			mainCamera = Camera.main;
			local = gameObject;
			local.SetActive(false);
		}

		private void LateUpdate()
			=> localCamera.orthographicSize = mainCamera.orthographicSize;
	}
}