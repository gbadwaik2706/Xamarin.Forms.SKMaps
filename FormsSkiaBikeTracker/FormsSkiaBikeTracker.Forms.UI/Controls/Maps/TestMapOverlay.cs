﻿// **********************************************************************
// 
//   TestMapOverlay.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Sylvain Gravel
// 
// ***********************************************************************

using FormsSkiaBikeTracker.Shared.Models.Maps;
using LRPLib.Services.Resources;
using LRPLib.Views.XForms.Extensions;
using MvvmCross.Platform;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FormsSkiaBikeTracker.Forms.UI.Controls.Maps
{
    public class TestMapOverlay : DrawableMapOverlay
    {
        private Size strokeWidthArea;
        private MapSpan baseBounds;

        public TestMapOverlay()
        {
            this.strokeWidthArea = SKMapCanvas.PixelsToMaximumMapSizeAtZoom(new Size(50, 50), SKMapCanvas.MaxZoomScale);

            baseBounds = new MapSpan(new Position(37, -122), 1, 1);
            GpsBounds = new MapSpan(baseBounds.Center,
                                    baseBounds.LatitudeDegrees + strokeWidthArea.Height,
                                    baseBounds.LongitudeDegrees + strokeWidthArea.Width);
        }

        public override void DrawOnMap(SKMapCanvas canvas, SKMapSpan canvasMapRect, double pixelScale)
        {
            SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.StrokeWidth = 1;
            paint.Color = Color.Fuchsia.ToSKColor();

            if (canvasMapRect.Center.Latitude > 0)
            {
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                paint);
            }
            else
            {
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude + canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                paint);
                canvas.DrawLine(new Position(canvasMapRect.Center.Latitude + canvasMapRect.LatitudeDegrees,
                                             canvasMapRect.Center.Longitude - canvasMapRect.LongitudeDegrees),
                                new Position(canvasMapRect.Center.Latitude - canvasMapRect.LatitudeDegrees, canvasMapRect.Center.Longitude),
                                paint);
            }

            paint.Style = SKPaintStyle.Stroke;
            paint.StrokeWidth = 10;
            paint.StrokeCap = SKStrokeCap.Round;
            paint.StrokeJoin = SKStrokeJoin.Round;
            paint.Color = Color.Red.ToSKColor();
            SKMapPath zonePath = canvas.GetEmptyMapPath();

            zonePath.MoveTo((float)(baseBounds.Center.Latitude - baseBounds.LatitudeDegrees), (float)baseBounds.Center.Longitude);
            zonePath.LineTo((float)(baseBounds.Center.Latitude + baseBounds.LatitudeDegrees), (float)(baseBounds.Center.Longitude + baseBounds.LongitudeDegrees));
            zonePath.LineTo((float)(baseBounds.Center.Latitude + baseBounds.LatitudeDegrees), (float)(baseBounds.Center.Longitude - baseBounds.LongitudeDegrees));
            zonePath.Close();

            canvas.DrawPath(zonePath, paint);

            paint.Color = Color.Green.MultiplyAlpha(0.5).ToSKColor();
            Size currentScaleStrokeArea = SKMapCanvas.PixelsToMapSize(new Size(5, 5), baseBounds.Center, pixelScale);
            MapSpan insetBounds = new MapSpan(baseBounds.Center,
                                              baseBounds.LatitudeDegrees - currentScaleStrokeArea.Height,
                                              baseBounds.LongitudeDegrees - currentScaleStrokeArea.Width);
            canvas.DrawRect(insetBounds, paint);

            paint.StrokeWidth = 1;
            paint.Color = Color.Black.ToSKColor();
            canvas.DrawRect(baseBounds, paint);

            paint.Color = Color.Black.ToSKColor();
            canvas.DrawRect(GpsBounds, paint);

            IResourceLocator resLocator = Mvx.Resolve<IResourceLocator>();
            string resPath = resLocator.GetResourcePath(ResourceKeys.ImagesKey, "symbol_logo.svg");
            SKSvg logoSvg = new SKSvg();
            logoSvg.Load(resLocator.ResourcesAssembly.GetManifestResourceStream(resPath));
            canvas.DrawPicture(logoSvg.Picture, baseBounds.Center, new Size(100, 100));
        }
    }
}