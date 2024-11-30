
# Leo's Templater

Do you ever want to create a simple C# script file, but Unity's default script comes with a lot of padding?

Leo's Templater provides an easy way to create different kinds of scripts, just the way you like them. Use the default scripts that come with the package or create your own, change the way you create scripts.

The templates will appear under `Create/Templates`.

## Features

- Generate scripts using templates
- Add Header and/or Footer to created scripts
- Built-in templates (C# Class, Interface, Scriptable Object)
- Support subfolder templates (Will appear in sub menus in the templates)

## Installation

Installing as GIT dependency via Package Manager
1. Open Package Manager (Window -> Package Manager)
2. Click `+` button on the upper left of the window, select "Add mpackage from git URL...'
3. Enter the following URL and click the `Add` button

   ```
   https://github.com/Mercury-Leo/LeosTemplater.git
   ```

## Getting Started

The templater comes with 3 basic templates.
To add custom templates:
1. Pick the templates location. I recommand saving the templates as part of source control, for exmaple under `Project/Assets/Templater/Templates`.
2. Go to `Edit/Preferences/Leo's Tools/Templates` and pick the folder for the templates.
3. Press `Regenerate Templates`

#### Creating the template
First create a new file inside the Templates folder, `{templateName}.cs.txt`
Edit the file to your specification.

This is a basic script template:
```
namespace #NAMESPACE# 
{
	public class #SCRIPTNAME# 
	{
	    #NOTRIM#
	}
}
```
- '#NAMESPACE#' will automatically try and assign the correct namespace to the generated script.
- '#SCRIPTNAME#' will assign the script name when generated.
- '#NOTRIM#' prevents an empty space from being deleted.

## Adding Header and Footer
To change the Header and Footer of scripts head to `Edit/Project Settings/Leo's Tools/Templates`.

Edit the Header and Footer to your liking, both will appear commented out at the top and bottom of the genereated script respectfully.

## Examples

Scriptable object
```
using UnityEngine;

namespace #NAMESPACE# 
{
    [CreateAssetMenu(fileName = "new#SCRIPTNAME#", menuName = "#SCRIPTNAME#")]
    public class #SCRIPTNAME# : ScriptableObject 
    {
        #NOTRIM#
    }
}

```

