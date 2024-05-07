
    using System;
    using UnityEngine;

    public static class TextureExtensions
    {
        public static Vector2 Size(this Texture2D texture)
        {
            if (texture is null) throw new ArgumentNullException(nameof(texture));
            return new Vector2(texture.width, texture.height);
        }
    }
