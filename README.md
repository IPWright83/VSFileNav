# VSFileNav

This is a simple extention to allow rapid navigation and finding of files within a Solution using a number of different search patterns.

For more information please visit https://github.com/IPWright83/VSFileNav

#### New Features
 - Multi montior support
 - Multi search

### Features

VSFileNav supports a number of features including:

 - **Fast** - It can search thousands of files within a Solution with ease
 - **Name Search** - Searches based upon the name of a file by simply using mixed case search string.
 - **Wildcard Search** - Use standard windows wildcards : `?` (any character) or `*` (number of any characters).
 - **Camel Case Search** - Use uppercase strings to do Camel case searching on file names.
 - **Mutli Search** - just put a space between multiple search terms.
 - **Highlighting** - Filtering as you type with match highlighting.
 - **Explore To** - Immediately explorer to the file on the file system
 - **Themes** - VSFileNav attempts to automatically detect your Visual Studio theme and colour itself appropriately
 - **Multi Montior** - Multi montor support has been added, the search dialog will remember both size and position
 
### Getting Started

The first thing you'll want to do when using VSFileNav is to set up a key binding. By default it uses `ctrl+alt+f` if it's avalaible. You can change the binding in the Keyboard settings. You'll want to bind the `Edit.QuickFindFile` command as shown below:

![image](https://cloud.githubusercontent.com/assets/1374775/18972646/f131cbd4-8691-11e6-870c-2ffe5b969fe8.png)

If you prefer using the menu system you can find it here under **Edit** > **Find and Replace** > **Quick Find File**

![image](https://cloud.githubusercontent.com/assets/1374775/18972716/3106afb8-8692-11e6-9031-700099547fc9.png)

### Searching

To search simply start typing, by typing a regular string the list of files will be automatically filtered. This feature is called the **Name Search**. 

![image](https://cloud.githubusercontent.com/assets/1374775/18972767/5fc73368-8692-11e6-8279-05bd9c8ce060.png)

If you're not 100% sure on the name of the file, then you can provide multiple search patterns by separating them with a space:

![image](https://cloud.githubusercontent.com/assets/1374775/18972814/93a74880-8692-11e6-9276-6ac26cb06a1e.png)

If you've pretty familiar with the name of the file, then often the quickest way is to type the camel case letters of the name like below. Note that VSFileNav tries to be helpful and highlight missed camel cased characters in blue, always providing the best match at the top of the list:

![image](https://cloud.githubusercontent.com/assets/1374775/18972875/c7dc2d46-8692-11e6-8e07-d79a643850e1.png)

Finally you can provide wildcards `?` or `*` for searching. `?` matches just a single character, while `*` matches any character:

![image](https://cloud.githubusercontent.com/assets/1374775/18972923/f32a8fe2-8692-11e6-8f01-d6f82f4bd260.png)


