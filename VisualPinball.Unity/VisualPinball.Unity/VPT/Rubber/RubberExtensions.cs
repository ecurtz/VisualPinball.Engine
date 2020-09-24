﻿// Visual Pinball Engine
// Copyright (C) 2020 freezy and VPE Team
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

using System;
using Unity.Entities;
using UnityEngine;
using VisualPinball.Engine.Game;

namespace VisualPinball.Unity
{
	internal static class RubberExtensions
	{
		public static MonoBehaviour SetupGameObject(this Engine.VPT.Rubber.Rubber rubber, GameObject obj, RenderObjectGroup rog)
		{
			MonoBehaviour mb = null;
			switch (rog.SubComponent) {
				case RenderObjectGroup.ItemSubComponent.None:

					mb = obj.AddComponent<RubberAuthoring>().SetItem(rubber);
					obj.AddComponent<RubberColliderAuthoring>();
					break;

				case RenderObjectGroup.ItemSubComponent.Collider:
					obj.AddComponent<RubberColliderAuthoring>().SetItem(rubber, rog);
					break;

				case RenderObjectGroup.ItemSubComponent.Mesh:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
			obj.AddComponent<ConvertToEntity>();
			return mb;
		}
	}
}
