using System;
using System.Windows.Input;

namespace Sightful.Avalonia.ZoomViewer;

internal sealed class RelayCommand : ICommand
{
	public event EventHandler? CanExecuteChanged;

	public RelayCommand(Func<object?, bool> canExecute, Action<object?> execute)
	{
		_canExecute = canExecute;
		_execute = execute;
	}

	public bool CanExecute(object? parameter)
	{
		return _canExecute(parameter);
	}

	public void Execute(object? parameter)
	{
		_execute(parameter);
	}

	public void NotifyCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	private Func<object?, bool> _canExecute;
	private Action<object?> _execute;
}