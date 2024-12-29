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

using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using RevitLookup.Utils;
using Color = System.Windows.Media.Color;
using ColorFormatUtils = RevitLookup.UI.Framework.Utils.ColorFormatUtils;

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class ColorMediaDescriptor : Descriptor, IDescriptorExtension
{
    private readonly Color _color;

    public ColorMediaDescriptor(Color color)
    {
        _color = color;
        Name = $"RGB: {color.R} {color.B} {color.B}";
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("HEX", () => Variants.Value(ColorRepresentationUtils.ColorToHex(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HEX int", () => Variants.Value(ColorRepresentationUtils.ColorToHexInteger(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("RGB", () => Variants.Value(ColorRepresentationUtils.ColorToRgb(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HSL", () => Variants.Value(ColorRepresentationUtils.ColorToHsl(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HSV", () => Variants.Value(ColorRepresentationUtils.ColorToHsv(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("CMYK", () => Variants.Value(ColorRepresentationUtils.ColorToCmyk(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HSB", () => Variants.Value(ColorRepresentationUtils.ColorToHsb(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HSI", () => Variants.Value(ColorRepresentationUtils.ColorToHsi(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("HWB", () => Variants.Value(ColorRepresentationUtils.ColorToHwb(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("NCol", () => Variants.Value(ColorRepresentationUtils.ColorToNCol(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("CIELAB", () => Variants.Value(ColorRepresentationUtils.ColorToCielab(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("CIEXYZ", () => Variants.Value(ColorRepresentationUtils.ColorToCieXyz(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("VEC4", () => Variants.Value(ColorRepresentationUtils.ColorToFloat(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("Decimal", () => Variants.Value(ColorRepresentationUtils.ColorToDecimal(ColorFormatUtils.GetDrawingColor(_color))));
        manager.Register("Name", () => Variants.Value(ColorRepresentationUtils.GetColorName(ColorFormatUtils.GetDrawingColor(_color))));
    }
}