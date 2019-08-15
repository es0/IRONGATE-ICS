# IRONGATE-ICS

## Decompiled code for the Irongate ICS malware. (I did NOT write this)
(This malware appears to have been a PoC or research as it only works in a specifically configured simulated enviornment)

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
      - Instead executes hardcoded values.(see below)
       
        short num = 30563;
        this.WriteInputPoint(272, 0, (object) num);
        this.WriteInputPoint(276, 0, (object) num);
       
  

## FOR RESEARCH PURPOSES ONLY! DO NOT MISUSE!!!


https://www.fireeye.com/blog/threat-research/2016/06/irongate_ics_malware.html
https://ics.sans.org/blog/2016/06/02/irongate-malware-thoughts-and-lessons-learned-for-icsscada-defenders


