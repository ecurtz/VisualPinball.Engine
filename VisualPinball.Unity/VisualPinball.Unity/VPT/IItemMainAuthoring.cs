// Visual Pinball Engine
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
using System.Collections.Generic;
using UnityEngine;

namespace VisualPinball.Unity
{
	public interface IItemMainAuthoring : IItemAuthoring
	{
		bool IsLocked { get; set; }

		bool CanBeTransformed { get; }

		IEnumerable<Type> ValidParents { get; }

		GameObject gameObject { get; }

		/// <summary>
		/// Sets the mesh of all mesh sub components to dirty.
		/// </summary>
		void SetMeshDirty();
		void RebuildMeshIfDirty();

		void Destroy();
		void DestroyMeshComponent();
		void DestroyColliderComponent();

		// the following interfaces allow each item behavior to define which axes should
		// be shown on the scene view gizmo, the gizmo itself will use the associated
		// get and set methods, which are expected to update item data directly
		ItemDataTransformType EditorPositionType { get; }
		Vector3 GetEditorPosition();
		void SetEditorPosition(Vector3 pos);

		ItemDataTransformType EditorRotationType { get; }
		Vector3 GetEditorRotation();
		void SetEditorRotation(Vector3 pos);

		ItemDataTransformType EditorScaleType { get; }
		Vector3 GetEditorScale();
		void SetEditorScale(Vector3 pos);
	}

	public enum ItemDataTransformType
	{
		None,
		OneD,
		TwoD,
		ThreeD,
	}
}
