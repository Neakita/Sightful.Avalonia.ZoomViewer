using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Sightful.Avalonia.ZoomViewer;

[TemplatePart("PART_ScrollViewer", typeof(ContentPresenter))]
public sealed class ZoomViewer : ContentControl
{
	public static readonly StyledProperty<double> ZoomProperty = ZoomContentPresenter.ZoomProperty.AddOwner<ZoomViewer>(new StyledPropertyMetadata<double>(coerce: CoerceZoom));

	public static readonly StyledProperty<bool> IsScrollBarsVisibleProperty =
		AvaloniaProperty.Register<ZoomViewer, bool>(nameof(IsScrollBarsVisible));

	public static readonly StyledProperty<Vector> OffsetProperty = ScrollViewer.OffsetProperty.AddOwner<ZoomViewer>(new StyledPropertyMetadata<Vector>(coerce: CoerceOffset));

	public static readonly DirectProperty<ZoomViewer, double> MinimumZoomProperty =
		AvaloniaProperty.RegisterDirect<ZoomViewer, double>(nameof(MinimumZoom), zoomViewer => zoomViewer.MinimumZoom);

	private static double CoerceZoom(AvaloniaObject sender, double value)
	{
		return Math.Max(value, sender.GetValue(MinimumZoomProperty));
	}

	private static Vector CoerceOffset(AvaloniaObject sender, Vector value)
	{
		var scrollViewer = ((ZoomViewer)sender)._scrollViewer;
		var extent = scrollViewer.Extent;
		var viewport = scrollViewer.Viewport;
		var maxX = Math.Max(extent.Width - viewport.Width, 0);
		var maxY = Math.Max(extent.Height - viewport.Height, 0);
		return new Vector(Clamp(value.X, 0, maxX), Clamp(value.Y, 0, maxY));
	}

	private static double Clamp(double value, double min, double max)
	{
		return (value < min) ? min : (value > max) ? max : value;
	}

	public double Zoom
	{
		get => GetValue(ZoomProperty);
		set => SetValue(ZoomProperty, value);
	}

	public bool IsScrollBarsVisible
	{
		get => GetValue(IsScrollBarsVisibleProperty);
		set => SetValue(IsScrollBarsVisibleProperty, value);
	}

	public Vector Offset
	{
		get => GetValue(OffsetProperty);
		set => SetValue(OffsetProperty, value);
	}
	
	public double MinimumZoom { get; private set; }

	public void ZoomToFit()
	{
		var ratio = Bounds.Size / Presenter.DesiredSize;
		SetCurrentValue(ZoomProperty, Math.Min(ratio.X, ratio.Y));
	}

	public void ZoomToActualSize()
	{
		SetCurrentValue(ZoomProperty, 1);
	}

	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		base.OnApplyTemplate(e);
		if (_presenter != null)
		{
			_presenter.PointerPressed -= OnPresenterPointerPressed;
			_presenter.SizeChanged -= OnPresenterSizeChanged;
		}
		_presenter = Presenter;
		if (_presenter != null)
		{
			_presenter.PointerPressed += OnPresenterPointerPressed;
			_presenter.SizeChanged += OnPresenterSizeChanged;
		}
		_scrollViewer = e.NameScope.Find<ScrollViewer>("PART_ScrollViewer");
	}

	private ContentPresenter? _presenter;
	private ScrollViewer? _scrollViewer;
	private Point _previousPanPosition;

	private void OnPresenterPointerPressed(object sender, PointerPressedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(_presenter);
		topLevel.PointerMoved += OnPointerMoved;
		topLevel.PointerReleased += OnPointerReleased;
		_previousPanPosition = e.GetPosition(this);
	}

	private void OnPointerMoved(object sender, PointerEventArgs e)
	{
		var currentPosition = e.GetPosition(this);
		var drag = _previousPanPosition - currentPosition;
		_previousPanPosition = currentPosition;
		SetCurrentValue(OffsetProperty, Offset + drag);
	}

	private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(_presenter);
		topLevel.PointerMoved -= OnPointerMoved;
		topLevel.PointerReleased -= OnPointerReleased;
	}

	private void UpdateMinimumZoom()
	{
		var oldValue = MinimumZoom;
		var newValue = ComputeMinimumZoom();
		MinimumZoom = newValue;
		RaisePropertyChanged(MinimumZoomProperty, oldValue, newValue);
		SetCurrentValue(ZoomProperty, CoerceZoom(this, Zoom));
	}

	private double ComputeMinimumZoom()
	{
		var ratio = Bounds.Size / Presenter.DesiredSize;
		var result = Math.Min(ratio.X, ratio.Y);
		if (result > 1)
			result = 1;
		return result;
	}

	protected override void OnSizeChanged(SizeChangedEventArgs e)
	{
		base.OnSizeChanged(e);
		UpdateMinimumZoom();
	}

	private void OnPresenterSizeChanged(object sender, SizeChangedEventArgs e)
	{
		UpdateMinimumZoom();
	}
}