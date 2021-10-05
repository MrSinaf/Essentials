using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System.Net;
using System;

namespace SinafProduction.Essentials
{
	public static class Utility
	{
		public static bool CheckInternetConnection()
		{
			try
			{
				using var client = new WebClient();
				using (client.OpenRead("https://google.com/generate_204")) 
					return true;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Récupére la position de la sourie.
		/// </summary>
		/// <returns>Retourne la positon de la sourie, avec z = 0.</returns>
		public static Vector3 GetMouseWorldPosition()
		{
			var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			position.z = 0;

			return position;
		}

		/// <summary>
		/// Chronométre la vitesse d'une Action.
		/// </summary>
		/// <param name="action">Action à chronométrer.</param>
		/// <param name="showInLog">Doit-il afficher le résultat dans les logs?</param>
		/// <returns>Retourne le temps écoulé.</returns>
		public static string StopWatchAction(Action action, bool showInLog = false)
		{
			var startTime = Time.realtimeSinceStartup;
			action();
			var endTime = (Time.realtimeSinceStartup - startTime) * 1000f + "ms";

			if (showInLog)
				Debug.Log(endTime);

			return endTime;
		}

		/// <summary>
		/// Ignore toute les exceptions.
		/// </summary>
		/// <param name="action">Action a produire.</param>
		/// <param name="so">Si l'action a une erreur, alors...</param>
		public static void IgnoreException(Action action, Action so = null)
		{
			try
			{
				action();
			}
			catch
			{
				so?.Invoke();
			}
		}

		/// <summary>
		/// Ignore toute les exceptions.
		/// </summary>
		/// <param name="action">Function a produire.</param>
		/// <param name="so">Si la function a une erreur, alors...</param>
		public static T IgnoreException<T>(Func<T> action, Func<T> so = null)
		{
			try
			{
				return action();
			}
			catch
			{
				return so != null ? so() : default;
			}
		}

		public static Vector3 Abs(Vector3 value)
		{
			value.x = Mathf.Abs(value.x);
			value.y = Mathf.Abs(value.y);
			value.z = Mathf.Abs(value.z);
			return value;
		}

		#region Coroutines
		private static IEnumerator _StartActionLater(Action action, float time)
		{
			yield return new WaitForSeconds(time);
			action();
		}

		private static IEnumerator _StartActionLater(UnityAction action, float time)
		{
			yield return new WaitForSeconds(time);
			action();
		}
		#endregion

		#region Extensions

		/// <summary>
		/// Calcule la distance entre deux float.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static float DistanceTo(this float a, float b)
			=> Mathf.Abs(b - a);
		
		
		/// <summary>
		/// Calcule la distance entre deux int.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int DistanceTo(this int a, int b)
			=> Mathf.Abs(b - a);

		/// <summary>
		/// Démarre une Action après le temps indiqué.
		/// </summary>
		/// <param name="instance">Instance qui fait démarrer l'Action.</param>
		/// <param name="action">Action à éxécuter.</param>
		/// <param name="time">Temps d'attente (en seconde).</param>
		public static Coroutine StartActionLater(this MonoBehaviour instance, Action action, float time)
			=> instance.StartCoroutine(_StartActionLater(action, time));

		/// <summary>
		/// Démarre une Action après le temps indiqué.
		/// </summary>
		/// <param name="instance">Instance qui fait démarrer l'Action.</param>
		/// <param name="action">Action à éxécuter.</param>
		/// <param name="time">Temps d'attente (en seconde).</param>
		public static Coroutine StartUnityActionLater(this MonoBehaviour instance, UnityAction action, float time)
			=> instance.StartCoroutine(_StartActionLater(action, time));

		/// <summary>
		/// Ajoute une couleur au texte.
		/// </summary>
		/// <param name="text">Texte où la couleur doit être appliqué.</param>
		/// <param name="color">Couleur à appliquer</param>
		/// <returns>Retourne le résultat final.</returns>
		public static string Color(this string text, Color color) => $"<#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";

		public static string Color(this float numb, Color color) => Color(numb.ToString(), color);
		public static string Color(this int numb, Color color) => Color(numb.ToString(), color);

		/// <summary>
		/// Copie le mesh donné.
		/// </summary>
		/// <param name="mesh">Mesh à copier.</param>
		/// <returns>Retourne le nouveau mesh.</returns>
		public static Mesh Copy(this Mesh mesh)
		{
			var newMesh = new Mesh
			{
				vertices = mesh.vertices,
				uv = mesh.uv,
				triangles = mesh.triangles
			};
			return newMesh;
		}
		#endregion
	}
}