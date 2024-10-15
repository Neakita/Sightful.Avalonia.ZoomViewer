using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Sightful.Avalonia.ZoomViewer;

public sealed class ZoomViewer : ContentControl
{
	public static readonly StyledProperty<double> ZoomProperty = ZoomContentPresenter.ZoomProperty.AddOwner<ZoomViewer>();

	public static readonly StyledProperty<bool> IsScrollBarsVisibleProperty =
		AvaloniaProperty.Register<ZoomViewer, bool>(nameof(IsScrollBarsVisible));

	public static readonly StyledProperty<Vector> OffsetProperty = ScrollViewer.OffsetProperty.AddOwner<ZoomViewer>();

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

	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		base.OnApplyTemplate(e);
		if (_presenter != null)
			_presenter.PointerPressed -= OnPresenterPointerPressed;
		_presenter = Presenter;
		if (_presenter != null)
			_presenter.PointerPressed += OnPresenterPointerPressed;
	}

	private ContentPresenter? _presenter;
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
		Offset += drag;
	}

	private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(_presenter);
		topLevel.PointerMoved -= OnPointerMoved;
		topLevel.PointerReleased -= OnPointerReleased;
	}
}