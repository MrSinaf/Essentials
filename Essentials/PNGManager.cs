using UnityEngine;
using System.IO;

namespace SinafProduction.Essentials
{
	public static class PNGManager
	{
		/// <summary>
		/// Enregistre une texture à un chemin choisi.
		/// </summary>
		/// <param name="texture">Texture à enregistrer.</param>
		/// <param name="path">Chemin d'enregistrement.</param>
		public static void Upload(Texture2D texture, string path)
		{
			var bytes = texture.EncodeToPNG();
			File.WriteAllBytes($"{path}.png", bytes);
		}

		/// <summary>
		/// Charge une texture à un chemin choisi.
		/// </summary>
		/// <param name="path">Chemin du chargement.</param>
		/// <returns>Retourne la texture chargé.</returns>
		public static Texture2D Load(string path)
		{
			var png = File.ReadAllBytes($"{path}.png");
			var texture = new Texture2D(1, 1);
			texture.LoadImage(png);

			return texture;
		}

		public static byte[] GetBytes(Texture2D texture) => texture.EncodeToPNG();

		public static Texture2D SetBytes(byte[] bytes)
		{
			var texture = new Texture2D(1, 1);
			texture.LoadImage(bytes);
			return texture;
		}
	}
}