# Abraham.WPFWindowLayoutManager

![](https://img.shields.io/github/downloads/oliverabraham/WPFWindowLayoutManager/total) ![](https://img.shields.io/github/license/oliverabraham/WPFWindowLayoutManager) ![](https://img.shields.io/github/languages/count/oliverabraham/WPFWindowLayoutManager) ![GitHub Repo stars](https://img.shields.io/github/stars/oliverabraham/WPFWindowLayoutManager?label=repo%20stars) ![GitHub Repo stars](https://img.shields.io/github/stars/oliverabraham?label=user%20stars)

## OVERVIEW

This library is a Nuget package to easily save and restore Window positions.
The library can save attributes of WPF Windows and DataGrids (column widths, orders etc)


## LICENSE

Licensed under Apache licence.
https://www.apache.org/licenses/LICENSE-2.0


## Compatibility

The nuget package was build with DotNET 6.



## INSTALLATION

Install the Nuget package "Abraham.WpfWindowLayoutManager" into your application (from https://www.nuget.org).

Add the following code:



## Getting started

Add the Nuget package "Abraham.WpfWindowLayoutManager" to your project.

Add this using to your project:

```C#
using Abraham.WPFWindowLayoutManager;
```

Add this field to your project:

```C#
private WindowLayoutManager _layoutManager;
```

In the constructor of your MainWindow, add an initialization:

```C#
_layoutManager = new WindowLayoutManager(window:this, key:"MainWindow");
```

Add a Closing Handler to your MainWindow, and add the following code:

```C#
_layoutManager.Save();
```

That's all!


### Child windows

For child windows, add this code:
```C#
public partial class ChildWindow : Window
{
    public WindowLayoutManager LayoutManager { get; internal set; }

    public ChildWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LayoutManager.RestoreSizeAndPosition(this, "ChildWindow1");
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        LayoutManager.SaveSizeAndPosition(this, "ChildWindow1");
    }
}
```

When opening the child window, set the LayoutManager reference like this:

```C#
var childWindow = new ChildWindow();
childWindow.LayoutManager = _layoutManager;
childWindow.ShowDialog();
```

### Adding Logic for DataGrids

You'll find a lot more methods to save and restore:
- DataGrid control sizes
- DataGrid column widths
- DataGrid column ordering
- DataGrid column sort direction


### Restore the original states

You can call the Reset method to delete the file that contains the layout information.
Afterwards you should reinitialize your windows.



## Example

For an example refer to project "WindowLayoutManager_Demo". It demonstrates:
- how to save the main window's size and position when it is closed by the user,
- and restore it when he opens it up again.



## HOW TO INSTALL A NUGET PACKAGE
This is very simple:
- Start Visual Studio (with NuGet installed) 
- Right-click on your project's References and choose "Manage NuGet Packages..."
- Choose Online category from the left
- Enter the name of the nuget package to the top right search and hit enter
- Choose your package from search results and hit install
- Done!


or from NuGet Command-Line:

    Install-Package Abraham.WPFWindowLayoutManager


## AUTHOR

Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de

Please feel free to comment and suggest improvements!



## SOURCE CODE

The source code is hosted at:

https://github.com/OliverAbraham/WPFWindowLayoutManager

The Nuget Package is hosted at: 

https://www.nuget.org/packages/Abraham.WPFWindowLayoutManager




## SCREENSHOTS

# MAKE A DONATION !
If you find this application useful, buy me a coffee!
I would appreciate a small donation on https://www.buymeacoffee.com/oliverabraham

<a href="https://www.buymeacoffee.com/app/oliverabraham" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>
