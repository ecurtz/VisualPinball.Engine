﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using VisualPinball.Engine.Game;
using VisualPinball.Engine.Resources;
using VisualPinball.Unity.Extensions;
using VisualPinball.Unity.Import;
using BoxCollider = Unity.Physics.BoxCollider;
using Material = UnityEngine.Material;
using Player = VisualPinball.Unity.Game.Player;
using SphereCollider = Unity.Physics.SphereCollider;

namespace VisualPinball.Unity.VPT.Ball
{
	public class BallManager
	{
		private int _id = 0;

		private readonly Engine.VPT.Table.Table _table;
		private readonly EntityManager _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		private readonly GameObject _spherePrefab;

		private static readonly int MainTex = Shader.PropertyToID("_MainTex");
		private static readonly int Metallic = Shader.PropertyToID("_Metallic");
		private static readonly int Glossiness = Shader.PropertyToID("_Glossiness");

		public BallManager(Engine.VPT.Table.Table table)
		{
			_table = table;

			// create a ball "prefab" (it's actually not a prefab, but we'll use it instantiate ball entities)
			_spherePrefab = CreateSphere(CreateMaterial());
			_spherePrefab.SetActive(false);
		}

		public BallApi CreateBall(Player player, IBallCreationPosition ballCreator, float radius, float mass)
		{
			_spherePrefab.SetActive(true);
			_spherePrefab.name = $"Ball{++_id}";
			using (var blobAssetStore = new BlobAssetStore()) {
				var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(_spherePrefab,
					GameObjectConversionSettings.FromWorld(_entityManager.World, blobAssetStore));

				_spherePrefab.SetActive(false);

				var m = player.TableToWorld;
				var ballPos = ballCreator.GetBallCreationPosition(_table).ToUnityFloat3();
				var pos = m.MultiplyPoint(ballPos);
				var rot = Quaternion.LookRotation(
					m.GetColumn(2),
					m.GetColumn(1)
				);
				var scale = new Vector3(
					m.GetColumn(0).magnitude,
					m.GetColumn(1).magnitude,
					m.GetColumn(2).magnitude
				) * (radius * 2);

				// local position
				_entityManager.SetComponentData(entity, new Translation {Value = pos});
				_entityManager.AddComponentData(entity, new Rotation {Value = rot});
				_entityManager.AddComponentData(entity, new NonUniformScale {Value = scale});
				_entityManager.AddComponentData(entity, new BallData {Mass = mass});

				// physics
				var boxCollider = BoxCollider.Create(new BoxGeometry {
					Center = pos,
					Size = new float3(
						radius * 2 * VpxImporter.GlobalScale,
						radius * 2 * VpxImporter.GlobalScale,
						radius * 2 * VpxImporter.GlobalScale
					)
				});
				var collider = SphereCollider.Create(new SphereGeometry {
					Center = pos,
					Radius = radius
				});
				var colliderComponent = new PhysicsCollider {Value = boxCollider};
				_entityManager.AddComponentData(entity, colliderComponent);
				_entityManager.AddComponentData(entity, PhysicsMass.CreateDynamic(colliderComponent.MassProperties, mass * 10));
				_entityManager.AddComponentData(entity, new PhysicsVelocity {
					Linear = float3.zero,
					Angular = float3.zero
				});
				_entityManager.AddComponentData(entity, new PhysicsDamping {
					Linear = 0.01f,
					Angular = 0.05f
				});

				return new BallApi(entity, player);
			}
		}

		private static GameObject CreateSphere(Material material)
		{
			var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.GetComponent<Renderer>().material = material;
			return sphere;
		}

		private static Material CreateMaterial()
		{
			var material = new Material(Shader.Find("Standard"));
			var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true) {name = "BallDebugTexture"};
			texture.LoadImage(Resource.BallDebug.Data);
			material.SetTexture(MainTex, texture);
			material.SetFloat(Metallic, 0.85f);
			material.SetFloat(Glossiness, 0.75f);
			return material;
		}
	}
}
