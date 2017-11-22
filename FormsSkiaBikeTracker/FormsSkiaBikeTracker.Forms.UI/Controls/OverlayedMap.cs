﻿// **********************************************************************
// 
//   OverlayedMap.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Sylvain Gravel
// 
// ***********************************************************************

using System.Collections.ObjectModel;
using FormsSkiaBikeTracker.Forms.UI.Controls.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FormsSkiaBikeTracker.Forms.UI.Controls
{
    public class OverlayedMap : Map
    {
        public static readonly BindableProperty MapOverlaysProperty = BindableProperty.Create(nameof(MapOverlays),
                                                                                              typeof(ObservableCollection<DrawableMapOverlay>),
                                                                                              typeof(OverlayedMap),
                                                                                              new ObservableCollection<DrawableMapOverlay>());

        public ObservableCollection<DrawableMapOverlay> MapOverlays => (ObservableCollection<DrawableMapOverlay>)GetValue(MapOverlaysProperty);

        public OverlayedMap()
        {
        }
    }
}
