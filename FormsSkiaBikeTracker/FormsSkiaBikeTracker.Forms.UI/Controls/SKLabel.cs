﻿// **********************************************************************
// 
//   SKLabel.xaml.cs
//   
//   This file is subject to the terms and conditions defined in
//   file 'LICENSE.txt', which is part of this source code package.
//   
//   Copyright (c) 2017, Le rond-point
// 
// ***********************************************************************

using System;
using System.IO;
using LRPLib.Services.Resources;
using LRPLib.Views.XForms;
using MvvmCross.Platform;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace FormsSkiaBikeTracker.Forms.UI.Controls
{
    public class SKLabel : DrawableView
    {
        public static readonly BindableProperty FontResourcePathProperty = BindableProperty.Create(nameof(FontResourcePath), typeof(string), typeof(SKLabel), string.Empty, BindingMode.OneWay, null, FontResourcePathPropertyChanged);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SKLabel), string.Empty, BindingMode.OneWay, null, ResizePropertyChanged);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SKLabel), Color.Black, BindingMode.OneWay, null, InvalidatePropertyChanged);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SKLabel), 14.0, BindingMode.OneWay, null, ResizePropertyChanged);
        private IResourceLocator _resourceLocator;

        public string FontResourcePath
        {
            get { return (string)GetValue(FontResourcePathProperty); }
            set { SetValue(FontResourcePathProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        private SizeRequest? _LastSize { get; set; }
        private SKPaint _Paint { get; }
        private SKTypeface _Typeface { get; set; }

        public SKLabel()
        {
            _Paint = new SKPaint();
            _resourceLocator = Mvx.Resolve<IResourceLocator>();

            HasTransparency = true;
        }

        ~SKLabel()
        {
            _Typeface?.Dispose();
            _Paint?.Dispose();
        }

        private static void FontResourcePathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKLabel view = bindable as SKLabel;

            view.RefreshTypeface();
            ResizePropertyChanged(bindable, oldValue, newValue);
        }

        private void RefreshTypeface()
        {
            string resourcePath = _resourceLocator.GetResourcePath("Fonts", FontResourcePath);
            Stream resStream = _resourceLocator.ResourcesAssembly.GetManifestResourceStream(resourcePath);

            _Typeface?.Dispose();

            if (resStream != null)
            {
                using (resStream)
                {
                    _Typeface = SKTypeface.FromStream(resStream);
                }
            }
            else
            {
                _Typeface = null;
            }
        }

        private static void InvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKLabel view = bindable as SKLabel;

            view.RefreshPaint();
            view.Invalidate();
        }
                
        private static void ResizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SKLabel view = bindable as SKLabel;

            view.RefreshPaint();
            view.InvalidateMeasure();
            view.Invalidate();
        }

        protected override void InvalidateMeasure()
        {
            _LastSize = null;
            base.InvalidateMeasure();
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (_LastSize == null && HasSomethingToDraw())
            {
                SKRect textBounds = new SKRect();
                Rectangle requestRect;

                _Paint.MeasureText(Text, ref textBounds);
                requestRect = textBounds.ToFormsRect();

                requestRect.Height += _Paint.FontMetrics.Descent;
                requestRect.Width = Math.Ceiling(Math.Min(requestRect.Width, widthConstraint));
                requestRect.Height = Math.Ceiling(Math.Min(requestRect.Height, heightConstraint));

                _LastSize = new SizeRequest(requestRect.Size);
            }

            return _LastSize.GetValueOrDefault(new SizeRequest());
        }

        protected override void Paint(SKCanvas canvas)
        {
            if (HasSomethingToDraw())
            {
                canvas.DrawText(Text, 0, (float)Height - (_Paint.FontMetrics.Descent * 0.5f), _Paint);
            }
        }

        private bool HasSomethingToDraw()
        {
            return _Typeface != null && !string.IsNullOrEmpty(Text) && TextColor != Color.Transparent && FontSize > 0;
        }

        private void RefreshPaint()
        {
            _Paint.Typeface = _Typeface;
            _Paint.Color = TextColor.ToSKColor();
            _Paint.TextSize = (float)FontSize;
            _Paint.IsAntialias = true;
        }
    }
}
