using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Sightful.Avalonia.ZoomViewer.Demo.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private static double CalculateZoom(Size contentSize, Size viewSize)
	{
		var ratio = viewSize / contentSize;
		return Math.Min(ratio.X, ratio.Y);
	}
}