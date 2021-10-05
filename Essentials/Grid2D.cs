using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace SinafProduction.Essentials
{
	/// <summary>
	/// Crée une grille en 2D ou 3D.
	/// </summary>
	/// <typeparam name="T">Type du contenue dans la grille.</typeparam>
	public partial class Grid<T> : IEnumerable<T>
	{
		private readonly T[,,] gridArray;
		public readonly int height;
		public readonly int width;
		public readonly int depth;
		
		public T this[int x, int y]
		{
			get => GetObject(x, y);
			set => SetObject(x, y, value);
		}
		
		public T this[Vector2Int position]
		{
			get => GetObject(position.x, position.y, 0);
			set => SetObject(position.x, position.y, 0, value);
		}

		/// <summary>
		/// Instancie une nouvelle grille.
		/// </summary>
		/// <param name="width">Taille en largeur.</param>
		/// <param name="height">Taille en longueur.</param>
		/// <param name="func">Fonction à lancer, pour définire l'objet dans la grille (envoi la position x et y de l'objet).</param>
		/// <param name="startVertical">Démarrer la boucle verticalement.</param>
		public Grid(int width, int height, Func<int, int, T> func, bool startVertical = true)
		{
			this.width = width;
			this.height = height;
			depth = 1;

			gridArray = new T[width, height, depth];
			if (startVertical)
				for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
					gridArray[x, y, 0] = func(x, y);
			else
				for (var y = 0; y < height; y++)
				for (var x = 0; x < width; x++)
					gridArray[x, y, 0] = func(x, y);
		}

		/// <summary>
		/// Convertie le x et le y en int d'un Vector3.
		/// </summary>
		/// <param name="value">Vector3 à convertire.</param>
		/// <param name="x">Sortie du x.</param>
		/// <param name="y">Sortie du y.</param>
		public void GetXY(Vector3 value, out int x, out int y)
		{
			x = Mathf.FloorToInt(value.x);
			y = Mathf.FloorToInt(value.y);
		}

		/// <summary>
		/// Echange la position entre deux objets.
		/// </summary>
		/// <param name="xA">Position x de l'objet A.</param>
		/// <param name="yA">Position y de l'objet A.</param>
		/// <param name="xB">Position x de l'objet B.</param>
		/// <param name="yB">Position y de l'objet B.</param>
		public void EchangePosition(int xA, int yA, int xB, int yB)
			=> (gridArray[xA, yA, 0], gridArray[xB, yB, 0]) = (gridArray[xB, yB, 0], gridArray[xA, yA, 0]);

		/// <summary>
		/// Définie un objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="obj">Nouveau objet à définir.</param>
		public void SetObject(int x, int y, T obj)
		{
			try
			{
				gridArray[x, y, 0] = obj;
			}
			catch (IndexOutOfRangeException)
			{
				throw new Exception($"La position: ({x}:{y}) se trouve en dehors de la grille.");
			}
		}

		/// <summary>
		/// Définie un objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="obj">Nouveau objet à définir.</param>
		public bool TrySetObject(int x, int y, T obj)
		{
			if (CheckOutOfRangeArray(x, y))
				return false;

			gridArray[x, y, 0] = obj;
			return true;
		}

		/// <summary>
		/// Récupére l'objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <returns>Retourne l'objet demandé.</returns>
		public T GetObject(int x, int y)
		{
			try
			{
				return gridArray[x, y, 0];
			}
			catch (IndexOutOfRangeException)
			{
				throw new Exception($"La position: ({x}:{y}) se trouve en dehors de la grille.");
			}
		}

		/// <summary>
		/// Récupére l'objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="obj">L'objet trouvé.</param>
		/// <returns>Retourne si l'objet à été trouvé.</returns>
		public bool TryGetObject(int x, int y, out T obj)
		{
			if (CheckOutOfRangeArray(x, y))
			{
				obj = default;
				return false;
			}

			obj = gridArray[x, y, 0];
			return obj != null;
		}

		/// <summary>
		/// Vérifie si si les valeurs x et y, dépasse la portée de la grille.
		/// </summary>
		/// <param name="x">Position x à vérifier.</param>
		/// <param name="y">Position y à vérifier.</param>
		/// <returns>Retourne si une des valeurs est hors-portée.</returns>
		public bool CheckOutOfRangeArray(int x, int y) => x < 0 || x >= width || y < 0 || y >= height;

		#region Interface
		public IEnumerator<T> GetEnumerator()
		{
			for (var y = 0; y < height; y++)
			for (var x = 0; x < width; x++)
			for (var z = 0; z < depth; z++)
				yield return gridArray[x, y, z];
		}

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		#endregion
	}
}