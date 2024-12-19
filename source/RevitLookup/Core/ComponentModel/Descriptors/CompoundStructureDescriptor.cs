// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Reflection;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class CompoundStructureDescriptor(CompoundStructure compoundStructure) : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CompoundStructure.CanLayerBeStructuralMaterial) => ResolveCanLayerBeStructuralMaterial,
            nameof(CompoundStructure.CanLayerBeVariable) => ResolveCanLayerBeVariable,
            nameof(CompoundStructure.CanLayerWidthBeNonZero) => ResolveCanLayerWidthBeNonZero,
            nameof(CompoundStructure.GetAdjacentRegions) => ResolveGetAdjacentRegions,
            nameof(CompoundStructure.GetCoreBoundaryLayerIndex) => ResolveGetCoreBoundaryLayerIndex,
            nameof(CompoundStructure.GetDeckEmbeddingType) => ResolveGetDeckEmbeddingType,
            nameof(CompoundStructure.GetDeckProfileId) => ResolveGetDeckProfileId,
            nameof(CompoundStructure.GetLayerAssociatedToRegion) => ResolveGetLayerAssociatedToRegion,
            nameof(CompoundStructure.GetLayerFunction) => ResolveGetLayerFunction,
            nameof(CompoundStructure.GetLayerWidth) => ResolveGetLayerWidth,
            nameof(CompoundStructure.GetMaterialId) => ResolveGetMaterialId,
            nameof(CompoundStructure.GetNumberOfShellLayers) => ResolveGetNumberOfShellLayers,
            nameof(CompoundStructure.GetOffsetForLocationLine) => ResolveGetOffsetForLocationLine,
            nameof(CompoundStructure.GetPreviousNonZeroLayerIndex) => ResolveGetPreviousNonZeroLayerIndex,
            nameof(CompoundStructure.GetRegionEnvelope) => ResolveGetRegionEnvelope,
            nameof(CompoundStructure.GetRegionsAssociatedToLayer) => ResolveGetRegionsAssociatedToLayer,
            nameof(CompoundStructure.GetSegmentCoordinate) => ResolveGetSegmentCoordinate,
            nameof(CompoundStructure.GetSegmentOrientation) => ResolveGetSegmentOrientation,
            _ => null
        };

        IVariants ResolveCanLayerBeStructuralMaterial()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerBeStructuralMaterial(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanLayerBeVariable()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerBeVariable(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveCanLayerWidthBeNonZero()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<bool>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.CanLayerWidthBeNonZero(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            return variants;
        }
        
        IVariants ResolveGetAdjacentRegions()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = new Variants<IList<int>>(regionsCount);
            
            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetAdjacentRegions(i);
                variants.Add(result, $"Region" +
                                     $" {i}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetCoreBoundaryLayerIndex()
        {
            var values = Enum.GetValues(typeof(ShellLayerType));
            var variants = new Variants<int>(values.Length);
            
            foreach (ShellLayerType value in values)
            {
                var result = compoundStructure.GetCoreBoundaryLayerIndex(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetDeckEmbeddingType()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<StructDeckEmbeddingType>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetDeckEmbeddingType(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetLayerAssociatedToRegion()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = new Variants<int>(regionsCount);
            
            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetLayerAssociatedToRegion(i);
                variants.Add(result, $"Region {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetLayerFunction()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<MaterialFunctionAssignment>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetLayerFunction(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetDeckProfileId()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<ElementId>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetDeckProfileId(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetLayerWidth()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<double>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetLayerWidth(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetMaterialId()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<ElementId>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetMaterialId(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetNumberOfShellLayers()
        {
            var values = Enum.GetValues(typeof(ShellLayerType));
            var variants = new Variants<int>(values.Length);
            
            foreach (ShellLayerType value in values)
            {
                var result = compoundStructure.GetNumberOfShellLayers(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetOffsetForLocationLine()
        {
            var values = Enum.GetValues(typeof(WallLocationLine));
            var variants = new Variants<double>(values.Length);
            
            foreach (WallLocationLine value in values)
            {
                var result = compoundStructure.GetOffsetForLocationLine(value);
                variants.Add(result, $"{value.ToString()}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetPreviousNonZeroLayerIndex()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<int>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetPreviousNonZeroLayerIndex(i);
                variants.Add(result, $"Layer {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetRegionEnvelope()
        {
            var regionsCount = compoundStructure.GetRegionIds().Count;
            var variants = new Variants<BoundingBoxUV>(regionsCount);
            
            for (var i = 0; i < regionsCount; i++)
            {
                var result = compoundStructure.GetRegionEnvelope(i);
                variants.Add(result, $"Region {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetRegionsAssociatedToLayer()
        {
            var layerCount = compoundStructure.LayerCount;
            var variants = new Variants<IList<int>>(layerCount);
            for (var i = 0; i < layerCount; i++)
            {
                var result = compoundStructure.GetRegionsAssociatedToLayer(i);
                variants.Add(result, $"Layer {i}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetSegmentCoordinate()
        {
            var segmentCount = compoundStructure.GetSegmentIds().Count;
            var variants = new Variants<double>(segmentCount);
            for (var i = 0; i < segmentCount; i++)
            {
                var result = compoundStructure.GetSegmentCoordinate(i);
                variants.Add(result, $"Segment {i}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveGetSegmentOrientation()
        {
            var segmentCount = compoundStructure.GetSegmentIds().Count;
            var variants = new Variants<RectangularGridSegmentOrientation>(segmentCount);
            for (var i = 0; i < segmentCount; i++)
            {
                var result = compoundStructure.GetSegmentOrientation(i);
                variants.Add(result, $"Segment {i}: {result}");
            }
            
            return variants;
        }
    }
}