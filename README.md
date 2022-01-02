# Abraham.WpfWindowLayoutManager

Oliver Abraham
mail@oliver-abraham.de


## Abstract

This library is a Nuget package to easily save and restore Window positions.
The library can save attributes of WPF Windows and DataGrids (column widths, orders etc)


## License

Licensed under GPL v3 license.
https://www.gnu.org/licenses/gpl-3.0.en.html


## Compatibility

The nuget package was build with DotNET 6.


## Example

For an example refer to project "WindowLayoutManager_Demo". It demonstrates:
- how to save the main window's size and position when it is closed by the user,
- and restore it when he opens it up again.


## Getting started

Add the Nuget package "Abraham.WpfWindowLayoutManager" to your project.

Add a field to your project:

		private WindowLayoutManager _layoutManager;

In the constructor of your MainWindow, add an initialization:

		_layoutManager = new WindowLayoutManager(window:this, key:"MainWindow");

Add a Closing Handler to your MainWindow, and add the following code:

		_layoutManager.Save();

That's all!


### Adding Logic for DataGrids

You'll find a lot more methods to save and restore:
- DataGrid control sizes
- DataGrid column widths
- DataGrid column ordering
- DataGrid column sort direction


### Restore the original states

You can call the Reset method to delete the file that contains the layout information.
Afterwards you should reinitialize your windows.

