using System.Collections.Generic;
using UnityEngine;
using System;

namespace SinafProduction.Essentials
{
	public partial class Grid<T>
	{
		public T this[int x, int y, int z]
		{
			get => GetObject(x, y, z);
			set => SetObject(x, y, z, value);
		}

		public T this[Vector3Int position]
		{
			get => GetObject(position.x, position.y, position.z);
			set => SetObject(position.x, position.y, position.z, value);
		}

		/// <summary>
		/// Instancie une nouvelle grille.
		/// </summary>
		/// <param name="width">Taille en largeur.</param>
		/// <param name="height">Taille en longueur.</param>
		/// <param name="depth">Taille en profondeur</param>
		/// <param name="func">Fonction à lancer, pour définire l'objet dans la grille (envoi la position x et y de l'objet).</param>
		/// <param name="startVertical">Démarrer la boucle verticalement.</param>
		public Grid(int width, int height, int depth, Func<int, int, int, T> func, bool startVertical = true)
		{
			this.width = width;
			this.height = height;
			this.depth = depth;

			gridArray = new T[width, height, depth];
			if (startVertical)
				for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				for (var z = 0; z < depth; z++)
					gridArray[x, y, z] = func(x, y, z);
			else
				for (var y = 0; y < height; y++)
				for (var x = 0; x < width; x++)
				for (var z = 0; z < depth; z++)
					gridArray[x, y, z] = func(x, y, z);
		}
		
		/// <summary>
		/// Convertie le x, y et le z en int d'un Vector3.
		/// </summary>
		/// <param name="value">Vector3 à convertire.</param>
		/// <param name="x">Sortie du x.</param>
		/// <param name="y">Sortie du y.</param>
		/// <param name="z">Sortie du z.</param>
		public void GetXYZ(Vector3 value, out int x, out int y, out int z)
		{
			x = Mathf.FloorToInt(value.x);
			y = Mathf.FloorToInt(value.y);
			z = Mathf.FloorToInt(value.z);
		}
		
		/// <summary>
		/// Définie un objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="z">Position z de l'objet.</param>
		/// <param name="obj">Nouveau objet à définir.</param>
		public void SetObject(int x, int y, int z, T obj)
		{
			try
			{
				gridArray[x, y, z] = obj;
			}
			catch (IndexOutOfRangeException)
			{
				throw new Exception($"La position: ({x}:{y}:{z}) se trouve en dehors de la grille.");
			}
		}
		
		/// <summary>
		/// Définie un objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="z">Position z de l'objet.</param>
		/// <param name="obj">Nouveau objet à définir.</param>
		public bool TrySetObject(int x, int y, int z, T obj)
		{
			if (CheckOutOfRangeArray(x, y, z))
				return false;

			gridArray[x, y, z] = obj;
			return true;
		}

		/// <summary>
		/// Récupère tout les objets z sur la grille.
		/// </summary>
		/// <param name="x">Position x des objets.</param>
		/// <param name="y">Position y des objets.</param>
		/// <returns>Retourne les objets trouvé.</returns>
		public T[] GetZObjects(int x, int y)
		{
			var objects = new List<T>();
			if (CheckOutOfRangeArray(x, y)) return objects.ToArray();
			
			for (var z = 0; z < depth; z++)
			{
				var obj = gridArray[x, y, z];
				if (obj != null) objects.Add(obj);
			}

			return objects.ToArray();
		}
		
		/// <summary>
		/// Récupére l'objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="z">Position z de l'objet.</param>
		/// <returns>Retourne l'objet demandé.</returns>
		public T GetObject(int x, int y, int z)
		{
			try
			{
				return gridArray[x, y, z];
			}
			catch (IndexOutOfRangeException)
			{
				throw new Exception($"La position: ({x}:{y}:{z}) se trouve en dehors de la grille.");
			}
		}
		
		/// <summary>
		/// Récupére l'objet sur la grille.
		/// </summary>
		/// <param name="x">Position x de l'objet.</param>
		/// <param name="y">Position y de l'objet.</param>
		/// <param name="z">Position z de l'objet.</param>
		/// <param name="obj">L'objet trouvé.</param>
		/// <returns>Retourne si l'objet à été trouvé.</returns>
		public bool TryGetObject(int x, int y, int z, out T obj)
		{
			if (CheckOutOfRangeArray(x, y, z))
			{
				obj = default;
				return false;
			}

			obj = gridArray[x, y, z];
			return obj != null;
		}

		/// <summary>
		/// Vérifie si si les valeurs x et y, dépasse la portée de la grille.
		/// </summary>
		/// <param name="x">Position x à vérifier.</param>
		/// <param name="y">Position y à vérifier.</param>
		/// <param name="z">Position z à vérifier.</param>
		/// <returns>Retourne si une des valeurs est hors-portée.</returns>
		public bool CheckOutOfRangeArray(int x, int y, int z) => x < 0 || x >= width || y < 0 || y >= height || z < 0 || z >= depth;
	}
}