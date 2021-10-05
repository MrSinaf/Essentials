using UnityEngine;

namespace SinafProduction.Essentials
{
	public static class TextureUtility
	{
		/// <summary>
		/// Crée une texture transparente.
		/// </summary>
		/// <param name="width">Largeur de la texture.</param>
		/// <param name="height">Hauteur de la texture.</param>
		/// <returns>Retourne la texture.</returns>
		public static Texture2D CreateTexture(int width, int height)
		{
			var texture = new Texture2D(width, height, TextureFormat.RGBA32, true) {filterMode = FilterMode.Trilinear};
			texture.SetPixels(0, 0, width, height, new Color[width * height]);
			return texture;
		}

		/// <summary>
		/// Teint une texture avec un mask incorporé.
		/// </summary>
		/// <param name="texture">Texture et mask.</param>
		/// <param name="tints">Teintures.</param>
		/// <returns>Retourne la référence de textureBase.</returns>
		public static Color[] TintTexture(Color[] texture, Color[] tints)
		{
			for (var i = 0; i < texture.Length; i++)
				if (texture[i].a > 0)
				{
					// Couleur primaire
					if (tints.Length > 0 && texture[i].r > 0)
					{
						var color = texture[i].r;
						texture[i].r = color * tints[0].r;
						texture[i].g = color * tints[0].g;
						texture[i].b = color * tints[0].b;
					}

					// Couleur secondaire:
					else if (tints.Length > 1 && texture[i].g > 0)
					{
						var color = texture[i].g;
						texture[i].r = color * tints[1].r;
						texture[i].g = color * tints[1].g;
						texture[i].b = color * tints[1].b;
					}
					
					// Couleur tertiaire:
					else if (tints.Length > 2 && texture[i].b > 0)
					{
						var color = texture[i].b;
						texture[i].r = color * tints[2].r;
						texture[i].g = color * tints[2].g;
						texture[i].b = color * tints[2].b;
					}
				}

			return texture;
		}
		
		/// <summary>
		/// Teint une texture à partir d'un mask.
		/// </summary>
		/// <param name="textureBase">Base de la texture.</param>
		/// <param name="tints">Teintures.</param>
		/// <param name="textureMask">Mask à utiliser.</param>
		/// <returns>Retourne la référence de textureBase.</returns>
		public static Color[] TintTextureFromMask(Color[] textureBase, Color[] tints, Color[] textureMask)
		{
			for (var i = 0; i < textureBase.Length; i++)
				if (textureMask[i].a > 0)
				{
					// Couleur primaire
					if (tints.Length > 0 && textureMask[i].r > 0)
					{
						textureBase[i].r *= tints[0].r;
						textureBase[i].g *= tints[0].g;
						textureBase[i].b *= tints[0].b;
					}

					// Couleur secondaire:
					else if (tints.Length > 1 && textureMask[i].g > 0)
					{
						textureBase[i].r *= tints[1].r;
						textureBase[i].g *= tints[1].g;
						textureBase[i].b *= tints[1].b;
					}

					// Couleur tertiaire:
					else if (tints.Length > 2 && textureMask[i].b > 0)
					{
						textureBase[i].r *= tints[2].r;
						textureBase[i].g *= tints[2].g;
						textureBase[i].b *= tints[2].b;
					}
				}

			return textureBase;
		}
		
		public static Color[] MergeTexture(Color[] textureBase, Color[] overlay)
		{
			for (var i = 0; i < textureBase.Length; i++)
				if (overlay[i].a > 0)
				{
					// Remplace la couleur opaque:
					if (overlay[i].a >= 1)
					{
						textureBase[i] = overlay[i];
					}
					else
					{
						// Fusionne la couleur transparente:
						var alpha = overlay[i].a;
						textureBase[i].r += (overlay[i].r - textureBase[i].r) * alpha;
						textureBase[i].g += (overlay[i].g - textureBase[i].g) * alpha;
						textureBase[i].b += (overlay[i].b - textureBase[i].b) * alpha;
						textureBase[i].a += overlay[i].a;
					}
				}

			return textureBase;
		}
		
		public static int ColorNumber(Color[] textureMask)
		{
			var colorNumber = 0;
			for (var i = 0; i < textureMask.Length; i++)
				if (textureMask[i].a > 0)
				{
					// Couleur primaire
					if (colorNumber < 1 && textureMask[i].r > 0)
						colorNumber = 1;

					// Couleur secondaire:
					else if (colorNumber < 2 && textureMask[i].g > 0)
						colorNumber = 2;

					// Couleur tertiaire:
					else if (textureMask[i].b > 0)
						return 3;
				}

			return colorNumber;
		}
	}
}