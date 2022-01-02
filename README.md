# Abraham.WpfWindowLayoutManager

Oliver Abraham
mail@oliver-abraham.de


## Abstract

This library is a Nuget package to easily save and restore Window positions.
The library can save all kinds of WPF Windows and Controls: 
Windows, GridViews, GridView column widths


## License

Licensed under GPL v3 license.
https://www.gnu.org/licenses/gpl-3.0.en.html


## Compatibility

The nuget package was build with DotNET 6.


## Example

For an example refer to project "WindowLayoutManager_Demo". 
It demonstrates how to save the main window's size and position when it is closed by the user,
and restore it when he opens it up again.


## Getting started

Add the Nuget package "Abraham.WpfWindowLayoutManager" to your project.

Add a field to your project:

		private WindowLayoutManager _layoutManager;

In the constructor of your MainWindow, add an initialization_

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
