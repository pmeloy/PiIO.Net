Adding a reference to PiIO.Net

To add the project to your solution:
* In the Solution Explorer right-click on the Solution line and select "Add". In the contect menu choose "Existing Project"
* Navigate to your copy of pmeloy/PiIO.Net and select the PiIO.csproj file then click "Ok".
* In the Solution Explorer, right click on your project (not solution) and choose Add Reference.
* On the left side of the requester that appears, click on "Solution" and PiIO.Net should
	appear with a checkbox. Click the checkbox then click "ok"

To use just the DLL in your Project
* Load pmeloy/PiIO.Net into visual studio then build using Release. Close the solution.
* Load your solution into visual studio then in the Solution Explorer right click on your project and choose
	"add" and in the context menu choose "Reference".
* In the requester that pops up, choose "Browse" from the bottom right and navigate to your /bin/Release folder
	and click on "PiIO.dll" and click "Ok".

Automatically copy the DLL to your project
* In the Solution Explorer, under your project, expand the references list and select PiIO.
* In the PiIO Properties window (right click PiIO and select "Properties" if it's not showing) set
	"Copy Local" to True.
	
Then the last step is to add "using PiIO;" to your c# projects and you're ready to go.
