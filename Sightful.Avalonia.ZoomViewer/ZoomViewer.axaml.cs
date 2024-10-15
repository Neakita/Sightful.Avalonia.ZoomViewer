using Avalonia;
using Avalonia.Controls;

namespace Sightful.Avalonia.ZoomViewer;

public sealed class ZoomViewer : ContentControl
{
	public static readonly StyledProperty<double> ZoomProperty = ZoomContentPresenter.ZoomProperty.AddOwner<ZoomViewer>();

	public static readonly StyledProperty<bool> IsScrollBarsVisibleProperty =
		AvaloniaProperty.Register<ZoomViewer, bool>(nameof(IsScrollBarsVisible));

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
}