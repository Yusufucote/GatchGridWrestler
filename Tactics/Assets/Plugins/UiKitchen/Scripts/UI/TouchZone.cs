using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UiKitchen
{
	/// <summary>
	/// Touch area is a Graphic that displays nothing, but the default GraphicRaycaster will find it
	/// </summary>
	public class TouchZone : Graphic
	{
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			// The VertexHelper passed to OnPopulateMesh is shared between different canvas elements.
			// It may contain leftover data from a previous widget.
			// Make sure we draw no geometry by clearing the VertexHelper.
			vh.Clear();
		}
	}
}
