using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Serialization;
using UnityEngine;

public class FXApplierColor : MonoBehaviour {
    
    private List<FXFactorColor> _factors = new List<FXFactorColor>();
    private List<SpriteRenderer> _originalSprites;
    private Dictionary<string, List<SpriteRenderer>> _colorizerSprites = new Dictionary<string, List<SpriteRenderer>>();

    private void Awake() {
        _originalSprites = GetComponentsInChildren<SpriteRenderer>().ToList();
    }
    
    public void AddFactor(FXFactorColor newFactor) {
        _factors.Add(newFactor);
        if (!_colorizerSprites.ContainsKey(newFactor.MaterialReference.name)) {
            AddColorizerSpritesForMaterial(newFactor.MaterialReference);
        }
    }

    private void Update() {
        ExpireFactors();
        foreach (KeyValuePair<string, List<SpriteRenderer>> materialGroup in _colorizerSprites) {
            List<FXFactorColor> relevantFactors = _factors.Where(e => e.MaterialReference.name == materialGroup.Key).ToList();
            List<ColorWeight> colorWeights = new List<ColorWeight> {new ColorWeight(Color.clear, 1f) {ChannelWeights = new Color(0.001f, 0.001f, 0.001f, 1f)}};
            foreach (FXFactorColor factor in relevantFactors) {
                colorWeights.Add(new ColorWeight(factor.GetColor(), factor.GetStrength()));
            }
            Color resultColor = ColorUtilities.BlendColors(colorWeights);
            foreach (SpriteRenderer sr in materialGroup.Value) {
                sr.color = resultColor;
            }
        }
    }
    
    private void ExpireFactors() {
        _factors.StrikeWhere(e => e.IsExpired);
    }

    private void AddColorizerSpritesForMaterial(Material material) {
        List<SpriteRenderer> newSprites = new List<SpriteRenderer>();
        for (int i = 0; i < _originalSprites.Count; i++) {
            _originalSprites[i].sortingOrder += i;
            var highestSortOrderSoFar = _originalSprites[i].sortingOrder;
            foreach (KeyValuePair<string, List<SpriteRenderer>> kvp in _colorizerSprites) {
                kvp.Value[i].sortingOrder += i;
                if (kvp.Value[i].sortingOrder > highestSortOrderSoFar) {
                    highestSortOrderSoFar = kvp.Value[i].sortingOrder;
                }
            }
            GameObject duplicatedSpriteObject = Instantiate(_originalSprites[i].gameObject, _originalSprites[i].transform.position, _originalSprites[i].transform.rotation, _originalSprites[i].transform);
            duplicatedSpriteObject.name = "Colorizer Sprite";
            duplicatedSpriteObject.transform.DestroyChildren();
            SpriteRenderer sr = duplicatedSpriteObject.GetComponent<SpriteRenderer>();
            sr.color = Color.clear;
            sr.material = material;
            sr.sortingOrder = highestSortOrderSoFar + 1;
            newSprites.Add(sr);
        }
        _colorizerSprites.Add(material.name, newSprites);
    }
    
}
