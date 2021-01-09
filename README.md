AppDevMonitor
=============

C# .Net console app to monitor apple dev center status web page

Updated project to read the current status page as of Jan 9, 2021
I've not been able to test when systems are actually down.
There is at least an api call that can be made now, so I'm assuming if a service is down, there is an event recorded in the data.
If the event is not "resolved", then the text status will show red.

Please do not slam their site! Keep the interval at a respective refresh rate!
