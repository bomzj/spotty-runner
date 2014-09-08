using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuslikGames.SpottyRunner.Classes.Extensions
{
    public static class BoxCollider2DExtensions
    {
        public static Rect GetWorldBounds(this BoxCollider2D boxCollider2D)
        {
            float worldRight = boxCollider2D.transform.TransformPoint(boxCollider2D.center + new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;
            float worldLeft = boxCollider2D.transform.TransformPoint(boxCollider2D.center - new Vector2(boxCollider2D.size.x * 0.5f, 0)).x;

            float worldTop = boxCollider2D.transform.TransformPoint(boxCollider2D.center + new Vector2(0, boxCollider2D.size.y * 0.5f)).y;
            float worldBottom = boxCollider2D.transform.TransformPoint(boxCollider2D.center - new Vector2(0, boxCollider2D.size.y * 0.5f)).y;

            return new Rect(
                worldLeft,
                worldBottom,
                worldRight - worldLeft,
                worldTop - worldBottom
                );
        }
    }
}
