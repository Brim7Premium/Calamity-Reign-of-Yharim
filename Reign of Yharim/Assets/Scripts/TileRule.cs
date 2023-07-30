using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Rule Sibling Tile", menuName = "2D/Tiles/Rule Sibling Tile")]
public class TileRule : RuleTile<TileRule.Neighbor> 
{
    public List<TileBase> sibings = new List<TileBase>();

    public class Neighbor : RuleTile.TilingRule.Neighbor 
    {

    }

    public override bool RuleMatch(int neighbor, TileBase tile) 
    {
        switch (neighbor) 
        {
            case Neighbor.This: return tile == this || sibings.Contains(tile);
            case Neighbor.NotThis: return tile != this && !sibings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}