# JollyConsole

# Description:

JollyConsole is an application that should provide better user experience when working with Windows Command Prompt. It wraps around Windows Command Prompt and adds new features. These new features can be used through graphical user interface. Application consists of graphical proxy to Windows Command Prompt and the panel with GUI components that can be used to interact with that proxy. Some of the features provided by this application are:
  - Creating macroes for chaining Windows Command Prompt commands.
  - Creating individual commands within macroes
  - Executing of created macroes
  - Exporting these macroes as templates in JSON format
  - Importing these templates back into application
  - Extracting macroes from already typed-in Windows Command Prompt commands
  - Delegating Windows Command Prompt commands to its proxy and displaying its response within application

# Requirements:
	- Operating system: Windows 10

# Other open source projects used:
	- Newtonsoft Json.NET - for serializing and deserializing JSON - https://github.com/JamesNK/Newtonsoft.Json
  
# Future work
  - Disabe/enable commands within macroes
  - Allow arbitrary number of macroes to be created
  - Allow arbitrary number of commands within macroes to be created
  - Allow macroes and their commands to be draggable enabling position switching
  - Create support for variables in commands
