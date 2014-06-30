codescope

See your code. 
See your code evolve.

Features
- see your code
	object oriented code: C++, C#, ObjectiveC, 
		what about C?
	classes, namespaces
	internal class structure
	collaborations/links/dependencies/usage patterns between classes
- see code evolution
	big picture (#of classes, class size, links between classes, LOC, method lengths)
	animate changes
		links, decoupling
		public interface growth
		internal implementation growth
		merging/splitting classes
		moving methods around
		visibility changes (private/public)

Components
	code analysis: take a project, process it, create project database
	visualization: take a project database, show it
	scm checkout: get a project from a source control system (git, tfs)
	history: get project commit history, process each of them

	dependencies
	history -> scm checkout -> code analysis -> visualization

	tool: auto-checkin: to keep as much history as possible, automatically check in code when it compiles (or all unit tests run cleanly)

Project database
	- what should be in the project database?
	- things we want to show:
		classes
			public interface
				methods and signatures
				# of public methods and properties
			internal implementation
				# and size of private/protected methods
				# of private variables
				warning for protected variables/properties
			links to other classes

Initial features
- code analysis: read a project, create project database
- visualization: read project database, show project
- project database:
	classes 
		name
		namespace
		LOC
		(#of and size of (LOC)) public methods, fields and properties
		(#of and size of (LOC)) private methods, fields and properties
	dependencies
		classes used by this class
		classes using this class
			which parts of the public interface is used by which classes
			*IDEA: detect unused public interface*


Engineering tasks
- environment
- project database: contains only classes (class name, namespace)
- process a project: extract all classes
- show a project: show each class as a rectangle, grouped by namespaces?
- figure out how project database will look (schema) and stored (XML? JSON? database?)
	figure out the simplest format
		CSV?, JSON?

Components Redux
- project parser is platform specific:
	one for C# on windows
	one for C++/objectiveC on MacOS
	Q: how about C++ on Windows? libclang should work, except the VS project format. Maybe we can use roslyn to get the documents and libclang to parse them?
- project database is the same (so you can parse your project on Mac and visualize on Windows)
- visualizer will be the same probably (OpenGL on both/all platforms) 
- scm connectors: 
	- scm specific (git, tfs)
	- tfs only on windows
	- git on Windows and MacOS
		so it's gonna be the same codebase?
- scm history: should be the same codebase, using scm connectors


Fun stuff
- somehow detect abstraction level in methods and warn if multiple levels are detected
	- look at where all the calls are going: the classes/methods called to should be few. and if they are more than one, they should be used mostly together (as in other usage sites should follow the same pattern)
- find clusters of methods using the same subset of variables/properties
- find patterns in class usage:
	- clusters of classes using disjunct subsets of the public interface (wrong abstractions: multiple responsibilities? multiple levels of abstraction exposed)
	- different methods using different dependencies (multiple responsibilities (if methods are public). However: facade's should do this )
- see interface usage. DI-wise...
- detecting changes between commits. Semantically relevant changes. Especially if the diff is large.
	operations: delete, create, modify (special: rename)
	entities: class, namespace, method, property
- try to fix design bugs:
	- based on usage patterns move methods/properties around, change their visibility to make better abstractions
		- what is better?
		- smaller classes, fewer dependencies
- use codescope on C code
	global namespace
	modules are source files probably
	global variables
	structs and typedefs are like classes
		BTW: you'll have typedefs, structs, global variables in C++ and ObjC too
- how about handling java? Maybe there is a java parse like roslyn and libclang
- javascript? ruby?
- handle asp.net mvc projects
- deal with UI components (ObjC, WinForms, WPF, Silverlight)
- show system namespaces/libraries/components
- persistence, eg. EF
- give the stuff a web UI and render the visualization in WebGL


Project database
- project name
- list of namespaces
- list of classes
- class: class name, inheritance
- dependencies: other classes used by this class:
	- call into methods of other classes from method implementations
	- member variables and properties of another class
	- return values or parameters in methods (private and public)

Visualization
- draw each class as a rectangle
- put class name on it
- group them by namespace
- draw lines from user to used classes
	- eventually from using method->used class/method/property
	- eventually color the lines based on usage
		- member variable of type
		- method using method/property
		- return value
		- parameter
- allow flying around
- select a class to filter links shown
- when a class is selected zoom in and show details
	- method names
	- dependency types
	- fun analysis

Project processor
- standalone tool
- runs on windows
- uses roslyn to parse the project
- runs on MacOS
- uses libclang to parse the project
- runs on Windows
- uses libclang to parse the project

scm bridge
- standalone tool
- runs on windows, uses GitLib to read from git 
	- if gitlib is .net specific, we're on shaky territory. see if we can have a C gitlib.
- runs on MacOS, uses git lib to get latest from git
- runs on windows, uses the TFS development classes to get the latest from TFS (won't run on MacOS)

scm history
- define interface for git and tfs scm bridges
- same code works with both on both Windows and MacOS