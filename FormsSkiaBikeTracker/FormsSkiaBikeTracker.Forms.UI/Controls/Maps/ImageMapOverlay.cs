﻿// **********************************************************************
// 
//   ImageMapOverlay.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Sylvain Gravel
// 
// ***********************************************************************

using LRPLib.Views.XForms.Extensions;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FormsSkiaBikeTracker.Forms.UI.Controls.Maps
{
    public class ImageMapOverlay : DrawableMapOverlay
    {
        public ImageMapOverlay()
        {
            GpsBounds = new MapSpan(new Position(0, 0), 1, 1);
        }

        public override void DrawOnMap(SKMapCanvas canvas, MapSpan canvasMapRect)
        {
            SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.StrokeWidth = 1;
            paint.Color = Color.Aqua.ToSKColor();

            if (canvasMapRect.Center.Latitude > 0)
            {
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                paint);
            }
            else
            {
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                paint);
            }

            paint.StrokeWidth = 10;
            paint.Color = Color.Red.ToSKColor();
            canvas.DrawLine(new Position(GpsBounds.Center.Latitude - GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude),
                            new Position(GpsBounds.Center.Latitude + GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude + GpsBounds.LongitudeDegrees),
                            paint);
            canvas.DrawLine(new Position(GpsBounds.Center.Latitude + GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude + GpsBounds.LongitudeDegrees),
                            new Position(GpsBounds.Center.Latitude + GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude - GpsBounds.LongitudeDegrees),
                            paint);
            canvas.DrawLine(new Position(GpsBounds.Center.Latitude + GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude - GpsBounds.LongitudeDegrees),
                            new Position(GpsBounds.Center.Latitude - GpsBounds.LatitudeDegrees, GpsBounds.Center.Longitude),
                            paint);
        }
    }
}
