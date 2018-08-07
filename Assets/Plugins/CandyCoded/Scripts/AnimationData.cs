﻿// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CandyCoded
{

    public struct TransformData : IEquatable<TransformData>
    {

        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }

        public bool Equals(TransformData other)
        {

            return other.Position == Position && other.Scale == Scale && other.Rotation == Rotation;

        }

    }

    public struct MaterialData : IEquatable<MaterialData>
    {

        public Material material { get; set; }
        public Color startColor { get; set; }

        public bool Equals(MaterialData other)
        {

            return other.material == material && other.startColor == startColor;

        }

    }

    public class AnimationData : MonoBehaviour
    {

        private TransformData _transformData = new TransformData();
        public TransformData TransformData
        {
            get
            {
                return _transformData;
            }
        }

        private List<MaterialData> _materials = new List<MaterialData>();
        public List<MaterialData> Materials
        {
            get
            {
                return _materials;
            }
        }

#pragma warning disable S1144
        // Disables "Unused private types or members should be removed" warning method is part of MonoBehaviour.
        private void Awake()
        {

            RebuildCachedData();

        }
#pragma warning restore S1144

        /// <summary>
        /// Rebuilds all cache data related to basic animations: initial transform and material color data.
        /// </summary>
        /// <returns>void</returns>
        public void RebuildCachedData()
        {

            CacheTransformData();
            CacheMaterials();

        }

        /// <summary>
        /// Rebuilds all cache transform data.
        /// </summary>
        /// <returns>void</returns>
        public void CacheTransformData()
        {

            _transformData.Position = gameObject.transform.localPosition;
            _transformData.Scale = gameObject.transform.localScale;
            _transformData.Rotation = gameObject.transform.localRotation;

        }

        /// <summary>
        /// Rebuilds all cache material color data.
        /// </summary>
        /// <returns>void</returns>
        public void CacheMaterials()
        {

            _materials = new List<MaterialData>();

            var materialsInChildren = CandyCoded.Materials.GetMaterialsInChildren(gameObject);

            foreach (Material material in materialsInChildren)
            {

                var materialData = new MaterialData
                {
                    material = material,
                    startColor = material.color
                };

                _materials.Add(materialData);

            }

        }

    }

}
