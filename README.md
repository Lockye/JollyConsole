# JollyConsole

# Description:

JollyConsole is an application that should provide better user experience when working with Windows Command Prompt. It wraps around Windows Command Prompt and adds new features. These new features can be used through graphical user interface. Application consists of graphical proxy to Windows Command Prompt and the panel with GUI components that can be used to interact with that proxy. Some of the features provided by this application are:
  - Writing Windows Command Prompt commands and executing them from the application and displaying the execution response
  - Creating macros for chaining Windows Command Prompt commands
  - Creating individual commands within macro chains
  - Executing of created macros
  - Exporting these macros as templates in JSON format
  - Importing these templates back into application
  - Extracting macros from already typed-in Windows Command Prompt commands

# Requirements:
	- Operating system: Windows 10

# Other open source projects used:
	- Newtonsoft Json.NET - for serializing and deserializing JSON - https://github.com/JamesNK/Newtonsoft.Json
  
# Future work
  - Disabe/enable commands within macros
  - Allow custom number of macros to be created
  - Allow custom number of commands within macro chains to be created
  - Allow macros and their commands to be draggable enabling position switching
  - Create support for variables in commands
