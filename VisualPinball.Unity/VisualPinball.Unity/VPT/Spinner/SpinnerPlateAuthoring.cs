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
using Unity.Mathematics;
using VisualPinball.Engine.Common;
using VisualPinball.Engine.VPT.Spinner;

namespace VisualPinball.Unity
{
	internal class SpinnerPlateAuthoring : ItemMainAuthoring<Spinner, SpinnerData>, IConvertGameObjectToEntity
	{

		protected override Type MeshAuthoringType { get; } = null;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			Convert(entity, dstManager);

			dstManager.AddComponentData(entity, new SpinnerStaticData {
				AngleMax = math.radians(Data.AngleMax),
				AngleMin = math.radians(Data.AngleMin),
				Damping = math.pow(Data.Damping, (float)PhysicsConstants.PhysFactor),
				Elasticity = Data.Elasticity,
				Height = Data.Height
			});

			dstManager.AddComponentData(entity, new SpinnerMovementData {
				Angle = math.radians(math.clamp(0.0f, Data.AngleMin, Data.AngleMax)),
				AngleSpeed = 0f
			});

			// register
			var spinner = transform.parent.gameObject.GetComponent<SpinnerAuthoring>().Item;
			transform.GetComponentInParent<Player>().RegisterSpinner(spinner, entity, gameObject);
		}

		protected override Spinner InstantiateItem(SpinnerData data) => transform.parent.gameObject.GetComponent<SpinnerAuthoring>().Item;
	}
}
