﻿// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace CandyCoded
{

    public class CameraFollow2D : MonoBehaviour
    {

        [Serializable]
        private struct CameraConstraints2D : IEquatable<CameraConstraints2D>
        {
            [Header("Freeze Original Position")]
            public bool freezePositionX;
            public bool freezePositionY;
            [Header("Maintain Original Offset")]
            public bool maintainOffsetX;
            public bool maintainOffsetY;
            [Header("Restrict Viewport to Transform")]
            public Transform boundsTransform;
            [Header("(or)")]
            [Header("Restrict Viewport to Manually Defined Bounds")]
            public Bounds bounds;
            public bool Equals(CameraConstraints2D other)
            {

                return other.freezePositionX == freezePositionX &&
                    other.freezePositionY == freezePositionY &&
                    other.maintainOffsetX == maintainOffsetX &&
                    other.maintainOffsetY == maintainOffsetY &&
                    other.boundsTransform == boundsTransform &&
                    other.bounds == bounds;

            }
        }

        [SerializeField]
        private bool _tracking = true;
        public bool tracking
        {
            get { return _tracking; }
            set { _tracking = value; }
        }

        [SerializeField]
        private Transform _mainTarget;
        public Transform mainTarget
        {
            get { return _mainTarget; }
            set { _mainTarget = value; }
        }

        [SerializeField]
        private float dampRate = 0.3f;

        [SerializeField]
        private CameraConstraints2D constraints;

        [SerializeField]
        private Transform cameraTransform;
        private Vector3 cameraPositionOffset;

        private float cameraOrthographicSize;

        private Vector3 velocity = Vector3.zero;

#pragma warning disable S4392
        // Disables "Unused private types or members should be removed" warning method is part of MonoBehaviour.
        private void Awake()
        {

            cameraTransform = Camera.main.transform;
            cameraOrthographicSize = Camera.main.orthographicSize;

            if (mainTarget == null)
            {

                mainTarget = gameObject.transform;

            }

            cameraPositionOffset = new Vector2(
                cameraTransform.position.x - mainTarget.transform.position.x,
                cameraTransform.position.y - mainTarget.transform.position.y
            );

        }

        private void LateUpdate()
        {

            if (tracking && mainTarget)
            {

                Vector3 newPosition = mainTarget.transform.position;

                if (constraints.maintainOffsetX)
                {

                    newPosition.x += cameraPositionOffset.x;

                }

                if (constraints.maintainOffsetY)
                {

                    newPosition.y += cameraPositionOffset.y;

                }

                if (constraints.boundsTransform)
                {

                    constraints.bounds = Calculation.ParentBounds(constraints.boundsTransform);

                }

                float cameraExtentHorizontal = cameraOrthographicSize * Screen.width / Screen.height;

                if (Mathf.Abs(constraints.bounds.size.magnitude) >= Single.Epsilon)
                {

                    newPosition.x = Mathf.Clamp(newPosition.x, constraints.bounds.min.x + cameraExtentHorizontal, constraints.bounds.max.x - cameraExtentHorizontal);
                    newPosition.y = Mathf.Clamp(newPosition.y, constraints.bounds.min.y + cameraOrthographicSize, constraints.bounds.max.y - cameraOrthographicSize);

                }

                if (constraints.freezePositionX)
                {

                    newPosition.x = cameraTransform.position.x;

                }

                if (constraints.freezePositionY)
                {

                    newPosition.y = cameraTransform.position.y;

                }

                newPosition.z = cameraTransform.position.z;

                cameraTransform.position = Vector3.SmoothDamp(
                    cameraTransform.position,
                    newPosition,
                    ref velocity,
                    dampRate
                );

            }

        }
#pragma warning restore S4392

    }

}
