# Catharsium.WhatsApp

Catharsium.WhatsApp.Terminal is the entry point to build and run.
Currently Program.cs points to a hardcoded location for an appsettings.json file.
Use appsettings.json-example as a template.

This application supports both WhatsApp group members by their plain phonenumbers or (as you add them to you contact list) later on different names.
      The easiest way to get the current group members imported is by using WhatsApp Web:
      > Select the group, use the browser Inspect tool to select the list of users on top of the page and copy the value here.
      Use the "Import users" action in this tool to generate a "AllUsers.json" in the 'ConversationsFolder'.
      Manually add Aliases and DisplayNames for each user in this json file, for more readable outputs
