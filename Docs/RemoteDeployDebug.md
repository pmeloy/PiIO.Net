# Remote Deployment and Debugging #

Programming for the Pi on a Windows PC can be a major challenge. You write and build then copy everything to the Pi and hope there are no build errors. If there are then you have do decifer the stack trace that Mono writes in the CLI then try and find that in your source.

I use MonoRemoteDebugger on the Pi and in Visual Studio I use the MonoRemoteDebugger extension. I then have my entire solution reside in my home directory on the Pi. I create a directory for projects then create a Samba share so it's available on my lan. All the code resides on the Pi (except backups which I drag onto my PC).

I run the MonoRemoteDebugger on the Pi from a CLI without detaching so I can see Mono messages and any Console output from the program. GUI Programs will show on the Desktop as if they were run locally.

I write my program in Visual Studio and I can build there to check for build errors the easy way. Then I use the MonoRemoteDebugger plug-in to remote debug. The program is actually run on the Pi, not the PC, but all the debugging information (including Debug.Print() appears on the PC.

Links for the two MonoRemoteDebugger packages

https://marketplace.visualstudio.com/items?itemName=Bongho.MonoRemoteDebugger

https://github.com/techl/MonoRemoteDebugger
