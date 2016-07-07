WhimsyServer
------------

Responds to the following get requests:

| Path | Details
| --- | --- |
| /takesForever | responds after 1 minute |
| /alwaysWorks | always responds successfully |
| /alwaysFails | always responds with a 429 error | 
| /randomlyFails | 50% of the times responds successfully and the other 50% responds with a 429 error |

