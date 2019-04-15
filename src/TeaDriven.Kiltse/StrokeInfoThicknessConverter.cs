﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TeaDriven.Kiltse
{
    public class StrokeInfoThicknessConverter : ConverterMarkupExtension<StrokeInfoThicknessConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var strokeInfoSelector = values[1] as StrokeInfoSelector;

            return strokeInfoSelector.GetStrokeInfo(values[0]).StrokeThickness;
        }
    }
}