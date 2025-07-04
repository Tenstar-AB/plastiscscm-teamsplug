# Teams plug for on premises PlasticSCM

The plug sends POST request to a Power Automate flow.

# Build

Open the solution and build.

# Setup the plug

Since this is a custom plug, it requires some setup.

## Configuraton files
The configuration files in the src/configuration have different purposes:
* `teamsplug.log.conf`: log4net configuration. tells this app how to log, it should be packaged together with the .exe and be next to it
* `teamsplug.config.template`: mergebot configuration template. It describes the expected format of the plug configuration. Should also be packaged together with the .exe and be next to it
The solution is not set up to automatically package these with the build.

## Setup the custom plug to DevOps Plug Types
Once the build is on the Plastic SCM Server computer, for example in C:\myteamsplug, open the admin console and go to the DevOps section, for example http://localhost:7178/devops/plugs

* Press "Add a custom plug type now"
* Fill in the name, plug type, config template path and command fields like in the screenshot, and then save it
<p align="center">
  <img alt="custom plug setup"
       src="https://raw.githubusercontent.com/tenstar/plasticscm-teamsplug/master/doc/img/customplug.png" />
</p>

## Create the Teams plug as a notifier plug
* Go to the dashboard and "Add a new plug"
* Choose plug type "TeamsPlug"
* Give it a name like "TeamsPlug"
* Set the Power Automate Flow url
* Set the email domain for example @yourdomain.com

<p align="center">
  <img alt="teams plug create"
       src="https://raw.githubusercontent.com/tenstar/plasticscm-teamsplug/master/doc/img/teamsplug.png" />
</p>

* Apply and save and hopefully it connects!

## Troubleshoot Plastic Setup
If something doesn't work then there's 3 ways to get more information
1. The page where the custom plug type is configured has a log
2. Check the logs folder of the app, for example in `C:\myteamsplug\logs`
3. Check the Plastic server logs in the devops folder, for example `C:\Program Files\PlasticSCM5\server\devops\logs`

# Power Automate Flow Reference
Create a new Power Automate flow which is triggered by a webhook request.
It should look something like this
<p align="center">
  <img alt="flow overview"
       src="https://raw.githubusercontent.com/tenstar/plasticscm-teamsplug/master/doc/img/flow-overview.png" />
</p>

## Parse JSON
* Content field should use the body of the webhook request. `@triggerBody()`
* The schema could be this
```
{
    "type": "object",
    "properties": {
        "recipient": {
            "type": "string"
        },
        "message": {
            "type": "string"
        }
    }
}
```

<p align="center">
  <img alt="flow overview"
       src="https://raw.githubusercontent.com/tenstar/plasticscm-teamsplug/master/doc/img/flow-parsejson.png" />
</p>


## Post message in chat or channel
* Post as Flow Bot
* Post in Chat with Flow bot
* Recipient is the recipient field from the previous step `@body('Parse_JSON')?['recipient']`
* Message is the message field from the previous step `@body('Parse_JSON')?['message']`

<p align="center">
  <img alt="flow overview"
       src="https://raw.githubusercontent.com/tenstar/plasticscm-teamsplug/master/doc/img/flow-postmessage.png" />
</p>
