## Demo

![EasySave3 0](https://user-images.githubusercontent.com/93043965/146934783-4877e13c-2384-4566-838a-17d24e482d53.gif)

## Description

### Graphical interface

Quitting [Console](https://github.com/itsyanis/EasySave-ConsoleVersion) mode. [EasySave 3.0](https://github.com/itsyanis/EasySave-Graphic-Version/edit/main) is developed in WPF under .Net Core

The software allows you to create an unlimited number of backup jobs

A backup job is defined by:

* An appellation

* A source directory

* A target directory

* A type (Complete, differential)

   
The user can request the execution of one of the backup jobs or the sequential execution of all the jobs.

The directories (sources and targets) can be on:

* Local disks

* External disks

* Network readers

All items in the source directory are affected by the backup.

Daily Log File:

EasySave writes in real time in a daily log file the history of the actions of the backup jobs. The minimum information expected is:

* Timestamp

* Name of the backup job

* Full address of the Source file (UNC format)

* Full address of the destination file (UNC format)

* File size

* File transfer time in ms (negative if error)

The software record in real time, in a single file, the progress of the backup jobs. The information to record for each backup job is:

* Name of the backup job

* Timestamp

The files (Daily Log and Status) are in JSON format. 


### Unlimited number of jobs

The number of backup jobs is unlimited.

### Encryption via CryptoSoft software

The software is capable of encrypting files using [CryptoSoft](https://github.com/itsyanis/CryptoSoft) software. Only files whose extensions have been defined by the user in the general settings will be encrypted.

### Evolution of the Daily Log file

The daily log file contains additional information

    Time required for file encryption (in ms)

        0: no encryption
        > 0: encryption time (in ms)
        <0: error code

### Business software

If the presence of business software is detected, the software prohibits the launch of a backup job. In the case of sequential jobs, the software must complete the current job and stop before starting the next job.
The user can define the business software in the general software parameters (settings).


## Features

* Create Backup Job (unlimited)
* Execute a specific Backup 
* Execute All Backups job
* Show created Backups
* Delete a specific Backup 
* Encrypt Files 
* Software prohibits the launch of a backup job if Buissness software is detected
* Multilanguage


## Tech Stack

* Visual Studio 2019
* .NET Framework (WPF)
* Git 

## Installation

```bash
  git clone https://github.com/itsyanis/EasySave-Graphic-Version.git
```
