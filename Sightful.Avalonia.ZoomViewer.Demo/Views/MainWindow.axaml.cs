using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Sightful.Avalonia.ZoomViewer.Demo.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void Button_OnClick(object? sender, RoutedEventArgs e)
	{
		ZoomViewer.ZoomToFit();
	}

	private void Button2_OnClick(object? sender, RoutedEventArgs e)
	{
		ZoomViewer.ZoomToActualSize();
	}
}