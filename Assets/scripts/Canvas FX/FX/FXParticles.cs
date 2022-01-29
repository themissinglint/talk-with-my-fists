using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FXParticles : FX {

    private List<ParticleSystem> _particleSystems;
    private List<ParticleSystem> ParticleSystems {
        get {
            if (_particleSystems == null) {
                _particleSystems = new List<ParticleSystem>();
                _particleSystems = GetComponentsInChildren<ParticleSystem>().ToList();
            }
            return _particleSystems;
        }
    }
    
    public override void Trigger(FXArgs args) {
        bool rotate = args.InputVector != Vector2.zero;
        foreach (ParticleSystem ps in ParticleSystems) {
            if (rotate) {
                ps.transform.localRotation = Quaternion.identity;
                ps.transform.Rotate(0f, 0f, MathUtilities.RotationToLookAtVector(Vector2.right, args.InputVector));
            }
            ps.Play();
        }
    }
    
}
