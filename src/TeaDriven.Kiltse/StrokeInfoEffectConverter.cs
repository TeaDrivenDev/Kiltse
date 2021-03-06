﻿using System;
using System.Globalization;

namespace TeaDriven.Kiltse
{
    public class StrokeInfoEffectConverter : ConverterMarkupExtension<StrokeInfoEffectConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var strokeInfoSelector = values[1] as StrokeInfoSelector;

            return strokeInfoSelector.GetStrokeInfo(values[0] as RingItem).Effect;
        }
    }
}