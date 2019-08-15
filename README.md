# IRONGATE-ICS

## Decompiled code for the Irongate ICS malware.
(This malware appears to have been a PoC or research as it only works in a spacificly configured simulated enviornment)

## -SCADA 
    - Scans filesystem for Step7ProSim.dll 
    - If dll is found it kills a process 'biogas.exe'
    - Renames original Step7ProSim.dll to Step7ConMgr.dll
    - Drops malicious Step7ProSim.dll.
    - Restart 'biogas.exe'


## -Step7ProSim (Malicious dll) 
      - Does a record/replay attack
          - Records 5sec of calls to ReadDataBlockValue (simlulating reading data from PLC)
          - Returns recorded data when call to ReadDataBlockValue is made
      - Acts as a proxy by forwarding all calls except ReadDataBlockValue to Step7ConMgr.dll(renamed original file)
      - Drops calls to WriteDataBlockValue
      - Instead executes hardcoded values.
  

## FOR RESEARCH PURPOSES ONLY! 


https://www.fireeye.com/blog/threat-research/2016/06/irongate_ics_malware.html


