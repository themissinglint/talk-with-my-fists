﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class FXApplierTransform : MonoBehaviour {

   private Transform _transform;
   private Vector2 _baseScale;
   private Vector2 _basePosition;
   private Quaternion _baseRotation;
   
   private List<FXFactorTransform> _factors = new List<FXFactorTransform>();
   
   // Testing Only
   private Transform _squashInversionTransform;
   private const float magnitudeCap = 0.5f;

   private void Awake() {
      _transform = transform;
      _baseScale = _transform.localScale;
      _basePosition = _transform.localPosition;
      _baseRotation = _transform.rotation;
      GameObject squashObject = Instantiate(new GameObject(), _transform.parent);
      squashObject.name = "Squash Transform";
      _squashInversionTransform = squashObject.transform;
      _transform.SetParent(_squashInversionTransform);
   }

   public void AddFactor(FXFactorTransform newFactor) {
      _factors.Add(newFactor);
   }
   
   private void Update() {
      ExpireFactors();
      Vector2 scale = CalculateScale();
      Vector2 offset = CalculateOffset();
      float rotation = CalculateRotation();
      Vector2 squashScale = CalculateSquash(out float squashRotation);
      
      _transform.localScale = new Vector2(_baseScale.x * scale.x * squashScale.x, _baseScale.y * scale.y * squashScale.y);
      _transform.localPosition = _basePosition + MathUtilities.RotateVector2(offset, squashRotation);
      _transform.rotation = _baseRotation;
      _squashInversionTransform.rotation = Quaternion.identity;
      _transform.Rotate(0f, 0f, squashRotation + rotation);
      _squashInversionTransform.Rotate(0f, 0f, -squashRotation);
   }

   private void ExpireFactors() {
      _factors.StrikeWhere(e => e.IsExpired);
   }

   private Vector2 CalculateScale() {
      Vector2 scale = new Vector2(1f, 1f);
      foreach (FXFactorTransform factor in _factors) {
         scale += factor.GetScaleVector();
      }
      return scale;
   }

   private Vector2 CalculateOffset() {
      Vector2 offset = Vector2.zero;
      foreach (FXFactorTransform factor in _factors) {
         offset += factor.GetOffset();
      }
      return offset;
   }

   private float CalculateRotation() {
      float rotation = 0f;
      foreach (FXFactorTransform factor in _factors) {
         rotation += factor.GetRotation();
      }
      return rotation;
   }

   private Vector2 CalculateSquash (out float squashRotation) {
      // Calculate Final Vector
      List<Vector2> squashVectors = _factors.Select(e => e.GetSquashVector()).ToList();
      Vector2 sumVector = new Vector2(squashVectors.Sum(e => e.x), squashVectors.Sum(e => e.y));
      float mag = sumVector.magnitude;
      if (mag > magnitudeCap) {
         sumVector *= magnitudeCap / mag;
      }
      squashRotation = MathUtilities.RotationToLookAtVector(Vector2.up, sumVector);
      
      // Apply Squash Scaling
      float scaleMultiplier = 1f + sumVector.magnitude;
      return new Vector2(scaleMultiplier, 1f / scaleMultiplier);
   }

}
