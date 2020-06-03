HelpScribble project file.
16
Nzngrhe _nqvb `nsrgl Sbhaqngvba V[P-29Q5078
0
2
Paclink


2007-2014 - Victor Poor, W5SMM
FALSE


1
BrowseButtons()
0
FALSE

FALSE
TRUE
16777215
0
16711680
8388736
255
FALSE
FALSE
FALSE
FALSE
150
50
600
500
TRUE
FALSE
1
FALSE
FALSE
Contents
%s Contents
Index
%s Index
Previous
Next
FALSE

39
10
Scribble10
Paclink Overview
B2F Protocol;Winlink 2000;



Writing



FALSE
16
{\rtf1\ansi\ansicpg1252\deff0{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}{\f4\froman\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red128\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\lang1033\b\fs32 Paclink Overview\f1 
\par \cf0\b0\f0\fs24 
\par \fs20 This program, Paclink, replaces the older Paclink MP. Paclink is an implementation of a Winlink 2000 (WL2K) radio email client that interfaces with most popular email client programs such as Microsoft Outlook Express and Mozilla Thunderbird.  Paclink adds telnet, VHF packet radio, and HF Pactor radio for WL2K connectivity to compatible user email client programs. \cf2 
\par \cf0 
\par Paclink is intended for use with the \cf1\strike Winlink 2000\cf3\strike0\f2\{link=*! \f3 ExecFile("http://www.winlink.org/")\}\cf0\f0  system and uses the \cf1\strike B2F\cf3\strike0\f2\{link=*! \f3 ExecFile("http://www.winlink.org/B2F.htm")\}\cf0\f0  protocol that supports attachments, multiple addresses and tactical addresses. Paclink is \cf4\strike licensed\cf3\strike0\{linkID=320\}\cf0  for amateur and MARS use only by the \cf4\strike Amateur Radio Safety Foundation\cf3\strike0\{linkID=20\}. \cf0  Commercial application of Paclink is not licensed and not permitted.  For commercial use \cf4\lang1024\strike contact\cf3\strike0\{linkID=300\} \cf0\lang1033 the authors.
\par 
\par \cf1\b\fs32 Adapting Paclink for Emergency Communications\cf0\b0\fs20 
\par \pard\sb100\sa100 Paclink is optimized for emergency communications and leverages all the capability of the WL2K system. However before adopting Paclink as your digital program for emergency communications purposes, please check with your local group for transition plans. This may be your ARES, RACES, MARS etc..... group who will be responsible for providing training and support.\f4\fs24  
\par \pard\f0\fs20 
\par \cf4\strike Installation\cf3\strike0\{linkID=30\}\cf0   
\par \cf4\lang1024\strike Contacts\cf3\strike0\{linkID=300\}\cf0\lang1033 
\par \cf4\strike License\cf3\strike0\{linkID=320\}\cf1\b\f1\fs32 
\par 
\par }
20
Scribble20
Amateur Radio Safety Foundation
Amateur Radio Safety Foundation;



Writing



FALSE
11
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil Arial;}{\f1\fnil\fcharset0 Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\f0\fs32 Amateur Radio Safety Foundation\f1  Inc\cf0\b0\f0\fs20 
\par 
\par Paclink is made possible through the Amateur Radio Safety Foundation Inc. \f1  The ARSF Inc is a Florida non profit corporation set up to promote safety and emergency communications using amateur radio.  The ARSF Inc provides information, training and support to local emergency groups and aids in the development and expansion of ham radio emergency applications.  The ARSF Inc is exempt from Federal Income tax under section 501(c) (3) of the Internal Revenue Code (IRC) and is classified as a public charity. Contributions to the ARSF are tax deductible under sections 170, 2055, 2106 and 2522 of the IRC. Donations of money, time, talent or equipment are much appreciated and are used to maintain and improve programs such as Paclink and the WL2K system and servers.\f0  \f1 See \cf1\strike http://www.arsfi.org\cf2\strike0\f2\{link=*! ExecFile("http://www.\f3 arsfi.org\f2 ")\}\cf0\f1  for more information,  to join the ARSF Inc or to make a contribution. \f0 Your membership in and support for the ARSF \f1 Inc \f0 make programs like Paclink and the Winlink 2000 system possible. 
\par \cf3\strike\f1 
\par Paclink Overview\cf2\strike0\{linkID=10\}
\par \cf3\strike Installation\cf2\strike0\{linkID=30\}\cf0 
\par \pard\qc 
\par \cf2\{bmc About.bmp\}\cf0\f0 
\par }
30
Scribble30
Installation
Cloned Installations;Computer and OS requirements;Installation;Multiple Paclink Instances;Ports;



Writing



FALSE
63
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}{\f4\fswiss\fcharset0 Arial;}{\f5\froman\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;\red255\green0\blue0;\red0\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Installation\cf0\b0\f1\fs20 
\par 
\par \f0 If you are reading this text then you have successfully completed the first step of the Paclink installation. Before you can use the program the \cf2\strike site parameters\cf3\strike0\{linkID=40\}\cf0 , and \cf2\strike user accounts\cf3\strike0\{linkID=80\}\cf0  (if any) must be completed.
\par 
\par Upon startup of the Paclink program if there is an available Internet connection Paclink checks and automatically installs any \cf2\lang1024\strike updates\cf3\strike0\{linkID=330\}.  \cf0 Upon initialization the \cf2\strike Main Display\cf3\strike0\{linkID=340\} \cf0 shows the site call sign and enables the main menu bar. 
\par 
\par \pard\sb100\sa100 Installation problems are sometimes experienced in certain Vista OS installations. The majority of issues with Vista seem to center around drivers especially the drivers for USB to serial adapters.  \lang1033 The FTDI drivers available both from \cf1\strike SerialGear\cf3\strike0\f2\{link=*! \f3 ExecFile("http://www.serialgear.com/ftdi-chip-drivers.cfm")\}\cf0\f0  and via Microsoft's update appear very stable and their latest chip sets are also USB 2.0 capable. Good results have been reported using a 4 port adapter from \cf1\strike usbgear\cf3\strike0\f2\{link=*! ExecFile("\f0 http://www.usbgear.com"\f2 )\}\cf0\f0  on a Vista 64 installation.
\par \pard Some \cf2\lang1024\strike USB to serial adapters\cf3\strike0\{linkID=380\}\cf0\lang1033  will \i not \i0 work reliably with Paclink (or any other .NET program using the standard MS .NET serial port). 
\par 
\par \pard\sb100\sa100\cf1\b\f4\fs32 Computer and OS Requirements\cf0\b0\f5\fs24 
\par \pard\f0\fs20 Paclink will run on any modern 32 bit Windows OS (Windows 2K, Win XP (Home or Pro), Windows Vista). There are minimal CPU demands with the exception of WINMOR operation.  The heavy real-time DSP demands of WINMOR require a compter of at least 700 MHz Pentium/Celeron class and at least 256 Meg of memory. If multiple applications are running a faster computer and more RAM may be required. 
\par 
\par \pard\sb100\sa100\cf1\b\f4\fs32 Required Ports\cf0\b0\f5\fs24 
\par \f4\fs20 Paclink uses a number of internet ports in normal operation and for auto updating. The program \i will\i0  function with no internet connection but auto updates and changes to accounts requiring access to the WL2K database cannot be done. If you are running with a firewall program or a hardware router/firewall it may be necessary to open these ports depending on how the firewall is set up. If you are using ZoneAlarm open up the ICMP ports listed below. Here is a list and functions of the various ports used:
\par \cf4\b Outbound Ports:\cf0\b0  (all outbound ports are grouped to facilitate router/firewall setup) \f5\fs24 
\par \f4\fs20 CMS Access: TCP link to appropriate CMS sites, port 8775\f5\fs24 
\par \f4\fs20 Autoupdate: Outbound http connection to autoupdate.winlink.org port 8776\f5\fs24 
\par \f4\fs20 Telnet connections: Port 8772 for CMS Telnet sites.\f5\fs24 
\par \f4\fs20 Version/Status Reporting: UDP outbound to winlink.org port 8778 \cf4\b 
\par Inbound Ports:\cf0\b0  The only inbound ports used are the local POP3 (default 110) and SMTP (default 25) TCP ports. If these conflict with other programs or services on the local machine they can be changed in the \cf2\strike\f0 Site Properties menu \cf3\strike0\{linkID=40\}\cf0  \f4 to other unused ports.
\par \pard\cf1\lang1024\b\f0\fs32 
\par Cloned Paclink Setup on Another Computer
\par \cf5\b0\fs20 Sometimes it is desirable to clone (duplicate) a Paclink setup on a different computer. The easiest way to do this is simply to do an Install of Paclink and then copy over the original Paclink.ini file from the first installation to the new Paclink \\Bin directory. The Paclink.ini file is located in the ...\\Bin\\ directory. If the new computer has different COM port assignments or different radios etc..... then these of course will have to be edited using the normal channel setup menus.\cf1\b\fs32 
\par \cf3\b0\fs20 
\par \cf1\b\fs32 Multiple Paclink Instances
\par \cf5\b0\fs20 Paclink may be installed and run as multiple instances.  This may be desirable for example if the same computer is used as both a normal amateur site and also a MARS site or otherwise must operate under multiple Paclink call signs. The following are important and necessary requirements for installing and running multiple Paclink instances on the same computer.
\par \cf3 
\par \cf4\b\i 1) Different Directories for EACH instance.  \b0\i0 
\par \cf5 If more than one instance of Paclink is used the program \i must\i0  be run from different Paclink directories and \i NOT\i0  share any of the same subdirectories. The Paclink.ini file (located in the \\Bin\\ directory) holds the settings for the instance. 
\par 
\par \cf4\b\i 2) Creating a multiple instance: (Follow carefully!)\b0\i0 
\par \cf5 a) Create a new directory (any desired name) for the new Paclink instance. It can be on any hard drive:
\par b) Copy the following directories and their contents from your original Paclink directory (default = C:\\Paclink\\) to your new Paclink Instance directory:
\par \tab\\Bin, \\Data, \\Documentation, \\Help
\par c) Delete the file "Paclink.ini" in the new \\Bin directory. This will give you a fresh start upon startup of the new instance.
\par d) If desired create a shortcut for the new instance by right clicking Paclink.exe in the NEW instance \\Bin directory. Select "create shortcut here" upon right click release.  Drag the shortcut (\cf4 DO \i NOT\i0  move the actual Paclink.exe file\cf5 )  from the ...\\Bin directory to the desired place (e.g. Desktop). Rename the shortcut to an appropriate name for the new instance.
\par e) Start the new instance by double clicking on the new instance shortcut or on Paclink.exe in the new instance \\Bin directory.
\par f)  Complete the Registration, Site Properties and desired Channel menus for the new instance. Add any call sign or Tactical accounts for the new instance.
\par \cf4 
\par \b\i 3) Instances may not share common accounts. \cf5\b0\i0  
\par Call sign or Tactical Accounts set up in the local computer or on the LAN should only use and connect to \i ONE\i0  Paclink instance. Allowing accounts to "cross" to a different instance will cause problems in the routing of messages to those accounts. 
\par 
\par \cf4\b\i 4) Instances must have unique site call signs.\b0\i0 
\par \cf5 The base call sign (set up in the registration dialog) \i MAY\i0  be the same in multiple instances but the site call sign (set up in the properties dialog) \i MUST\i0  \i BE UNIQUE\i0  (call sign or -ssid) for each instance. 
\par 
\par \cf4\b\i 5) If instances are to be run simultaneously additional limitations apply due to hardware considerations:\b0\i0 
\par \cf5 a) Paclink instances must use different SMTP and POP3 port numbers and have their email clients configured accordingly. If there is a port conflict Paclink will pop up an error message.
\par b) Instances should not share hardware ports (e.g. TNC serial ports) except in the case of those using AGWPE. AGWPE will allow TNC sharing if the call signs are unique.
\par \cf4\b\i 
\par 6) Care must be used if multiple Paclink instances handle mail for the SAME Call sign or Tactical Address accounts:\b0\i0 
\par \cf5 a) It is possible to set up multiple Paclink instances (on one computer or multiple computers) that handle mail for the same Call sign or Tactical Address Accounts. However when mail is picked up by any of these instances it is considered \i DELIVERED\i0  to the account and will \i NOT\i0  then be forwarded to any other instance. This works much the same way normal ISP Email accounts work.
\par \cf4\tab   
\par \cf2\strike Overview\cf3\strike0\{linkID=80\}
\par \cf2\strike Site Properties\cf3\strike0\{linkID=40\}\cf0 
\par \cf2\lang1033\strike Email Clients\cf3\strike0\{linkID=310\}
\par \cf2\lang1024\strike Updates\cf3\strike0\{linkID=330\}
\par \cf2\strike Main Display\cf3\strike0\{linkID=340\}
\par 
\par \cf0\lang1033\f1 
\par 
\par }
40
Scribble40
Site Properties
Site Properties;



Writing



FALSE
52
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}{\f4\fswiss\fprq2\fcharset0 Arial;}{\f5\fswiss\fprq2\fcharset0 Calibri;}{\f6\froman\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Site Properties\cf0\b0\f1\fs20 
\par \f0\fs24 
\par \fs20 Use this dialog (opened with \i Files|Site Properties\i0 ) to initialize or change this site's characteristics.
\par 
\par \pard\qc\cf2\{bmc Site Properties.bmp\}\cf0 
\par \pard 
\par \cf3\b\i Callsign\cf0\b0\i0   -This is the site callsign. Enter only the base callsign.  Do not specify a SSID
\par 
\par \cf3\b\i Password (for secure login only)\b0  \cf0\i0  - Letters and numbers between 4 and 12 characters long. This is only required if you have Use Secure Login checked. It must match the login associated with the base callsign login recorded in the system database.
\par 
\par \cf3\b\i Password (for POP3/SMTP)\b0  \cf0\i0  - Letters and numbers between 4 and 12 characters long. This must be the password you use from your local email client to access the POP3/SMTP ports in Paclink for mail to the local Paclink callsign.
\par 
\par \cf3\b\i Site Grid Square\cf0\b0\i0   -This is the grid square for the Paclink site. Link to \cf1\strike http://www.arrl.org/locate/grid.html\cf2\strike0\f2\{link=*! ExecFile("http://www.\f3 arrl.org/locate/grid.html/\f2 ")\}\cf0\f0   to find your grid square.)
\par \cf3 
\par \b\i Message Size Limit\b0 : \cf0\i0 - This is the maximum size of message in its compressed format the this site will accept either from a CMS site or from a user's SMTP client. There is also a system-wide limit of 120,000 bytes (after compression) that will be accepted anywhere within the system.
\par 
\par \cf3\b\i SMTP port number\cf0\b0\i0   - Default 25  The program tests to see if the port is in use which can sometimes happen if other programs or services are running (e.g. some email virus or spam scanners). If a conflict is found you can either shut down the conflicting program or service or change the port number to an unused one (e.g. 26 or 111).  If the port numbers are changed for either the SMTP or POP3 port numbers you will also have to change them in the account set up on the \cf4\strike email client\cf2\strike0\{linkID=310\}\cf0 . 
\par 
\par \cf3\b\i POP3 port number\cf0\b0\i0   - Default 110 (see info above on port number conflicts) 
\par 
\par \pard\li840\b\f4 A note on port numbers for Windows Vista and Windows 7 users. Paclink may not be able to access ports 25 and 110 unless it is launched with Administrator privileges.  This can be easily resolved by simply choosing port numbers greater than 1023.  For example, selecting an SMTP port of 2025 and a POP3 port of 2110 will allow Paclink to operate without the need for any Administrator privileges.\b0\f5\fs22 
\par \pard\cf3\i\f0\fs20 
\par \b Local IP Address\cf0  \b0\i0 - List box used to select a specific Local IP address (NIC Adapter). Set to "default" unless this installation is multi homed and you wish to force Paclink to use a specific NIC adapter/Local IP address.
\par \cf3\i 
\par \b Callsign ID Prefix\cf0  \b0\i0 - Default blank. Optional prefix sent to ID the call to meet legal requirements. Does NOT change the actual call sign in the Winlink database. Example "XE1/" 
\par 
\par \cf3\b\i Callsign ID Suffix \cf0\b0\i0 - Default blank. Optional suffix sent to ID the call to meet legal requirements. Does NOT change the actual call sign in the Winlink database.  Example "/MM"
\par 
\par \cf3\b\i Use Secure Logon\cf0\b0\i0  - Active if checked. See \cf4\strike Secure Logon\cf2\strike0\{linkID=60\}\cf0  for details.
\par 
\par \cf3\b\i LAN Accessible\cf0\b0\i0   - Default unchecked. Check if this Paclink Instance will be accessible by other computers on the local LAN.
\par 
\par \cf3\b\i Enable Range and Bearing Display\cf0\b0\i0  -  This enables a feature to show a screen indicating the remote station's call sign, start time of the session (UTC) and  range and bearing. Range is in statute miles, bearing in degrees True (great circle). Range rings use a log scale inner ring 10 sm, 100 sm, 1000 sm, outer ring 10,000 sm.  The screen is only shown when a forwarding session is active and the remote station has sent his grid square. Default is unchecked.
\par 
\par \cf3\b\i Add this account to Outlook Express\cf0\b0\i0  -  If checked the site call sign will be added as an account to the Outlook express (if present) on the local computer.  The site call sign account can also be added manually to any other \cf4\strike Email Clients \cf2\strike0\{linkID=310\}\cf0  
\par 
\par \cf3\b\i Send messages via radio-only forwarding\cf0\b0\i0  - If checked, when Paclink connects to an RMS it signals the RMS that all messages being sent are to be transferred using radio-only forwarding rather than being sent through the Internet to a CMS.
\par 
\par \cf3\b\i Connect directly to CMS telnet port\cf0\b0\i0  - This is the normal default selection. When selected Paclink will search for and connect to a CMS site each time a telnet channels is selected.
\par \cf3\b\i 
\par Connect via RMS Relay telnet port\cf0\b0\i0  - When selected Paclink will connect to the RMS Relay site indicated in the adjacent text box. The RMS Relay site will pass the connection on to a CMS site if it is available and will act as a local telnet port, saving and routing messages locally, if it cannot reach a CMS site.
\par 
\par \cf3\b\i IP address of RMS Relay\cf0\b0\i0  - This is the IP address of an RMS Relay site on the local area network shared with this Paclink site. It may be located in the same machine ("localhost") or on another machine on the net. Normally the entry is the name of the machine that is running RMS Relay but it may also be a dotted IP address if the RMS Relay host has a static IP address.
\par 
\par \cf4\lang1024\strike Overview\cf2\strike0\{linkID=10\}
\par \cf4\strike Accounts\cf2\strike0\{linkID=80\}
\par \cf4\strike Channels\cf2\strike0\{linkID=110\}
\par \cf4\strike Email Clients\cf2\strike0\{linkID=310\}
\par \cf0\lang1033\f6\fs24 
\par }
50
Scribble50
Simple Terminal
Simple Terminal;



Writing



FALSE
17
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Simple Terminal\cf0\b0\f1\fs20 
\par 
\par \f0 A simple terminal is provided to facilitate testing a TNC manually from the keyboard. Use this terminal window to verify the operation of a TNC before configuring it into the system. The facility may be accessed by clicking on \i Help|Simple Terminal...
\par \i0 
\par The settings menu selection will allow you to configure the terminals profile as needed. It is recommended that you test a TNC using 9600 baud, 8-bit data, no-parity before attempting to use it with Paclink. In most cases the Paclink channel code will successfully start and configure a TNC automatically but there can be exceptions when using the TNC the first time. Use the simple terminal program to clear up any issues in advance.
\par \f1 
\par \f0\tab\tab\cf2\{bmc Simple Terminal.bmp\}
\par 
\par 
\par 
\par \cf0 Use the bottom line in the window to enter text or commands. 
\par 
\par Note: While it is \i possible\i0  to operate a TNC directly using the simple terminal this should not be used to make connections to WL2K gateways or other stations. The concept and goal of Paclink is to use the much more accurate and efficient auto forwarding mechanism of Winlink and to avoid time and spectrum wasting keyboard connections.\cf2 
\par \cf0\f1 
\par }
60
Scribble60
Secure Login
Secure Login;



Writing



FALSE
13
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\froman\fcharset0 Times New Roman;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Secure Login\cf0\b0\f1\fs20 
\par 
\par \f0 Secure Login adds a strong layer of protection from someone logging in to an RMS site using your call sign.  If selected, when you login a secure login mechanism is implemented requiring your password (which is also saved in the Winlink 2000 data base).  The mechanism never sends the actual password over the air and is very secure.  If you use secure login be careful to remember and keep track of your password or you could be blocked from logging in to the system.
\par 
\par \cf2\lang1024\strike Overview\cf3\strike0\{linkID=80\}
\par \cf2\strike Site Properties\cf3\strike0\{linkID=40\}
\par \cf2\strike Accounts\cf3\strike0\{linkID=80\}
\par \cf2\strike Channels\cf3\strike0\{linkID=110\}
\par \cf0\lang1033\f2\fs24 
\par \f1\fs20 
\par }
70
Scribble70
AGW Engine Properties
AGW Packet Engine;



Writing



FALSE
26
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red0\green128\blue0;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 AGW Engine Properties\cf0\b0\f1\fs20 
\par 
\par \f0 Paclink supports the AGW Packet Engine. This is third party software that manages multiple TNCs allowing several AGW compliant programs to share TNCs, radios and antennas simultaneously.  AGW interfaces to the TNCs using their basic KISS protocol and supports most TNCs that are KISS compatible including the small $99 USB \cf1\strike Elcom Micro TNC\cf0\strike0  \cf2\f2\{link=*! ExecFile("http://\f0 http://www.elcom.gr/ \f2 ")\}\cf0\f0 . With the AGW Packet Engine the same TNC can be used for Paclink, a digipeater or node, and a terminal program.  The AGW PE does require some setup and initial testing but once installed is reliable and useful in  many applications. In some advanced applications it is possible to run Paclink with the AGW PE installed on a remote computer. AGW PE also supports sound card modems for 1200 and 9600 baud packet but a dedicated sound card is recommended in such applications. AGW PE pro also supports the built in TNCs found in some Kenwood radios (D710 recommended).
\par \f1 
\par \f0 If you have a TNC that is not directly supported by Paclink the chances are pretty good that it can be used with the aid of the AGW Packet Engine supplied by \cf1\strike SV2AGW\cf2\strike0\f2\{link=*! ExecFile("http://www.elcom.gr/sv2agw/inst.htm")\}\cf0\f0 . See the \cf1\strike AGW Packet Engine WEB page\cf2\strike0\f2\{link=*! ExecFile("http://www.elcom.gr/sv2agw/index.html")\}\cf0\f0  for more information.
\par 
\par The \cf3\lang1024\strike SCS PTC II\cf2\strike0\{linkID=370\}\cf0\lang1033  (all models) may be used with the AGW Packet Engine (free or Pro).
\par 
\par If you do choose to use the AGW Packet Engine it needs to be installed and configured and then the following form used to link the program to Paclink. It is strongly recommended that the AGW PE installation be installed and \i TESTED\i0  using a simple AGW compliant terminal program like AGW Term \i before\i0  trying to use with Paclink.
\par 
\par \pard\qc\cf2\{bmc AGW Engine Properties.bmp\}
\par \pard 
\par \cf4\b\i Path to AGW Packet Engine\b0\i0  \cf0 - Once you have installed the AGWPE (Free or Pro version) You will have to point to the exe directory where the packet engine was installed. The default directory for PE Pro is shown but depending on where you installed the program you may have to browse to the directory that has the Packet Engine or Packet Engine Pro .exe file. 
\par 
\par \cf4\b\i AGW Host\cf0\b0\i0 - This is the host IP address (friendly or dotted format) where the AGW PE is installed. It can be a remote computer or local on the same computer as Paclink. If the AGW is running on the local computer simply use "localhost" or "127.0.0.1" for the host address.
\par \cf4 
\par \b\i AGW Port\cf0\b0\i0 - This is the port the AGW PE is listening on. The default port for AGWPE is 8000 but it may have been changed when it AGWPE was installed.
\par \cf4 
\par \b\i AGW User Id\cf0\b0\i0 - Normally the user ID is not required.  If the AGWPE (local or remote) was set up requiring a secure login you will need to put the USER ID (as set up on the AGWPE in here).
\par \cf4 
\par \b\i AGW Password\cf0\b0\i0 - Normally not required unless the AGWPE was set up requiring secure login. Then the password should match the one for the USER ID as set up in AGW PE.
\par \cf4 
\par \f1 
\par }
80
Scribble80
Accounts
Accounts;



Writing



FALSE
19
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Accounts
\par \cf0\b0\f1\fs20 
\par \f0 User accounts are how Paclink knows what user email client to send messages to. User accounts in Paclink and the email client \i must \i0 have a name and password. The name can be a \cf2\strike radio callsign\cf3\strike0\{linkID=90\}\cf0  such as W3GHI-4 or a \cf2\strike tactical address\cf3\strike0\{linkID=100\}\cf0 . 
\par 
\par Every Paclink site needs one radio call sign account (created by default when the site is set up). This name is the call sign for the site and is the call sign used to send messages to the site's sysop.
\par 
\par Beyond that any number of additional may be set up to service users that use Paclink to access Winlink 2000 from their personal email programs. Messages are always addressed to Paclink users in the form of <account name>@winlink.org. These other user accounts are always sent to WL2K \i via\i0  the primary radio call sign account.
\par 
\par For each account in Paclink there should be a corresponding account set up on at least one \cf2\strike email client\cf3\strike0\{linkID=310\}
\par \cf0 
\par \cf2\lang1024\strike Overview\cf3\strike0\{linkID=10\}\cf0 
\par \cf2\lang1033\strike Callsign Accounts\cf3\strike0\{linkID=90\}\cf0 
\par \cf2\strike Tactical Accounts\cf3\strike0\{linkID=100\}\cf0 
\par \cf2\strike Editing Account Properties\cf3\strike0\{linkID=75\}
\par \cf2\strike Email Clients\cf3\strike0\{linkID=310\}
\par \cf0\f1 
\par }
90
Scribble90
Callsign Accounts
Callsign Accounts;



Writing



FALSE
23
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Callsign Accounts\cf0\b0\f1\fs20 
\par 
\par \f0 The Manage Callsign Accounts dialog box is used to enter or delete call sign accounts. When a new account is entered the properties for the account are saved both in the Paclink.ini file and optionally in Outlook Express in this machine.
\par 
\par \pard\qc\cf2\{bmc Manage Callsign Accounts.bmp\}\cf0 
\par \pard 
\par \cf3\b\i Account Callsign\cf0\b0  \i0 - May be any valid radio call sign, with or without and SSID. \cf3\i IMPORTANT: A callsign with an -ssid become a SEPARATE user from the base call sign.  Mail sent to the call sign with -ssid will not be sent or copied to the base call sign and vice versa.
\par \cf0 
\par \cf3\b Callsign POP3/SMTP Account Password\cf0\b0  \i0 - Any number of letters and numbers, 4 to 12 in length. This becomes the email client password for the callsign account.
\par 
\par \cf3\b\i Add Selected Account to Outlook Express\b0\i0  \cf0 - This button automatically adds the named account to Outlook Express installed \ul in this machine\ulnone . Once installed Outlook Express must be opened and the password entered to gain access to messages. Other email clients can be used as long as they interface to the SMTP and POP3 standards and logon mechanism. See \cf4\lang1024\strike email Clients \cf2\strike0\{linkID=310\} \cf0 for help in configuring other email clients.
\par \lang1033 
\par \lang1024 A callsign account name must consist of a valid ham or MARS radio call sign. Valid call sign account name examples:  W1ABC, WA9DEF-12 
\par 
\par The <account name>@Winlink.org (case insensitive) will be the email address of the account user. To enter a tactical account use the \cf4\strike Tactical Accounts\cf2\strike0\{linkID=100\}\cf0  dialog box.
\par 
\par \cf4\strike Overview\cf2\strike0\{linkID=80\}\cf0 
\par \cf4\strike Accounts\cf2\strike0\{linkID=80\}\cf0 
\par \cf4\lang1033\strike Editing Account Properties\cf2\strike0\{linkID=75\}\cf0\f1 
\par \cf4\lang1024\strike\f0 Email Clients \cf2\strike0\{linkID=310\} \cf0\lang1033\f1 
\par }
100
Scribble100
Tactical Address Accounts
Tactical Address Accounts;



Writing



FALSE
23
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil\fprq1\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Tactical Address Accounts\cf0\b0\f1\fs20 
\par 
\par \f0 The Manage Tactical Accounts dialog box is used to enter or delete tactical address accounts. When a new account is entered the properties for the account are saved both in the Paclink.ini file and in the Winlink 2000 (WL2K) master database. In an emergency it is possible to add a new account even if there is no Internet access to WL2K.\i  \i0 The next successful connection to RMS Packet, RMS Pactor, or to the CMS via telnet will add the new tactical addresses to the master database (except for local connections using RMS Relay).
\par 
\par \pard\qc\cf2\{bmc Manage Tactical Accounts.bmp\}\cf0 
\par \pard 
\par \cf3\b\i Account Name\cf0\b0  \i0 - May be any valid tactical address. \lang1024 A tactical account name may consist of a tactical address of alpha characters ONLY or alpha characters ONLY, followed by a dash, followed by alphanumeric characters . A name may not exceed 12 characters.  When connected to the internet Paclink will verify that the new Account name is unique to the WL2K system.
\par 
\par Valid account name examples:  MLBSHELTER, REDCROSS-12, POLICE-9A, FLDADEEOC-1 
\par \lang1033 
\par \cf3\b\i Account Password\cf0\b0  \i0 - An number of letters and numbers, 4 to 12 in length (case insensitive). If the Tactical Address is registered in the WL2K database then you MUST know the correct Password to add the account to Paclink (security feature). This password becomes the WL2K password and also the password for the local Email client for this account. If the Tactical Address is not yet registered in the WL2K database then it will be added with this account password.
\par 
\par \cf3\b\i Add Selected Account to Outlook Express\cf0\b0\i0  - Clicking this button automatically adds the named account to Outlook Express (if installed)  \ul in this machine\ulnone . Once installed Outlook Express must be opened and the password entered to gain access to messages. No not click this button if the account is to be on another machine on the LAN. Other email clients can be used as long as they  interface to the SMTP and POP3 standards and logon mechanism. See \cf4\lang1024\strike email Clients \cf2\strike0\{linkID=310\} \cf0 for help in configuring other email clients.\lang1033 
\par 
\par \lang1024 The <account name>@Winlink.org (case insensitive) will be the E-mail address of the account user. New accounts are not activated until at least one message has been sent from that account to at least one recipient. To enter a ham or MARS radio call sign account use the \cf4\strike Callsign Accounts\cf2\strike0\{linkID=90\}\cf0  dialog box.
\par \f2 
\par \cf4\strike\f0 Overview\cf2\strike0\{linkID=80\}\cf0 
\par \cf4\strike Accounts\cf2\strike0\{linkID=80\}\cf0 
\par \cf4\lang1033\strike Editing Account Properties\cf2\strike0\{linkID=75\}
\par \cf4\lang1024\strike email Clients \cf2\strike0\{linkID=310\}\cf0\lang1033\f1 
\par }
110
Scribble110
Channels
Channels;



Writing



FALSE
22
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Channels\cf0\b0\f1\fs20 
\par 
\par \f0 A channel in this context is the means Paclink uses to connect to and exchange mail with Winlink 2000. The channel connection is always made to a RMS site where each message is received, parsed, and routed to it's destination.
\par 
\par There are currently four types of channels supported \cf2\strike telnet\cf3\strike0\{linkID=120\}\cf0 , VHF/UHF packet (\cf2\strike TNC\cf3\strike0\{linkID=130\}\cf0  or \cf2\strike AGW\cf3\strike0\{linkID=140\}\cf0 ),  \cf2\strike HF Pactor\cf3\strike0\{linkID=150\}\cf0  and \cf2\strike WINMOR \cf3\strike0\{linkID=160\}\cf0   Upon an automatic connection request every channel marked as enabled will be tried in order until a successful exchange is completed. Channels have a priority associated with them. By default the highest priority is telnet (default = priority 1), the next is VHF packet (default = priority 3), and finally HF Pactor (default = priority 5). When a connect cycle is started channels are attempted in order from priority 1 to priority 5.  If multiple channels of the same priority are enabled they are tried randomly. The connect attempt ends with the first channel where a successful exchange of messages is completed.
\par 
\par This mechanism is designed to provide a powerful redundant capability which automatically tries alternative channels until a path to WL2K is found.
\par 
\par A connection cycle may be started automatically (see \cf2\strike RMS Polling\cf3\strike0\{linkID=200\}\cf0 ) or may be started manually by using the Connect entry on the main menu. A manually initiated connection may initiate a full connection cycle or initiate a connection only on a specific channel.
\par 
\par Channel priorities may be altered by the sysop as needed by circumstances.\f1 
\par 
\par \cf2\strike\f0 Overview\cf3\strike0\{linkID=10\}\cf0 
\par \cf2\strike Telnet Channels\cf3\strike0\{linkID=120\}\cf0 
\par \cf2\strike Packet TNC Channels\cf3\strike0\{linkID=130\}\cf0 
\par \cf2\strike Packet AGW Channels\cf3\strike0\{linkID=140\}\cf0 
\par \cf2\strike HF Pactor Channels\cf3\strike0\{linkID=150\}
\par \cf2\strike WINMOR Channels\cf3\strike0\{linkID=160\}\cf0\f1 
\par 
\par }
120
Scribble120
Telnet Channels
Telnet Channels;



Writing



FALSE
25
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil\fcharset0 Courier New;}{\f3\fnil Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Telnet Channels\cf0\b0\f1\fs20 
\par 
\par \f0 The preferred connection from Paclink to WL2K is always telnet if it is available. Telnet channels are now automatically directed to one of the CMS Telnet servers. By default a new telnet channel is assigned a channel priority of 1.  A telnet channel will try all available CMS Telnet servers before abandoning the channel connection attempt. Normally only one telnet channel is needed in a Paclink instance but multiple channels may be useful if the Paclink installation has multiple IP connection mechanisms (multi homed) . This dialog may be accessed by clicking on Files|TelnetChannels...
\par 
\par \pard\qc\cf2\{bmc Telnet Channels.bmp\}\cf0 
\par \pard 
\par \cf3\b\i Channel\cf0  \cf3 Name\cf0\b0\i0  - Enter a channel name. It should be descriptive enough to make the channel easily identified. If viewing or editing an existing channel use the drop down box to select an existing telnet channel.
\par 
\par \cf3\b\i Channel Priority\cf0\b0\i0  - The default priority for telnet is 1. If you want change the order in which channels are tried you may set the priority between 1 (highest) to 5 (lowest).
\par \cf3 
\par \b\i Local IP Address\cf0\b0\i0  - If your computer uses a single connection to your cable or DSL modem or to your LAN then there will be only one default entry. Do not attempt to change it. If your computer has multiple accesses (multi-homed) then you will need to select the local IP address you want this telnet channel to use.
\par 
\par If you want to try one internet access path and if that fails try a second,  set up two telnet channels but with different local IP addresses and priorities. In this way you could for example have a  higher priority Telnet channel using a wired internet connection and a lower priority Telnet channel using a wireless TCP link (e.g.   \cf1\strike AvaLAN\cf2\strike0\f2\{link=*! ExecFile("http://www.avalanwireless.com/assets_2006/AW900m-SRK_ProductBrief.pdf")\}\cf0\f0  or  \cf1\strike D-STAR\cf2\strike0\f3\{link=*! \f2 ExecFile("http://www.icomamerica.com/amateur/dstar/")\}\cf0\f0  ) 
\par \cf3 
\par \b\i Channel Enabled\cf0\b0\i0  - If this box is unchecked then Paclink will not attempt to use this channel.
\par 
\par \cf4\strike Overview\cf2\strike0\{linkID=10\}\cf0 
\par \cf4\strike Channels\cf2\strike0\{linkID=110\}\cf0 
\par \cf4\strike VHF Packet Channels\cf2\strike0\{linkID=130\}\cf0 
\par \cf4\strike HF Pactor Channels\cf2\strike0\{linkID=150\}\cf0\f1 
\par \cf4\strike\f0 WINMOR Channels\cf2\strike0\{linkID=160\}\cf0\f1 
\par \cf4\strike\f0 
\par }
130
Scribble130
Packet TNC Channels
Native KISS driver;Packet TNC Channels;Radio Control;



Writing



FALSE
57
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Packet TNC Channels\cf0\b0\f1\fs20 
\par 
\par \f0 Use this dialog, accessed by clicking on Files|Packet TNC Channels... You may establish any number of TNC packet channels connecting either directly to an RMS site or Telpac site or over a UHF/VHF network using the optional connect script.
\par 
\par \pard\qc\cf2\{bmc Packet TNC Channels.bmp\}\cf0 
\par \pard 
\par \cf3\b\i Channel Name\cf0\b0\i0  - Enter a channel name. It should be descriptive enough to make the channel easily identified. If viewing or editing an existing channel use the drop down box to select and existing TNC packet channel.
\par 
\par \cf3\b\i Remote Callsign\cf0\b0\i0  - Enter the call sign of the remote RMS or Telpac site this channel is to connect to. If using a connect script over a packet network this is the \i END\i0  target call sign not the call of any intermediate nodes or digis.
\par 
\par \cf3\b\i TNC Type\cf0\b0\i0  - Select the type of TNC used by this channel. (Note: If using the PK232 you must have firmware revision 7.1 or later)  Paclink now supports a Native KISS mode (no third party software required). TNC types ending in "int" are  internal to the specific radio or manufacturer (Kenwood D710, D700, D7, TS-2000 and Alinco DR135T, DR435T) and operate in Native KISS mode.
\par \cf3 
\par \b\i Channel Priority\cf0\b0\i0  - The default priority for packet channels is 3. If you want change the order in which channels are tried you may set the priority between 1 (highest) to 5 (lowest).
\par \cf3 
\par \b\i Activity Timeout\cf0\b0\i0  - Set the number of minutes the channel should wait before disconnecting due to channel inactivity.
\par \cf3 
\par \b\i Channel Enabled\cf0\b0\i0  - If this box is unchecked then Paclink will not attempt to use this channel.
\par \cf3\b\i 
\par On-Air Baud Rate\cf0\b0\i0  - Only enabled for Native KISS mode TNCs this selects the 1200 baud or 9600 baud section of the .aps file to set appropriate parameters.
\par \cf3\b\i 
\par Serial Port\cf0\b0\i0  - Select the serial port used to connect to the TNC. (8N1 setting is implied) 
\par \cf3 
\par \b\i Baud Rate\cf0\b0\i0  - Select the baud rate used to connect to the TNC. This is NOT the on-air radio baud rate. Recommend 9600 baud or higher for most TNCs.
\par \cf3 
\par \b\i Optional Connect Script\cf0\b0\i0  - Enter the connect script for establishing a connection to the TNC. See \cf4\strike Packet Connection Scripts\cf2\strike0\{linkID=180\}\cf0  for instructions.
\par \cf3 
\par \b\i Script Inactivity Timeout\cf0\b0\i0  - Set the maximum number of seconds allowed between steps in the connection script. 
\par 
\par \cf3\b\i Do a full TNC configuration only on first use\cf0\b0\i0  - If this box is set the TNC on any given channel will only be configured the first time it is used after starting Paclink. This will speed up starting a connection on subsequent connections. \i DO NOT CHECK THIS BOX IF YOU USE THE SAME TNC WITH OTHER PROGRAMS WHILE Paclink IS RUNNING, IF YOU USE THE SAME TNC ON OTHER CHANNELS WITH DIFFERENT SETTINGS, OR IF YOU USE A RESTORE FILE.
\par \cf3\i0 
\par \b\i TNC Configuration File\cf0\b0\i0  - Select a TNC initialization file for the type of TNC being used. Normally the default file is sufficient. See \cf4\strike TNC Initialization File\cf2\strike0\{linkID=170\}\cf0  for more details. TNC initialization files permit customization if required and allow supporting different TNC firmware revisions. If you need to create a custom initialization file do so by copying and renaming then editing one of the example files (.aps extension required) to avoid any problems with file corruption during auto updates.  KISS mode TNCs using Paclink's \cf4\strike native KISS driver \cf2\strike0\{linkID=390\}\cf0 still require a simple configuration file. The example .aps files should work for 1200 and 9600 baud but may be edited to handle any required parameter changes or tweaks.
\par \cf3 
\par \cf1\b\fs28 Optional VHF/UHF Radio Control\cf0\b0\fs20 
\par If you have a modern transceiver it is possible to use the same automatic radio control as is available on HF channels. This is optionally set up in this control group. Radio control is not possible using AGWPE channels or if using the internal KISS TNC with the Kenwood TS-2000.
\par 
\par \cf3\b\i Control Mechanism\cf0\b0\i0  - If not using Radio control for VHF/UHF Packet select Manual(none). If using at PTC II, PTC IIpro or PTC IIusb TNC you can select via the PTC II or direct via Serial port. If using the PTC IIpro or PTC IIusb select TTL or RS-232 levels.  
\par 
\par \cf3\b\i Baud Rate\cf0\b0\i0  - Select the radio control baud rate...this must match the radio settings. This does not have to be the same as the TNC serial baud rate above.\cf3 
\par \cf0 
\par \cf3\b\i Serial Port\cf0\b0\i0  - Select the serial port used to control the radio. This will only be enabled for Direct Serial control.\cf3 
\par \cf0 
\par \cf3\b\i Radio Model\cf0\b0\i0  - Select the radio model. Currently only those models in the radio model drop down list are supported.\cf3 
\par \cf0 
\par \cf3\b\i Radio Address\cf0\b0\i0  - For Icom radios set the 2 hex digit address here that matches the radio address setting.\cf3 
\par \cf0 
\par \cf3\b\i Channel Freq in MHz\cf0\b0\i0  - Enter the channel frequency in MHz for the channel. e.g. 145.090  This will be set in the radio and the mode switched to FM upon initiating the Packet connection.\cf3 
\par \cf0 
\par \cf4\strike Overview\cf2\strike0\{linkID=10\}\cf0 
\par \cf4\strike Channels\cf2\strike0\{linkID=110\}
\par \cf4\strike Telnet Channels\cf2\strike0\{linkID=120\}\cf0 
\par \cf4\strike HF Pactor Channels\cf2\strike0\{linkID=150\}
\par \cf4\strike WINMOR Channels\cf2\strike0\{linkID=160\}\cf0\f1 
\par \cf4\strike\f0 Customizing TNC Initialization Files\cf2\strike0\{linkID=170\}
\par \cf4\strike 
\par }
140
Scribble140
Packet AGW Channels
KISS operation;Packet AGW Channels;



Writing



FALSE
29
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Packet AGW Channels\cf0\b0\f1\fs20 
\par 
\par \f0 Use this dialog to set up AGW packet channels, accessed by clicking on Files|AGW Packet Channels... You may establish any number of AGW packet channels connecting either directly to an RMS site or Telpac site or over a UHF/VHF network using the optional connect script.
\par 
\par \pard\qc\cf2\{bmc Packet AGW Channels.bmp\}
\par \pard 
\par \cf3\b\i Channel Name\cf0\b0\i0  - Enter a channel name. It should be descriptive enough to make the channel easily identified. If viewing or editing an existing channel use the drop down box to select an existing AGW packet channel.
\par \cf2 
\par \cf3\b\i Remote Callsign\cf0\b0\i0  - Enter the call sign of the remote RMS or Telpac site this channel is to connect to.
\par \f1 
\par \cf3\b\i\f0 AGW Engine Port\cf0\b0\i0  - This is the AGWPE port as set up on the Packet Engine.  If the AGW PE is installed on the same computer as Paclink then all installed ports should be available on the drop down list. If the AGW PE is on a remote computer then you can connect to and display the ports on that computer by clicking the Remote Port Info button. Important \i Note:  If you change the number or configuration of ports in your AGWPE setup you may have to go back and update the port selection in all AGW channels.
\par \i0 
\par \cf3\b\i Channel Priority\cf0\b0\i0  - The default priority for packet channels is 3. If you want change the order in which channels are tried you may set the priority between 1 (highest) to 5 (lowest).
\par \cf3 
\par \b\i Channel Enabled\cf0\b0\i0  - If this box is unchecked then Paclink will not attempt to use this channel.
\par \cf3 
\par \b\i Activity Timeout\cf0\b0\i0  - Set the number of minutes the channel should wait before disconnecting due to channel inactivity.
\par \cf3 
\par \b\i Packet Length\b0\i0  \cf0 - Set the packet length (32 - 256) here. Under good conditions larger packet length will improve throughput. (Default = 128). If trying to use a radio's internal modem (usually Tasco chips) try small values of packet length (32 - 128) or use Paclink's \cf4\strike native KISS driver\cf2\strike0\{linkID=390\}\cf0 .
\par \cf3 
\par \b\i Max Frames Outstanding\cf0\b0\i0  - This sets the maximum number of outstanding frames (1-7) Larger numbers may improve throughput but may hog the packet channel in some networks. (Default = 2)  If using  a radio's internal modem try using low values 1 or 2.
\par \cf3 
\par \b\i Optional Connect Script\cf0\b0\i0  - Enter the connect script for establishing a connection to the TNC. See \cf4\strike Packet Connection Scripts\cf2\strike0\{linkID=180\}\cf0  for instructions.
\par \cf3 
\par \b\i Script Inactivity Timeout\cf0\b0\i0  - Set the maximum number of seconds allowed between steps in the connection script. If there is no script or the script is only one line (simple Via connect) this is the time limit to make a connection. For direct connections (no script) this also is the connection timeout.
\par \f1 
\par }
150
Scribble150
HF Pactor Channels
HF Pactor Channels;Radio Control;



Writing



FALSE
75
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fmodern\fcharset238{\*\fname Arial;}Arial CE;}{\f3\fmodern\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red0\green128\blue0;\red0\green255\blue0;\red255\green0\blue0;\red0\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 HF Pactor Channels\cf0\b0\f1\fs20 
\par 
\par \f0 Use this dialog, accessed by clicking on Files|HF Pactor Channels... You may establish any number pactor channels to connect directly to an RMS over a HF radio channel.
\par 
\par \pard\qc\cf2\{bmc Pactor TNC Channels.bmp\}\cf0 
\par \pard 
\par Configuring Pactor Channels is done with the Pactor TNC Channels menu above and the \cf3\strike Pactor Connect Menu\cf2\strike0\{linkID=190\}\cf0 . Some parts of the Pactor Channels menu may change depending on which TNC is selected.
\par 
\par \cf1\b\fs32 PACTOR Channel Settings\fs20 
\par \cf4\b0 
\par \cf5\b\i Channel Name\cf0\b0\i0  - Enter a channel name. It should be descriptive enough to make the channel easily identified. If viewing or editing an existing channel use the drop down box to select and existing TNC pactor channel.
\par 
\par \cf5\b\i Remote Callsign\cf0\b0\i0  - For Pactor channels the remote call sign may be selected here. The drop down list shows all available MARS or Public RMSs.  If the optional EMCommPMBOs.txt file is included in the Paclink\\Documentation directory those call signs will be appended to the Public PMBO list. The call sign may  also be set/modified using the \cf3\strike Pactor Connect Menu\cf2\strike0\{linkID=190\}\cf0  menu for manually forwarded channels. 
\par \cf5 
\par \b\i Channel Priority\cf0\b0\i0  - The default priority for pactor channels is 5. If you want change the order in which channels are tried you may set the priority between 1 (highest) to 5 (lowest). Normally HF channels have the lowest priority (highest number).
\par 
\par \cf5\b\i RF Center Freq(kHz)\cf0\b0\i0  - This drop down list will contain the available frequencies for the selected RMS. The frequency may also be set/modified using the \cf3\strike Pactor Connect Menu\cf2\strike0\{linkID=190\}\cf0  menu for manually forwarded channels. Frequencies marked with a trailing " (p3)" designation are Pactor 3 only channels and will not accept Pactor 1 or 2 connections. Pactor 3 only frequencies will not be accepted if a Pactor 1 only modem type is selected.
\par 
\par \cf5\b\i Channel Enabled\cf0\b0\i0  - If this box is unchecked then Paclink will not attempt to use this channel.
\par 
\par \cf5\b\i Pactor ID Enabled\cf0\b0\i0  - Will transmit a Pactor 1 FEC-mode station ID (de <Call sign> ) at the end of each link or attempted link if checked. This is required on the amateur frequencies. Some delay (up to 15 seconds) is normal during FEC ID while some modems have to wait for a clear channel indication and to insure proper FEC shutdown.
\par \cf5 
\par \b\i Autoforward Enabled\cf0\b0\i0  - If this box is checked then this Pactor channel may be used to automatically connect to a distant RMS. \i THIS SHOULD ONLY BE USED IN EMERGENCY SITUATIONS. AUTOMATIC FORWARDING FOR HAM APPLICATIONS MAY BE RESTRICTED TO CERTAIN AUTO FORWARD SUBBANDS ON HF. NORMALLY LEAVE THIS UNCHECKED.\i0   For PTC II type TNCs there is a mechanism to search for a clear channel (as defined by the PTC IIs DSP based busy detector) before making an automatic connection. There must be a continuous period of at least 6 seconds of clear channel detected before a connection attempt is made. If no clear channel is found in 30 seconds the automatic connection attempt is aborted and the channel is closed.
\par \cf5 
\par \b\i Enable Busy Channel Hold\cf0\b0\i0  - This box is checked by default. If it is necessary to disable the busy channel detection transmit lock then this box should be unchecked. Unchecking this box while using automatic forwarding is only authorized on non-amateur frequencies.
\par \cf5 
\par \b\i Activity Timeout\cf0\b0\i0  - Set the number of minutes the channel should wait before disconnecting due to channel inactivity. The remote server may shut down a poor link before this timeout.
\par \cf5 
\par \cf1\b\fs32 TNC Settings\b0\fs20 
\par \cf5 
\par \b\i TNC Type\cf0\b0\i0  - Select the type of TNC used by this channel.  If you have a KAMXL and have problems with Pactor try using the Simple terminal to do a \f2 RESTORE DEFAULTS\f3 . \f0 (Note: If using the PK232 you must have firmware revision 7.1 or later)
\par \cf5 
\par \b\i Audio Tones Center\cf0\b0\i0  - Select the average (center) of the Mark and Space tones (Hz). Normally 1500 is a good setting. PK-232 TNCs have a fixed setting and cannot be adjusted. For PTC IIs using Pactor 3 the value 1500 \i must\i0  be used.
\par \cf5 
\par \b\i Serial Port\cf0\b0\i0  - Select the serial port used to connect to the TNC. (fixed setting 8N1)
\par \cf5 
\par \b\i Baud Rate\cf0\b0\i0  - Select the baud rate used to connect to the TNC. This is NOT the on-air radio baud rate. For PTC II TNCs a minimum baud rate of 38400 is recommended.  For other TNCs a minimum of 9600 baud is recommend.
\par 
\par \cf5\b\i Do a full TNC configuration only on first use\cf0\b0\i0  - If this box is set the TNC on any given channel will only be configured the first time it is used after starting Paclink. This will speed up starting a connection on subsequent connections. \i DO NOT CHECK THIS BOX IF YOU USE THE SAME TNC WITH OTHER PROGRAMS WHILE Paclink IS RUNNING OR IF YOU USE THE SAME TNC ON OTHER CHANNELS WITH DIFFERENT SETTINGS.
\par \i0 
\par \cf5\b\i Audio Drive Levels\b0\i0  \cf0  - If the TNC is an SCS model the audio drive levels can be specified here and will over ride any settings in the TNC Configuration File. Other Pactor TNC types require hardware (pot) adjustments of the drive level or settings in the configuration file depending on the model. Drive level should be set low enough that no ALC action is shown on most transmitters. Required drive levels are affected by the mike gain or auxiliary input gain/sensitivity of the transceiver.
\par \cf5 
\par \b\i TNC Configuration File\cf0\b0\i0  - Select a TNC initialization file for the type of TNC being used. Normally the default file is sufficient. See \cf3\strike TNC Initialization File\cf2\strike0\{linkID=170\}\cf0  for more details. TNC initialization files permit customization if required and allow supporting different TNC firmware revisions. If you need to create a custom initialization file do so by copying and renaming then editing one of the example files (.aps extension required) to avoid any problems with file corruption during auto updates.  
\par 
\par \cf1\b\fs32 Optional Radio Control\cf6\fs20 
\par \cf0\b0 
\par \cf5\b\i Manual (none\b0\i0 ) \cf0  - Select this if no radio control is desired or available.
\par 
\par \cf5\b\i Via PTC II, IIpro, IIusb\cf0\b0\i0  - If you are using these TNCs which have a radio control port you may select this option. You will need a custom cable interfacing the PTC II to the radio. For this option and direct serial port control set the baud rate that matches the setup of the radio control port. Check your radio manual to make sure of the correct radio control levels (TTL, RS232 or Icom C-IV)
\par 
\par \cf5\b\i PTC to Radio Levels\cf0\b0\i0  - If you are using a PTC IIpro or PTCIIusb models this control will allow selection of the control level to Kenwood and Yaesu radios. Most modern radios use RS232 levels but check your radio manual. \i Putting RS232 levels into a TTL radio port may damage the radio!\i0   The older PTC II models only support TTL levels so an external translator to RS232 levels may be required. ICOM radios use the ICOM C-IV bus which is open collector TTL levels.
\par 
\par \cf5\b\i Direct via Serial Port:\cf0\b0\i0  - Select this and the corresponding serial port if you are using a separate serial (or USB to Serial) port connected to the remote control port of the radio.  ICOM radios may require a special C-IV adapter. For ICOM radios you will have to enter the radio address (Hex) that matches the address set up in the radio.
\par 
\par \cf5\b\i Radio Model\cf0\b0\i0  - Select the radio model. If your model is not shown try the generic radio for your manufacturer (Kenwood, Icom, Yaesu) 
\par 
\par \cf5\b\i Radio Address\cf0\b0\i0  - Required on most ICOM radios.
\par 
\par \cf5\b\i Use LSB\b0\i0  \cf6 - USB is used by default. check this box on a channel where it is desired to use LSB.\cf0  Dial calculations for both LSB and USB are shown in the Pactor Connect menu. Pactor will work on either sideband and users and RMS sites do \i not\i0  have to use the same sideband.
\par 
\par \cf5\b\i Enable Narrow Filter.\b0\i0 .. \cf0  - Checking this options enables Paclink to change the radio filter to a narrow 500 Hz filter (if the radio is so equipped) on Pactor 1 or 2 connects. This can reduce adjacent channel interference in some cases. Pactor 3 connects always require wide (2.4 kHz or above)  filter settings.
\par 
\par \cf3\strike Establishing HF Connections\cf2\strike0\{linkID=190\}\cf0 
\par \cf3\strike Overview\cf2\strike0\{linkID=10\}\cf0 
\par \cf3\strike Channels\cf2\strike0\{linkID=110\}
\par \cf3\strike Telnet Channels\cf2\strike0\{linkID=120\}
\par \cf3\strike VHF Packet Channels\cf2\strike0\{linkID=130\}
\par \cf3\strike WINMOR Channels\cf2\strike0\{linkID=160\}\cf0 
\par \cf3\strike Customizing TNC Initialization Files\cf2\strike0\{linkID=170\}
\par \cf0\f1 
\par 
\par 
\par }
170
Scribble170
TNC Initialization Files
TNC Initialization Files;



Writing



FALSE
8
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\fs32 TNC Initialization Files\cf0\b0\f1\fs20 
\par 
\par \f0 Default TNC initialization files are provided for each TNC supported directly by Paclink. These files are normally sufficient for a typical Paclink installation. If your TNC has drive levels set by command you will probably have to edit those commands.  If you are using a TNC in Native KISS mode you may have to edit the example .aps files for some parameters. You do not have to add MYCALL or MPTCALL statements to the Initialization file or commands setting the MARK or SPACE frequencies. Those are all set automatically by the program.  If you need to make changes in the default settings you should copy the default file (normally found in C:\\Paclink\\Data) and give the copy a new meaningful name with a .aps extension. This is important since modifications to a default file may be overwritten when the program is automatically updated.  If you are going to edit or create a new .aps file be sure to have a correct manual (matching your TNCs firmware) with description of all the commands and their functions. It is a good idea to also comment the file as shown in the example to help document any changes or additions. 
\par 
\par All TNC Initialization files MUST be saved with a .aps file extension to the Paclink\\Data directory and should only be edited/saved in plain text.\f1 
\par }
180
Scribble180
Packet Connection Scripts
Connection Scripts;Digipeaters;



Writing



FALSE
34
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\fs32 Packet Connection Scripts\cf0\b0\f1\fs20 
\par 
\par \f0 For sequenced connections to the remote target station a connect script is used.  A script is entered in the packet channels dialog script text box.
\par 
\par No connect script is required for a direct connection to the target call sign. If a simple connection using digipeaters is desired the script can be a single line of the form:
\par 
\par \b\i C or CONNECT  <target call>   V or VIA  <digi 1>,<digi 2>
\par \b0 
\par \i0 Where \i <target call>\i0  is the remote call sign and  \i <digi n>\i0  are the digipeaters (up to two).  Examples:
\par 
\par \b\i C  W4RP VIA COA1, MLBDIG\b0\i0      (connect to W4RP via digipeaters COA1 then MLBDIG)
\par 
\par If a more complex script is required to make the connection it is entered in two-line pairs. The first script line is the connection command or data to be \i sent\i0  and the following script line being the \i expected\i0  text to sequence to the next command/data line.  Example:
\par 
\par \b C COA1 \b0   (Issues a connect request to Node COA1)
\par \b\i CONNECTED WITH STATION COA1\b0\i0  (is the expect text after a successful connection)
\par \b\i C W4RP-4\b0\i0   (is the text sent to the node asking the node to connect to W4RP-4)
\par \b\i CONNECTED TO W4RP-4\b0\i0  (is the expected text upon a connection to W4RP-4)
\par 
\par Since this is the last line of the script when the text \b\i CONNECTED TO W4RP-4\b0\i0  is received the script is ended and normal message exchange can commence. There is no absolute limit to the length of a script. The packet channel dialog will perform basic syntax checking of the connect script. All scripting is case insensitive and converted to upper case. When entering text on even lines (text used to compare to received text) make certain you do not \i OVER\i0  specify the text or include any text that would be transitory (dates, traffic counts, etc..). The text you enter must be contained (somewhere) \i exactly\i0  as typed in the received text to sequence to the next script line. Also if your last script line is looking for text reply from an automated WL2K RMS or Linux Gateway make sure the search text occurs \i before\i0  the SID of the remote station. SIDs look something like "[WL2K-3.0.48-B2FIHM$]" and are necessary to identify the protocol Paclink will use. The packet containing the search text in the last script line is also passed to Paclink's protocol processor. The main display shows script activity in green text.
\par 
\par When a connect script is active the channel also continuously monitors for KEY words that will terminate the script and abort the connection. These key words are:
\par 
\par \b DISCONNECTED
\par TIMEOUT
\par EXCEEDED
\par FAILURE
\par BUSY
\par \b0 
\par The connect script timeout above in the packet channels dialog is a time limit (default 60 seconds) for each level of scripting. A script line that takes longer than this to complete it will cause a timeout and abort the script and disconnect the link. Connect scripts are a powerful tool and can be used to transverse nodes, switches and packet backbones to reach distant stations but at a reduction in the throughput of the channel. Direct connection should be used whenever possible.
\par \f1 
\par }
190
Scribble190
Establishing HF Connections
HF Connections;



Writing



FALSE
54
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Establishing HF Connections\cf0\b0\f1\fs20 
\par 
\par \f0 There are some necessary steps to complete before trying to establish a HF Pactor or WINMOR Connection.
\par 
\par 1) Connect  your TNC or Sound card audio and PTT signals to the radio mike or auxiliary input jack. Use short, shielded cables with clamp-on ferrite chokes to minimize RF feedback.
\par 
\par 2) (optional but very convenient and important in EMComm work) Connect the TNC radio control output or direct serial port to the radio control port.
\par 
\par 3) Set the audio levels in the \cf2\strike HF Pactor Channels\cf3\strike0\{linkID=150\}\cf0  menu  or \cf2\strike WINMOR Channels\cf3\strike0\{linkID=160\}\cf0  , the TNC configuration .aps file, or in the TNC hardware to insure the transmitter is operating in the linear range. Normally there should be little or no ALC action showing on the transmitter. When sending large files or messages at high power the high duty cycle of Pactor or WINMOR can cause overheating on some radios. In general limit the audio levels to the following or lower based on an average reading watt meter:
\par 
\par \tab Pactor I  (50-60% of the rated PEP power  e.g. 50-60 watts for most amateur transceivers)
\par 
\par \tab Pactor 2 or 30 -50% of the rated PEP power with \cf4\b\i NO\cf0\b0\i0  ALC action. 
\par 
\par \tab WINMOR Two Tone Test: 50% of the rated PEP power with \cf4\b\i NO\cf0\b0\i0  ALC action
\par 
\par \pard\qc\cf3\{bmc Pactor Connect.bmp\}\cf0 
\par \pard 
\par                                                    The Pactor Connect dialog is a unique to Pactor connections. 
\par 
\par 
\par 
\par  
\par \pard\qc\cf3\{bmc WINMOR Connect.bmp\}\cf0 
\par \pard 
\par                                                    The WINMOR Connect dialog is a unique to WINMOR connections.
\par 
\par Since these HF connections are normally done manually these dialogs provide a fast and convenient way to select alternate RMS stations and frequencies.  On WINMOR channels since there is no physical TNC a virtual TNC "panel" is shown during operation. This TNC panel has no user controls or menus and is just for display. See \cf2\strike HF WINMOR Channels \cf3\strike0\{linkID=160\}\cf0 
\par 
\par \cf4\b\i Channel Status\cf0\b0\i0  - The channel status indicator shows if the channel is active. For Pactor this is only functional with PTC II type modems but gives an indications of other Pactor activity on the frequency. WINMOR Has a busy channel detector implemented in the DSP modem. Be certain to listen for a clear frequency \i BEFORE\i0  making a connection attempt. For PTC II type TNCs or WINMOR the background will be green when a clear channel is detected and red when busy. Wait for the channel to clear or select another frequency before connecting.
\par 
\par \cf4\b\i Remote Call Sign\cf0\b0\i0  - This drop down list selects the remote call sign for the Pactor or WINMOR connection.  The drop down list shown is dependent on the site's call sign (MARS or HAM). For Ham calls the EMComm RMS sites will be added to the Public drop down list if the optional EMCommRMSs.txt file installed in your Paclink\\Documentation directory.  A header label over the call sign and frequency drop down lists identifies MARS, Public or EMComm type RMS sites. You may also enter a specific call sign directly into this field though this is normally only needed for testing. The call sign must be a valid ham or MARS call sign. Full details on any RMS can be viewed using the Help|Documentation menu and viewing the appropriate list.
\par 
\par \cf4\b\i RF Center Frequency (kHz)\cf0\b0\i0  - This drop down list shows which frequencies are supported \i by the selected RMS Remote Call Sign \i0 above. Frequency may also be entered directly and is always expressed in kHz and decimal .  Pactor 3-only frequencies are designated by a " (p3)" designator after the frequency. Pactor 3 only frequencies will not be accepted if a Pactor 1 only modem type has been selected for the channel. WINMOR frequencies are designated by a "w1", "w2" or "w3" tag to the frequency indicating a 200Hz, 500Hz or 2000Hz WINMOR channel.
\par 
\par For Pactor the frequencies in the drop down list are the center (published) frequencies and the dial settings for upper sideband are offset by the Audio Tone Center as set up in the Pactor TNC Menu. If you have configured the Pactor or WINMOR Channel for automatic radio control then selecting or entering a frequency will automatically and conveniently set the radio to that frequency. 
\par 
\par For both Pactor and WINMOR the operation is always USB and the calculated USB Dial frequency will be below the published center frequency. 
\par 
\par \i IT IS EXTREMLY IMPORTANT TO LISTEN FIRST ON THE DESIRED FREQUENCY TO INSURE IT IS CLEAR BEFORE TRYING TO CONNECT!\i0   The channel activity indicator is active \i ONLY\i0  for PTC II type TNCs and WINMOR but it is NOT always able to detect weak or non-pactor/non-WINMOR signals. For other Pactor TNCs it will remain yellow and channel activity must be determined solely by listening.
\par 
\par \cf4\b\i Bandwidth (Hz)\cf0\b0\i0  - On the WINMOR connect menu there is an additional Bandwidth drop down box allowing selecting either 200, 500 or 2000Hz bandwidth modes. Normally this is set automatically from the Center Frequency drop down list however it is possible to set this to a different value. \i Note however that WINMOR enabled RMS HF stations will probably accept a connection request ONLY for the published WINMOR bandwidth (w1=200Hz, w2=500Hz, w3=2000Hz).
\par \i0 
\par \cf4\b\i Connect Button\cf0\b0\i0  - This button starts the actual connect sequence. Before clicking this make SURE the frequency is clear. Remember it is often possible there could be an existing session running very near the noise level. Also remember that in amateur radio we share the spectrum so other modes (CW, SSB Voice, PSK31, etc..) could be present. \i Frequency or call sign changes made on the Connect form are temporary and will NOT alter the default call sign and frequencies set up in the channel menu. Use the appropriate channel menu to make these permanent changes. 
\par \i0 
\par \cf4\b\i Close Button\cf0\b0\i0  - This button will close down the channel. If a channel's forwarding session (automatic or manual) completes normally the channel is automatically closed.
\par 
\par \cf2\strike HF Pactor Channels\cf3\strike0\{linkID=150\}
\par \cf2\strike WINMOR Channels\cf3\strike0\{linkID=160\}\cf0 
\par 
\par 
\par }
200
Scribble200
CMS/RMS Polling
Polling;



Writing



FALSE
16
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 CMS/RMS Polling\cf0\b0\f1\fs20 
\par 
\par \f0 Paclink must explicitly connect to a CMS Telnet Server or an RMS site via radio. This can be done manually or by automatically polling at specified intervals. When a poll is requested Paclink will attempt to connect using each of the channels configured (and not disabled) in order of priority until a successful exchange of messages is accomplished.
\par 
\par HF channels are a special case on amateur frequencies where manual intervention is required since fully automatic operation is not normally permitted. Automatic polling is not restricted on MARS channels where MARS policy permits it.
\par 
\par Use this dialog box, accessed by clicking on Files|Polling Interval... to set any automatic operation.
\par 
\par \pard\qc\cf2\{bmc Polling Intervals.bmp\}\cf0 
\par \pard 
\par \cf3\strike Overview\cf2\strike0\{linkID=10\}\cf0 
\par \cf3\strike Channels\cf2\strike0\{linkID=110\}\cf0 
\par \f1 
\par }
210
Scribble210
Message Limitations
Limitations;



Writing



FALSE
15
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\fs32 Message Limitations\cf0\b0\fs20 
\par \f1 
\par \f0 Messages sent via Paclink have some limitation that are not imposed on normal Internet mail.
\par 
\par These messages are subject to being passed over radio channels with limited capacity and limited bandwidth. Messages should be kept as short as reasonable without compromising their intent or meaning. The total number of bytes in a message (body and attachments) should be held to less than 100,000 bytes if at all possible. If a message is received from an email client that is too large  (120 K bytes compressed size) it is immediately rejected so the sender receives an error message showing the size is too large.
\par 
\par The content of messages must be within the legal limits set by the FCC rules (US)  or other national administrations if the message is to pass over amateur radio channels. The content of messages must conform to MARS policies if they are to pass over MARS channels.
\par 
\par Because messages may pass over radio channels they may not be completely private. In addition RMS and CMS site sysops have access to all messages handled both to insure acceptable content (especially on amateur channels) and for troubleshooting when there are delivery problems. Messages transmitted over amateur frequencies may not be encrypted for the purpose of obscuring their content. Messages passed over MARS channels may be encrypted where permitted by MARS policies.
\par 
\par When a user's client email program does an SMTP link to Paclink to send a message the message is received, parsed, and sanity checked before the acceptance is completed. In this way only messages that are certain to be handled by Winlink are accepted into the system. In all other cases an error message is returned to the email client so the sender can immediately be aware of any problems. Reasons for rejection can be message too long (a compressed size of > 120K bytes), an invalid sender's address, too many recursions to decode the mime, unacceptable attachment file extensions, and any other cause for a failure to parse the offered message. Note compression is a function of file type: .txt, .rt,  .doc, and .xls compress typically 2:1 or more. Images like .jpg or .gif usually compress very little.
\par 
\par }
220
Scribble220
Logs
Logs;



Writing



FALSE
16
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Logs\cf0\b0\fs20 
\par \f1 
\par \f0 There are two types of logs possible with Paclink; both types are found in the ..\\Logs subdirectory. You may access any of these logs by clicking on Logs|View.
\par 
\par The most used log is the \i Channel Events <date>.log.\i0  This records normal events as they occur within the Paclink programs. Each event carries a date/time stamp and a description of the event. This log is archived at the end of each 24 hour period and a new log started.
\par 
\par \i Exceptions <date>.log\i0  is another type of log that may be created if a exception event occurs indicating an unexpected event has occurred. This file should be checked first if Paclink exhibits any unexpected behavior. This log is archived at the end of each 24 hour period and a new log started.
\par 
\par \cf2\b\i Temporary Debug Logs:
\par \cf0\b0\i0 
\par During the debug of the new WINMOR protocol a debug log (selected in the WINMOR channel dialog; default = enabled) is generated for extended debugging and analysis. The log is named \i WINMORdebug<date>.log\i0  and is found in the ..-\\Logs subdirectory.
\par 
\par During the debug of the new Native KISS driver a debug log \i NativeKISSax25PktLog.log\i0   will be generated automatically. This log is cleared upon a new connect. If problems with the NativeKISS driver are encountered post the log to the WINMOR or Paclink reflectors.
\par }
230
Scribble230
Troubleshooting
Troubleshooting;



Writing



FALSE
20
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Troubleshooting\cf0\b0\fs20 
\par \f1 
\par \f0 Paclink is a complex program with many other interfaces (3rd party software, TNCs from various manufacturers, etc..). Most problems are the result of improper setup or incompatible hardware or TNC firmware revisions. Troubleshooting is a learned art and begins with some basic concepts and important steps.
\par 
\par 1)  Minimize the unknowns! When starting with an installation minimize the number of unknowns and take small steps to install, test and verify each component.  For example if installing Paclink with a new TNC and the AGW Packet engine first confirm your computer can talk to the TNC using a dumb terminal program. Make sure the TNC is in the correct mode and baud rate. Then try to install and configure the AGW Packet Engine. Test your AGW Installation with another simple AGW program like AGW Term to confirm it talks to the TNC and can make a packet connection. \i THEN\i0  once you have verified the above and eliminated possible problems in the TNC/AGWPE installation try to configure Paclink and set up your AGW Packet Channel.
\par 
\par 2) When seeking help completely identify your installation. Paclink revision, TNCs used and firmware revs, Connect scripts if used, types of serial ports used, Computer OS and version. In extreme cases you can also export your \cf2\lang1024\strike Paclink.ini file\cf3\strike0\{linkID=260\} \cf0\lang1033 and that file will contain all your settings. The more \i pertinent\i0  information you can supply the easier it is to debug a problem.
\par 
\par 3)  If at all possible try and duplicate the problem and identify the settings or steps that can cause it to happen. If you can duplicate the problem be sure and supply the logs for the channel and time...it contains important debugging information to the trained observer.
\par 
\par 4) Describe your problem clearly and post information about it on one of the \cf2\lang1024\strike Winlink 2000 forums\cf3\strike0\{linkID=300\}\cf0\lang1033 . There may be others that have seen and solved the problem or be able to help you solve it.
\par 
\par 5) Supply all necessary but not extraneous information. For example if you have captured a particular failure or problem in a log file.  Use a plain text editor to "snip" that \i section\i0  of the log and possibly add your own comments or observations.  Send some of the log file leading up to and after the error. Sending a large log file with 99% extraneous information is not courteous and will only slow down the debugging process. In general log files are more efficient and supply more information than screen captures of a particular problem or menu.
\par 
\par 6) Have patience! Paclink and the Winlink 2000 system are all volunteer efforts. We try to help as much as possible but don't expect 24/7/365 free support. When YOU become the expert try and help out others that are learning by joining one of the \cf2\lang1024\strike Winlink 2000 forums\cf3\strike0\{linkID=300\}\cf0\lang1033 .
\par \cf2\lang1024\strike 
\par Contacts\cf3\strike0\{linkID=300\}\cf0\lang1033 
\par }
240
Scribble240
Implementation
Implementation;



Writing



FALSE
27
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Implementation
\par \cf0\b0\f1\fs20 
\par \f0 Paclink is written in Microsoft VB.NET 2005.  Paclink is compiled in x86 mode to avoid possible problems with 64 bit computers. The source code is not released for public distribution.\f1 
\par 
\par \f0 The program runs three primary threads; processing messages from Winlink, processing messages from client email programs (SMTP), and delivering messages to client email programs (POP3).
\par 
\par Approximately every three seconds the program's main thread tests the "From Winlink" subdirectory for messages. If any are found then each message is passed to the Paclink message processor that reads the message, places copies of the message in the subdirectory for each account found in the message's address fields, and then removes the message from the "From Winlink" directory. Processing errors are recorded in the "Paclink Errors.log" file. In all cases the message is removed from the "From Winlink" subdirectory.
\par 
\par An SMTP port is open and listening for SMTP connection requests from email clients. Up to 10 links may be serviced at a given instant. The client forwards one or more messages during a link and as they are processed by an instance of the message processor they are placed in the "To Winlink" subdirectory.
\par 
\par A POP3 port is open and listening for POP3 connection requests from email clients. Up to 10 links may be serviced at a given instant. Once a link is established the program passes any pending message for the client using the POP3 protocol and the closes the link.
\par 
\par As messages pass through Paclink unique message IDs are added to the "\i Mids Seen.dat\i0 " file (found in the \\Data subdirectory). This data is used to prevent duplicate messages from passing through the system.
\par 
\par TNC drivers are all written in Host mode for improved performance and reliability:
\par 
\par \tab Kantronics TNCs use Kantronics host mode
\par \tab SCS PTC II TNCs use CRC Extended Host (W8DED compatible) Mode
\par \tab AEA/Timewave and TNC2 Clone TNCs use W8DED Host Mode
\par \tab The AGW Packet Engine interfaces to all TNCs in KISS mode
\par \tab The Native KISS drivers interface to most TNCs in KISS mode and do not require 3rd party software.
\par 
\par Paclink uses the \cf1\strike B2F\cf2\strike0\f2\{link=*! \f3 ExecFile("http://www.winlink.org/B2F.htm")\} \cf0\f0 forwarding protocol which is an extension of the popular \cf1\strike FBB\cf2\strike0\f2\{link=*! \f3 ExecFile("http://www.f6fbb.org/")\}\cf0\f0  forwarding protocol. The B2F protocol supports multiple addresses, mixed amateur, MARS and internet email addresses and attachments. The compressed binary B2F protocol offer a degree of security since it is difficult to capture and decode by a casual observer however per amateur rules there is no encryption of messages sent over radio links of the Winlink 2000 system.
\par \f1 
\par }
250
Scribble250
Directories and Files
Directories;Files;



Writing



FALSE
18
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\fs32 Directories and Files\cf0\b0\fs20 
\par \f1 
\par \f0 Paclink uses a number of subdirectories to hold executables, data, messages, and documentation.
\par 
\par    <Root Paclink Directory - default C:\\Paclink\\..>
\par       ..\\Bin - Holds executables and the Paclink.ini settings file
\par       ..\\Data - Holds configuration data (.aps and .script files)
\par       ..\\Documentation - Holds revision history, program info and RMS lists.
\par       ..\\Channels - Holds channel configurations
\par       ..\\Accounts - Holds individual account subdirectories
\par       ..\\From Winlink - Holds mime files received from Winlink
\par       ..\\To Winlink - Holds mime files to send to Winlink
\par       ..\\Logs - Holds event, error and trace logs
\par       ..\\Help - Holds help file
\par 
\par }
260
Scribble260
Paclink Settings
Multiple Paclink Instances;Paclink Settings;



Writing



FALSE
16
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Paclink Settings\cf0\b0\fs20 
\par \f1 
\par \f0 All Paclink settings are stored in the ini file "Paclink.ini" located in the execution (\\Bin) directory. This plain text file holds all configuration and channels settings except script files which are held in the ..\\Data directory under the channel name with a ".script" extension.  With the setup menus and utilities supplied below you should never have to edit the Paclink.ini file.  While the ini files are readable do not attempt editing these files. An error in syntax will render the file useless requiring re setting up from scratch. The menus provide all the tools needed to edit parameters saved in the ini files.
\par 
\par There are two operations that can be done to the Paclink settings using the File drop down menu of the main form.
\par 
\par \cf2\b\i 1) Backup INI file -\cf0\b0\i0  This creates a copy of the current Paclink.ini file which saves all settings for the program (excluding script files mentioned above) to a user specified file.  This file can be used for debugging (it contains the entire setup information).  It can also be used as a safe restore point or to aid in setting up similar multiple Paclink installations easily (cloning). If you are having problems with setting up Paclink using this option is a good way to create a file of all the Paclink settings which can be then sent to others for analysis or comment. \cf3\strike Troubleshooting\cf4\strike0\{linkID=230\}\cf0 
\par 
\par \cf2\b\i 2) Restore INI file -\cf0  \b0\i0 This takes a previously saved INI file (as in 1) above) and uses it to restore or set Paclink to that state and settings. It is used primarily for recovery or as an aid in setting up multiple duplicate or similar Paclink installations. \i Use with caution as existing settings will be erased (but are automatically saved to a backup file Paclink.ini.bak).
\par \i0  
\par If you wish to start Paclink with a clean start (all default settings) simply shut down Paclink. Then delete or rename the file "Paclink.ini" in the execution (\\Bin) directory and then restart Paclink.  You may setup and run \cf3\lang1024\strike multiple Paclink Instances \cf4\strike0\{linkID=30\}\cf0\lang1033 on the same computer as long as they operate in different directories.
\par 
\par \cf1\lang1024\b\fs32 
\par }
270
Scribble270
Message Routing
Message Routing;



Writing



FALSE
18
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\fs32 Message Routing\cf0\b0\f1\fs20 
\par \f0 
\par The routing of messages to and from users is handled primarily by the Winlink 2000 CMS sites. A message can arrive at a CMS site from Internet or from an RMS site. The CMS reads the addresses in the message and routes the message to Internet mail addresses or to other RMS sites as required.
\par 
\par The CMS maintains a record of each RMS site used by a Paclink user account in the past 90 days. When a message arrives at a CMS site for a Paclink user account it is immediately posted for pickup by all of the RMS sites used by that user account. This usually takes well under 10 minutes.  RMS sites make an on-demand connection directly to a CMS upon RF connection so there is no latency and no routing issues using RMS stations.
\par 
\par There are some shortcuts to this process. A message between two users accessing the \i same\i0  Paclink instance will be delivered within Paclink and does not wait for an RMS connection. 
\par 
\par When a user sends a message the paths used to deliver messages are established dynamically and remembered by the system for 90 days. The message that establishes routing may be to any addressee or even to a non-existent addressee but \i until a message is sent\i0  a user account will not receive any messages.
\par 
\par If a new RMS site is accessed by Paclink then a new message must be sent by each user account to stablish a route thorough that PMBO. If messages are pending for a user at the time connection is made to a new PMBO site those messages are routed to the new site within a few minutes. Upon connecting to the new site again all pending messages will be forwarded. Since RMS stations make an immediate on-demand connection to a CMS there is no issues with establishing a route. 
\par 
\par Message delivery within Winlink 2000 is very fast and rarely takes more than a few minutes once a connection is made to an RMS site. There are occasional longer delays for message from or to Internet mail addresses. These delays are outside of the control of Winlink 2000 and can be hours but are typical of delays between any two Internet mail servers.
\par 
\par 
\par }
280
Scribble280
Whitelist
Whitelist;



Writing



FALSE
28
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red0\green128\blue0;\red128\green0\blue0;\red0\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 The Whitelist\cf0\b0\fs20 
\par 
\par Winlink 2000 maintains a "whitelist" of all Internet email addresses a user account has \i sent\i0  a message TO. This list is used to filter all messages from Internet addressed to amateur Paclink user accounts. The whitelist does NOT filter messages between Winlink 2000 users - only messages entering Winlink from Internet email sources. The primary purpose of the whitelist feature is to eliminate spam or any other unwanted messages from being accepted by Winlink 2000 for the user account. Whitelist filtering is not used on MARS Paclink accounts.
\par 
\par Messages to amateur call signs from Internet email sources that include "//WL2K " in the subject line bypass whitelist checking and will be passed through to the addressee.
\par 
\par An entry is made to the whitelist for every Internet email address the user account uses in a message. Alternatively, a user account may send a message to the Winlink 2000 system adding addresses to the list that will be accepted or that are NOT to be accepted. These entries may be by a complete address or by domain name only.
\par 
\par To add these entries to the whitelist address a message from the user account to "SYSTEM@Winlink.org" with a subject of "WHITELIST".  In the body of the message you may make any number of the following types of entries:
\par 
\par Accept: <address>
\par Reject: <address>
\par Delete: <address>
\par 
\par Where <address> can be a full address such as "John.Smith@cfl.rr.com" or it can be a domain name only such as "cfl.rr.com" or just "rr.com". If only the domain name is give then the command will apply to all messages from users at that domain. Do NOT make entries with a leading dot, containing an @ unless it is a full address, or containing a *.
\par 
\par You will received an acknowledgment message to messages addressed to "SYSTEM@Winlink.org" confirming any changes that have been made.
\par 
\par 
\par \cf2\b\i Other Anti Spam features:\cf0\b0\i0 
\par 
\par Winlink also uses an additional SPAM filtering function in the subject line of the message. If the subject line does not include the Text ("//WL2K " for  ham call signs or "//MARS " for MARS call signs and the sender is not in the whitelist the message will be rejected with a short link in the reply to direct the sender to a web site for explanation. This mechanism is part of the \cf3\strike Message Precedence \cf4\strike0\{linkID=360\} \cf5 used in WL2K\cf4 .\cf0 
\par 
\par 
\par \cf3\strike Whitelist Notices\cf4\strike0\{linkID=210\}\cf0 
\par }
290
Scribble290
Addressing Messages
Addressing Messages;Message Precedence;



Writing



FALSE
32
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Addressing Messages\cf0\b0\f1\fs20 
\par 
\par \f0 When you send a message from your email client program to Paclink pay special attention to the addresses you use. A message to someone that is going to receive the message via ham radio via the Winlink 2000 system is addressed in this way:
\par 
\par <call sign>@Winlink.org   
\par 
\par Example: W1AW@winlink.org (case insensitive). The address can be typed directly in the To or Cc fields (separated by a "," or ";") or from the address book of the email client.  NOTE: Paclink and the Winlink 2000 system will not accept mail with blind carbon (Bcc) copies.
\par 
\par Tactical addresses if used are always of the form: <tactical address>@winlink.org  
\par Example: ARCMLB-4@winlink.org (case insensitive)
\par 
\par Other \i Internet\i0  email addresses are exactly the same as they would be using any other mail account.  Example: JohnDoe@aol.com.
\par \b 
\par \cf2\i An important note about addresses:\cf0 
\par 
\par \b0 Incorrect addresses will normally result in a bounce message to you.  To be delivered a call sign or tactical address must be \b known\b0  to the Winlink 2000 system (that call sign or tactical address is a registered user). If you send a message and receive a "bounce" reply stating the message could not be delivered it is probably due to one or more of the following:
\par 
\par \pard\fi-360\li1080\tx1080 1)   The radio user or tactical address is not known to the WL2K system.
\par 
\par \pard\fi-360\li1080 2)   The call sign, tactical address  or email address was misspelled. Close does not count! The letter "O" is not the same as the numeral "0".
\par 
\par 3)   There was no such Internet address or the syntax was incorrect.
\par 
\par 4)   There was a problem with the mail server at the destination address or it rejected your message for some reason.
\par 
\par \pard\cf2\b Message Precedence (Priority): 
\par  \cf0\i0 
\par \b0 Message precedence priority may be set for MARS site installations using Paclink's  \cf3\strike Message Precedence mechanism\cf4\strike0\{linkID=360\}
\par \f1 
\par }
300
Scribble300
Contacts
Contacts;



Writing



FALSE
32
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil Arial;}{\f1\fnil\fcharset0 Arial;}{\f2\fnil\fcharset0 Courier New;}{\f3\froman\fcharset0 Times New Roman;}{\f4\fnil Courier New;}{\f5\fmodern\fcharset238{\*\fname Arial;}Arial CE;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red128\green0\blue0;\red0\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\f0\fs32 Contacts\cf0\b0\fs20 
\par 
\par \f1 Paclink is written, tested and supported by volunteers. Try and be as efficient as possible in communicating and requesting support. If you join one of the support groups you can help others by exchanging information and helping new users. The following are contacts:
\par \pard\tx4140\cf2 
\par \pard\sb100\sa100\b\i Winlink Web\cf0\b0\i0  -\f2  \f1 The \cf1\strike Winlink 2000 Web site \cf3\strike0\{link=*! ExecFile("http://www.winlink.org/")\} \cf0 set up by \f3\fs24 Lor Kutchins, W3QA
\par \pard\f1\fs20  should be your first stop for anything Winlink.  Lots of news, info and downloads.\f2  
\par \pard\tx4140\cf2\f1 
\par \b\i PaclinkMP Reflector\cf0\b0\i0  - This is your best bet for info and help during the beta testing of Paclink.  Go to the Yahoo \cf1\strike PaclinkMP\cf3\strike0\{link=*! ExecFile("http://groups.yahoo.com/group/PaclinkMP/")\} \cf0 group for more info or to join.
\par \pard\cf2\b\i 
\par WINMOR Reflector\cf0\b0\i0    WINMOR is in the initial testing phases and so will have its own group for exchange of problem and troubleshooting info.  Go to the \cf1\strike WINMOR reflector \cf3\strike0\f4\{link=*! \f2 ExecFile("http://groups.yahoo.com/group/WINMOR")\} \cf4\f1 for more info or to join.\cf0 
\par \pard\tx4140\cf2\b\i  
\par WL2KEmComm Reflector\cf0  \b0\i0 - This group is focused on Emergency applications of Paclink and WL2K. Lots  of experience and examples can be found in this large forum.  Go to the Yahoo \cf1\strike WL2KEmComm\cf3\strike0\{link=*! ExecFile("http://groups.yahoo.com/groups/wl2kemcomm/")\} \cf0 group for more info or to join.
\par \pard\f2 
\par \cf2\b\i\f1 "Winlink for Dummies"\cf0\b0\i0\f2  - \cf1\strike\f1 Loading WL2K User Programs\cf3\strike0\{link=*! ExecFile("http://groups.yahoo.com/group/LOADING_WL2K_USER_PROGRAMS/")\}\cf0   This group is specifically designed for those learning to install various client programs for the WL2K system. Info and help installing Paclink and AirMail may be found there.
\par \f2 
\par \cf2\b\i\f1 Winlink MARS\cf0\b0\i0  -\f2  \f1 The \cf1\strike Winlink 2000 MARS Deployment Help Group\cf3\strike0\{link=*! ExecFile("http://groups.yahoo.com/group/mars_wl2k/")\} \cf0 is set up to assist those MARS members who wish to deploy Winlink 2000 in the MARS Service.\f2  
\par 
\par \cf2\b\i\f1 Authors\cf0\b0\i0  - Paclink is written and maintained by:
\par 
\par    Vic Poor, W5SMM (SK)\cf3 
\par    \cf0 Rick Muething, KN6KB, AAA9WK\cf3  \cf1\ul MailTo:rmuething@cfl.rr.com\cf3\ulnone\{link=*! ExecFile("mailto:rmuething@cfl.rr.com")\}
\par    Phil Sherrod, W4PHS\cf0   \cf1\ul MailTo:phil@philsherrod.com\cf3\ulnone\{link=*! ExecFile("mailto:phil@philsherrod.com")\} 
\par 
\par 
\par    \cf0 Additional Coding and support by:
\par    \tab Native Kiss DLL by Peter Woods, N6PRW  \cf1\ul MailTo:\f5 prwoods@alum.wpi.edu\cf3\ulnone\f1\{link=*! ExecFile("mailto:prwoods@alum.wpi.edu")\} 
\par \cf0     \tab Adaptation of Phil Karn's "Voyager" VIterbi Encoder/Decoder to .NET by Randy Miller \cf1\ul MailTo:millerr@socal.rr.com\cf3\ulnone\{link=*! ExecFile("mailto:\cf1\ul millerr@socal.rr.com\cf3\ulnone ")\} 
\par 
\par \cf0 
\par }
310
Scribble310
Email Clients
Email Client Programs;Thunderbird Email Client;



Writing



FALSE
55
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 E\f1 mail Clients\cf0\b0\fs20 
\par 
\par \f0 Paclink will automatically set up (except for password which must be entered manually) accounts for the Microsoft Outlook Express email client shipped with all Windows installations prior to Windows Vista.  Many other email clients (e.g. \cf1\strike Mozilla Thunderbird\cf2\strike0\f2\{link=*! \f3 ExecFile("http://www.mozilla.com/en-US/thunderbird/")\}\cf0\f0  may be used however and one of the key features of Paclink is to allow the user to use their own favorite email client to send and receive messages using Paclink.
\par 
\par \cf1\b\fs32 General Email Client Setup Info\fs28 
\par \cf0\b0\fs20 
\par Most email client programs work similarly and require the same basic setup parameters. If you are using an email client different than Outlook Express you want to make sure that it can support more than one "account".  Most do. You probably have at least one account set up already for normal email.  Paclink interfaces just like a normal ISP email server with each account in Paclink corresponding to an account on an email client.
\par 
\par The important and necessary parameters to set up these accounts on an email client are:
\par 
\par \cf3\b\i Account Name\cf0\b0\i0  - This is the account name and should agree (normally \i not\i0  case sensitive) with the account name for the call sign or tactical address account set up in Paclink. For call sign accounts it should be the call sign of the account with -ssid if used (e.g. KN6KB-1). For tactical address accounts it will be the tactical address.
\par 
\par \cf3\b\i Email Address\cf0\b0\i0  - This will be the email address and will always be <account name>@winlink.org  e.g. KN6KB-1@winlink.org
\par 
\par \cf3\b\i Synchronization\cf0\b0\i0  - If you have other email accounts on the same server and wish them all to be synchronized (sent/received together) you can check the "Include this account when receiving mail or synchronizing" option available on most email clients. Note however Paclink MUST be running or trying to synchronize the Paclink accounts will usually result in an email client error.
\par 
\par \cf3\b\i Server Information\cf0\b0\i0  - This is where the servers are defined for the account in the email client. The normal settings:
\par 
\par \tab\cf1 For Incoming mail:\cf0  POP3 address "localhost" or "127.0.0.1"  (without the quotes) if on the same computer as Paclink.  If the email client program is located on a different computer than the one running Paclink then the address should be the computer name or dotted IP address of the computer on the LAN running Paclink. There may be places to enter the account name and password (should match the Paclink account name and password).  Note: Passwords are usually case sensitive! Do NOT check "Log on using Secure Password Authentication" if this option is available. If there is an option to "Save Password" you can check this to avoid having to manually enter the password upon a connection to Paclink.
\par 
\par \tab\cf1 For Outgoing mail:\cf0  SMTP use the same address as the POP3 settings above. Generally there is an option to select "My server requires authentication, use same settings as my incoming mail server" and these SHOULD be checked or enabled.
\par 
\par \cf3\b\i Connection\cf0\b0\i0  -  Most email programs provide some sort of option for connecting. In almost all cases for Paclink this should be "Local Area Network" even if the email client is on the same computer as Paclink.
\par 
\par \cf3\b\i Security\cf0\b0\i0  - No signing certificate or encrypting preferences should be selected or enabled.
\par 
\par \cf3\b\i Advanced or Port Setup\cf0\b0\i0  - If when you tried to set up Paclink's POP3 and SMTP ports you ran into a conflict and had to set alternate port numbers from the default 25 and 110 this is where you will set those ports in the mail client. The port numbers MUST match those set up in Paclink for the mail client to communicate with Paclink.
\par 
\par \cf3\b\i Mail Configuration\b0\i0  \cf0 - Most email client programs allow you to select which format to use for sending mail. While fancier formats (HTML etc.) and "stationary" allow for attractive messages and graphics they are not practical over low bandwidth radio channels.  If your email client has an option it should be set for "plain text". If Paclink receives mail that is not in plain text format it will attempt to convert the mail back to plain text to conserve space and bandwidth. 
\par 
\par \cf3\b\i Sending Mail From a specific Account -\cf0\b0\i0  When you have the Paclink account(s) set up on your email client and that client is configured for more than one account there is usually a provision to select which account is being used when composing and sending a message. Mail sent from other non-Paclink accounts will NOT go out via the Paclink program and radio email mechanism. 
\par 
\par 
\par \cf1\b\fs32 Mozilla Thunderbird Email Client Setup for Paclink
\par \cf3\i\fs20 
\par (Based on Mozilla Thunderbird 2.0.0.19 Dec 2008 )\cf0\b0\i0 
\par 1. Make sure Paclink is running, and your call sign account is configured in it.
\par 2. Make sure Paclink has selected non in-use ports for the POP3 and SMTP Servers and has the Password for POP3/SMTP set.
\par 2. Start Thunderbird and go to Tools, Account Settings, and click on Add Account.
\par 3. Select Email Account. Click Next.
\par 4. For Your Name put in your call sign. For your email address, your call sign @winlink.org. Click Next.
\par 5. Select POP and for the Incoming Server put "localhost", with no quotes. Uncheck the Use Global Folders. Click Next.
\par 6. For Incoming User Name, your call sign. Click Next.
\par 7. Account name should be your call sign @winlink.org. Click Finish.
\par 8. On the Account Settings page Find the Outgoing Server SMTP and click it. Click Add in the window.
\par 9. For Description, put in your call @winlink.org. For Server Name, put in localhost. For User Name, put in your call. Enter your password (case sensitive!) that matches the one in Paclink. User Name and Password should be checked. Make sure the port number matches what you set up in Paclink Site Properties. Use Secure Connection should be set to NO.
\par 10. Click OK, then OK.
\par 11. Click the account you just made in the left window's list. Look at the Outgoing Server SMTP for it, and make sure the WL2K account is the one in the window. If not, use the drop down and select it. With the Outgoing Server (SMTP) settings showing click Edit. Make sure the port number matches what was set up in Paclink. Server name should be localhost. Use Name and Password should be checked. User name should be your call sign. Use secure connection should be set to NO. Click OK. 
\par 12 Finally tweak the composition for radio use. For the Account selected Click Composition on the left. Uncheck Compose messages in HTML and uncheck automatically quote the original message.  
\par You will see both your internet email address account (provide it is configured) and your WL2K account, with separate folders for each. To send via your email account, select it at the top of it's folder list, to send via your WL2K account, select it at the top of it's folder list.
\par \f1 
\par \f0 When you Get or send mail from Thunderbird you should see the session in green (POP3) or blue (SMTP) on Paclink's right hand display. \f1 
\par }
320
Scribble320
License
License;



Writing



FALSE
83
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fmodern\fprq1\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red0\green0\blue0;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 License
\par \cf0\b0\f1\fs24 
\par \cf2\b\f0\fs20 Paclink Software License
\par \b0 
\par This license governs use of the accompanying software. If you use this software, you accept this license. If you do not accept this license, do not use the software.
\par Paclink is not "freeware" and is not public domain software. License to use this program is granted only under the terms outlined here.
\par 
\par \b Allowed Use\b0 
\par 
\par This software may be used without charge on amateur radio bands by licensed amateur radio operators. All other uses outside the amateur radio bands and by users licensed for other services than the amateur radio service, specifically commercial use of any kind, is prohibited without permission and prior separate license in writing. You may inquire about license for other use by contacting the Winlink System Administrator. Refer to https://winlink.org.
\par 
\par \b Copyright
\par 
\par \b0 This program is copyrighted by the Amateur Radio Safety Foundation, Inc. \'a9 2010-2017. All rights are reserved. 
\par 
\par \b Distribution\b0 
\par 
\par This program is distributed without charge from the Winlink (SM) web site (https://winlink.org) or from the Winlink (SM) FTP site (ftp://autoupdate.winlink.org). The intact distribution file can be copied, redistributed, and transmitted to others. Derivative works are prohibited. This program may not be reverse-engineered.  Redistributions in binary form must reproduce the above copyright notice, this intact, unmodified license description, and the following disclaimer in the documentation and/or other materials provided with the distribution.
\par  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
\par 
\par \b Registration\b0 
\par 
\par Registration to your callsign of certain software programs from ARSFI is voluntary and helps insure the continued support and enhancement of the Winlink (SM) system, and all ARSFI projects.  When you start a program that can be registered it will periodically display a registration reminder screen.  You can click a button to be reminded later and continue use the program without registering.  When you complete the purchase of a registration key you will receive a hexadecimal key linked to your call sign. Enter it on the registration reminder screen or the appropriate setup screen.  Once your registration key has been entered, the registration reminder will not display. 
\par 
\par \b Use of the Winlink (SM) System With This or Other Software\b0 
\par 
\par By using the Winlink servers and gateway stations ("the system") and by creating and maintaining an account in the system, you agree with, understand, and hold the Amateur Radio Safety Foundation, Inc., its contributors, developers, administrators, and system operators harmless per the conditions set forth here. You also agree to the following conditions, policies, and operating guidelines.  The Winlink Common Message Servers (CMS) and the participating Winlink stations (RMS gateways) make available publicly accessible ("Public Domain") information and data from the National Weather Service, the U.S. Government and other third-party sources. You must use such information with care as it may be inaccurate or out-of-date. The Amateur Radio Safety Foundation, Inc., its contributors, developers, administrators and system operators expressly disclaim responsibility for the content stored or transported by the system.  The information stored and transported by the system may be used freely by you as long as you operate in compliance with the rules governing Amateur Radio where you are located. Before using information obtained through the system, special attention should be given to the date and time of the data being displayed as well as the date and time of transport. Information passing through the system is not guaranteed to be accurate or current. The developers, administrators and system operators of the system will apply their best efforts to inspect and remove content prohibited by rules governing Amateur Radio. Should a message or data appear to be in violation of the these rules, administrators and system operators have the right to hold or destroy the prohibited data, content or message.  The system administrators and participating station sysops strive to make the Winlink service available 24 hours a day, seven days a week. However, because individuals who provide Winlink radio email service are volunteers and that many aspects of system administration are not automated, the timely delivery of data or messages may not always be possible, and, therefore, is not guaranteed.  Special care, automated tools and design is provided by the developers and administrators to protect the system, you and your computers from spam, malware and viruses, but freedom from these is not guaranteed. The system provides you with options for spam control. You are ultimately responsible to properly use them.  You assume the entire risk related to your use of information or data received or sent over the system. This information or data is provided "as is", and the Amateur Radio Safety Foundation, Inc., its contributors, developers, administrators, and system operators disclaim any and all warranties, whether express or implied, including (without limitation) any implied warranties of fitness for a particular purpose. In no event will the Amateur Radio Safety Foundation, Inc., its contributors, developers, administrators or operators be liable to you or to any third-party for any direct, indirect, incidental, consequential, special or exemplary damages resulting from any use or misuse of the system, from data or information acquired from it, or for any failure of the system to perform in any way. 
\par 
\par \b Winlink (SM) Privacy Policy
\par 
\par \b0  You understand and agree that email content sent and received over the system is not private. By most countries' amateur radio rules--to which each radio licensee must abide--content may not be encrypted with the intention to obscure it. Message content transported through the system over public or government radio frequencies, over the internet, or over private networks may be accessed by anyone with appropriate equipment and software. However, the policy and practice of Winlink administrators and system operators is to never disclose the content of messages sent over the system unless expressly permitted by the affected user, or requested by law enforcement, regulatory or defense agencies, health and safety agencies, or similar organizations in support of life- or property-threatening mitigation efforts, search and rescue, legal inquiries, or incident investigations. Personally identifying information is collected by user programs and system web applications only when users knowingly submit such information. You agree to allow publishing what you submit if it is also publicly available on government license databases or web sites. Winlink users, administrators, and system operators may use the information you submit for your safety or to improve your experience using the Winlink service. Personally identifying information will not be sold, traded or used for commercial gain. Aggregated demographic information and user behavior data, without personally identifying information, may be used for marketing, promotion, or fund-raising purposes.
\par 
\par \b Winlink (SM) System Policies and Operating Guidelines
\par 
\par \b0 Winlink is a global radio email service. Most gateway sysops and users are subject to amateur radio regulations. Winlink is also used for MARS and government and non-government agency communications on frequencies outside the amateur bands. Different regulations apply to those users. It is always the responsibility of all radio licensees to operate within the laws and rules that apply to their license and location. Winlink administrators and volunteer gateway station operators exercise the self-regulation customs common in amateur radio, and so, to assist Winlink participants to stay out of trouble, and to foster orderly operations, you agree to these guidelines and policies by maintaining an account and using the system. Operating within these guidelines will keep most operators within the laws that govern their radio license, and avoid administrative actions against your account.
\par 
\par Do Not Interfere
\par 
\par Listen first! Because a live human being (control operator) is always present at the initiating station, there is one common theme paramount to the successful operation of the system: simply listen on the frequency about to be used to determine if that frequency is occupied. If the frequency is occupied by any detectable signal, the proper action is to either wait until it is free before transmitting, or find another Radio Message Server (RMS or gateway station) whose frequencies are not busy. Not only is this a common courtesy to other operators, but it is a specific requirement of every country's rules regulating amateur radio licenses, worldwide.
\par 
\par Message Content
\par 
\par There is no privacy over amateur radio. Anyone who is properly equipped can read messages handled by Winlink. Each gateway sysop routinely monitors messages passing through their station to ensure acceptable message content. Any message violating local rules is deleted and the sender advised. Gateway sysops are legally responsible for traffic flowing through their stations.
\par 
\par Third-Party Traffic
\par 
\par Third-party traffic is any message transmitted that is either from or to a non-licensee. In the Western Hemisphere (with a few exceptions) there is no restriction on third-party traffic being passed over amateur radio. Many countries outside of the Western Hemisphere also now permit third-party traffic over amateur radio. Messages between amateurs if they originate from or are delivered over Internet are not considered third-party traffic. Third-party traffic rules only deal with that portion of the message path which is transmitted over the radio spectrum.  For example: if a message originates from a non-amateur as an internet email in the U.K. and is delivered to a U.S. amateur over the radio from a gateway station in the US, no third-party rule is broken even though the U.K. does not allow third-party traffic over amateur channels. Likewise, a message originating from a U.S. amateur and passed by radio to a U.S. gateway is okay even if it is addressed to the Internet address of a non-amateur in the U.K.  Users and sysops must make themselves familiar with these third-party rules for the country in which they are operating as well as linking with if they are exchanging messages with non-amateurs. US gateway sysops should know that \'a797.219(c) provides protection for licensees operating as part of a message forwarding system. "...the control operators of forwarding stations that retransmit inadvertently communications that violate the rules in this Part are not accountable for the violative communications. They are, however, responsible for discontinuing such communications once they become aware of their presence."
\par 
\par Business Content
\par 
\par Directly or indirectly enhancing one's pecuniary interest using amateur radio is universally prohibited. Business traffic is any message, sent or received, that is related to an amateur's business or an activity involved in making money, attempting to make money, or even to save from spending money for the amateur. Placing orders to trade stocks or receiving investing guidance are clear-cut examples of prohibited message content. Ordering or receiving paid subscription-based weather guidance, custom forecasts, or weather information products is another. On the other hand, in the US, the FCC has allowed that infrequently ordering physical items (ordering a pizza or repair parts) for personal use is not in violation of the rules so long as it is incidental to your activity as an amateur, and not done to enhance your pecuniary interest. Even though the Winlink user may use a Telnet connection or WebMail and never transfer prohibited content over the Amateur radio spectrum, it is the policy of Winlink administrators to abide by the Part 97 (US rules), and FCC interpretations of these rules, as it pertains to business message content transported by the system.   Winlink administrators consult with the FCC or other regulatory entity representatives for their interpretation of applicable rules whenever their application or intent is unclear. Administrators will communicate these interpretations to any amateur who may question an administrative warning or action. Administrators may block or remove messages from unsolicited mail sources, incoming messages from list servers or any regular or frequent "subscription-type" messages that might contain business related content. The best way to avoid unsolicited mail is to keep your Winlink email address private and to learn to properly use your account anti-spam whitelist. Do not instruct paid information services to deliver purchased or subscribed information products to your Winlink account when they can be delivered through other radio services off the amateur bands.
\par 
\par Commercial services must always be employed first if business content will be common. Sailmail, Inmarsat, or Iridium are recommended for maritime users, and there are others regionally available. Winlink connections for business content transfer via telnet, satellite services, or high-speed (WiFi) radio LAN are allowed for infrequent use.
\par 
\par Encrypted Messages
\par 
\par All messages sent over amateur radio services must be in plain language and use a publicly published format. Message attachments must be of file types that can be viewed with commonly available software such as .doc, .rtf, .jpg, .bmp, etc. The system will not accept as attachments executable files with extensions such as .exe, .com, .vbs, etc. This reduces the chance of an encrypted message but also is another protection against malware. Administrators may change the acceptable file types as experience dictates.
\par 
\par Obscene Content
\par 
\par Obscene content is not allowed by the laws of most countries on amateur radio frequencies, and also not on the Winlink system. It will be deleted when discovered and the sender immediately locked out of the system whether the sender accesses the system via Internet or radio. The receiving gateway sysop usually determines if a message is obscene and usually warns the originator prior to taking any action. Warnings are not required, however.
\par 
\par Ban Requests
\par 
\par If a user receives unwanted, illegal or improper messages they should advise the system administrator (address a message to 'SYSOP@Winlink.org) and the source of those messages can be locked out. If a user persists in violating the content rules, the system administrator or one of the gateway station operators will permanently lock the abuser out of the system.
\par 
\par Viruses & Malware
\par 
\par Every message offered to the system from any internet source is scanned for viruses and malware, and any detected threat is deleted before it can enter the system. The recipient is notified whenever an infection is detected and purged. Where possible the sender will be advised that his or her system is sending malware.
\par 
\par Callsign (license) Validation
\par 
\par Any licensee may self-register with the Winlink system by using a client program, connecting to any gateway via radio or over Telnet to a Winlink server. One of the Winlink gateway operators will verify the call against the appropriate country's available amateur license database. This ensures the license is valid and that operating privileges are appropriate for the frequency band used. If this information is not found or available, the user will be asked to provide documentary evidence of their identity and that their license is valid. The user will be locked out of the system if it is not supplied within a specified time. Applicable "special temporary authority" agreements from CEPT, IARP or other reciprocal agreements must be sent with proof of license on demand by a system administrator in order to retain use of the system.
\par 
\par Login Security
\par 
\par Winlink system use requires secure account login for web and radio users using client programs or terminal or web access. Passwords are secure and not sent over radio links. With this users may protect themselves from gaining unauthorized access to their account. This does not secure messages passed over the radio. It only authenticates that the radio link is established with a verified and licensed station within the Winlink system.
\par 
\par Winlink (SM) is a service mark of the Amateur Radio Safety Foundation, Inc.  \cf3\strike Amateur Radio Safety Foundation\cf4\strike0\{linkID=20\}\cf0 
\par \cf2 
\par }
330
Scribble330
Updates
Updates;



Writing



FALSE
11
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil Arial;}{\f1\fnil\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue255;}
\viewkind4\uc1\pard\cf1\b\f0\fs32 Updates\cf0\b0\fs20 
\par 
\par \f1 Paclink like any complex piece of software will have bug fixes and added features and capabilities. The program and support files are designed to automatically update if there is internet access.  This insures that a site is always running the most up-to-date version and there are no version incompatibilities.  It also dramatically reduces the support effort. 
\par 
\par When the program starts it will check the Winlink autoupdate site for any available update and compare that to the current running version.  If there is a newer version available it will be automatically downloaded. Checks for available updates are performed every 24 hours while the program is running. Autoupdates also may include support files such as Paclink Revision History,  new or updated TNC configuration files (.aps files) RMS list updates, Help or other documentation.
\par 
\par A "*" is prepended to the text on title bar of the main form following an autoupdate.  This provides a visual indication that an autoupdate has been applied to the application and the user should restart Paclink to use the updated version.
\par \f0 
\par }
340
Scribble340
Main Display
Main Display;



Writing



FALSE
28
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil Arial;}{\f1\fnil\fcharset0 Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;\red128\green0\blue128;\red0\green128\blue0;}
\viewkind4\uc1\pard\cf1\b\f0\fs32 Main Display\cf0\b0\fs20 
\par 
\par \f1 Paclink's main displays shows connection activity both from radio and telnet connections (left hand display) and from SMTP/POP3 sessions with email clients (right hand display).   The panel separator can be moved left or right changing the relative size of the left and right display panels. Once the program is configured there is normally no or minimal user interaction required allowing it to run unattended.
\par 
\par There are a few items worth pointing out in the example display below:
\par 
\par \pard\qc\cf2\{bmc Main Display.png\}\cf0 
\par \pard\f0 
\par \cf3\b\i\f1 Display Colors\i0  
\par \b0 
\par \cf0 The Left side of main display uses colors to help identify the source of displayed messages:
\par \tab\cf3\b Red\cf0\b0  is used to indicate warnings or abnormal status type messages
\par  \tab\cf4\b Purple\cf0\b0  is used to indicate TNC status messages or events
\par \tab\b Black\b0  is used to indicate text received from the remote station. (note compressed binary data is suppressed)
\par \tab\cf1\b Blue\cf0\b0  is used to indicate text sent by the Paclink station. (again binary data is suppressed)
\par \tab\cf5\b Green\cf0\b0  is used to indicate script progress during packet connection scripts and also fo rnormal status type messages.
\par 
\par The Right side of the main display uses colors to show direction of messages:
\par \tab\cf3\b Red\cf0  \b0 is used to indicate information or status
\par \tab\cf1\b Blue\cf0\b0  is used to indicate messages received from the email client that are bound to Winlink
\par \tab\cf5\b Green\cf0\b0  is used to indicate messages received from Winlink and delivered to the email client.
\par 
\par \cf3\b\i Status Bar\i0 
\par 
\par \cf0\b0 The status bar at the bottom of the window is used to communicate status of a connection. Connection state is shown in the far left.  The current link speed is shown next and then the progress bar is next showing the approximate percentage of completion of all transmitted (outbound) and received (inbound) messages for the session (based on the compressed size of all accepted message proposals). On the far right of the status bar is the message count of current pending messages to email clients or to Winlink. \f0 
\par }
350
Scribble350
Documentation
Documentation;



Writing



FALSE
9
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Documentation
\par 
\par \cf0\b0\fs20 This menu lets you view any plain text (.txt) or rich text (.rtf) file. It is accessed via Help|Documentation from the Main Paclink menu. You can select the file and directory using the standard file list menu below. Normally the RMS Frequency list and Paclink Revision history are in the documentation directory. 
\par \f1 
\par \pard\qc\cf2\f0\{bmc Documentation.bmp\}\cf0\f1 
\par \pard 
\par }
360
Scribble360
Message Precedence
Message Precedence;



Writing



FALSE
22
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;\red0\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Message Precedence\cf0\b0\f1\fs20 
\par 
\par \f0 Paclink uses two simple but effective mechanisms for establishing a message precedence (priority) for sending outbound messages. The Winlink servers (telnet, RMS) use a similar mechanism for sending inbound messages to Paclink.
\par 
\par \cf2\b\i Non MARS site installations:
\par \cf0\i0\fs24 
\par \b0\fs20 If a message is originated by the mail client program that connects to Paclink that does \i NOT\i0  contain a subject line precedence header Paclink will automatically insert the code \cf2 //WL2K / \cf0  (routine) at the beginning of the subject line and process the message as routine. If Higher pecedence is required required the user can iinclude one of the following in the subject line:
\par \cf3 //WL2K Z/, //WL2K O/, //WL2K P/ or //WL2K R/. Where //WL2K Z/ is the highest priority and //WL2K R/ is routine priority.
\par \cf0 
\par When a connection is made by Paclink all outbound messages will be sorted first by subject line precedence code and then by size within a precedence group with the shortest messages first.
\par 
\par The "//WL2K " is also used as a SPAM filter for inbound mail to WL2K from senders that are not on the recipients approved whitelist.
\par \f1 
\par \f0 
\par \cf2\b\i MARS site Installations:
\par \cf0\i0\fs24 
\par \b0\fs20 When the Paclink site call sign is a legitimate MARS call sign it uses a message precedence system which uses a short header in the message subject line to indicate the message priority. If a message is originated by the mail client program that connects to Paclink that does \i NOT\i0  contain a subject line precedence header Paclink will automatically insert the code \cf2 //MARS R/ \cf0  (routine) at the beginning of the subject line and process the message as routine.
\par 
\par When a connection is made by Paclink all outbound messages will be sorted first by subject line precedence code and then by size within a precedence group with the shortest messages first.\f1 
\par }
370
Scribble370
PTC II with AGW Packet Engine




Writing



FALSE
13
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 PTC II with AGW Packet Engine\cf0\b0\f1\fs20 
\par 
\par \f0 The SCS PTC II (all models) can be used with the AGW Packet Engine (Free or Pro).  It must have firmware 3.7 or later installed (which supports KISS mode). The following is a screen capture of the AGW Pro setup courtesy of Jean-Marie VE2AEY.
\par 
\par \tab\tab\cf2\{bmc PTCII_AGWPE.bmp\}
\par 
\par 
\par \cf0\f1 
\par 
\par 
\par }
380
Scribble380
USB to Serial Adapters
USB to Serial Adapters;



Writing



FALSE
12
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil Courier New;}{\f3\fnil\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red128\green0\blue0;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 USB to Serial Adapters\cf0\b0\f1\fs20 
\par 
\par \f0 Depending on the installation Paclink may require one or more serial ports. These generally can be either standard serial ports or USB to serial adapters. However when using USB to serial adapters some care must be utilized.
\par 
\par \pard\sb100\sa100 Some USB to serial adapters (notably Belkin but some others as well) will \i NOT \i0 work reliably with Paclink (or any other .NET program using the standard MS .NET serial port).  This is normally not a hardware problem but due to drivers that don't correctly implement the .NET serial port control. You may be able to locate, download and install suitable drivers for .NET from the web site of the USB to serial adapter manufacturer. Reliable operation with Edgeport (single or multiple ports),  Keyspan, and SerialGear (with FTDI drivers) USB to serial adapters have been reported on Win 2000 and Win XP.
\par \pard\f1 
\par \pard\sb100\sa100\lang1024\f0 The majority of issues with Vista seem to center around drivers especially the drivers for USB to serial adapters.  \lang1033 The FTDI drivers available both from \cf1\strike SerialGear\cf2\strike0\f2\{link=*! \f3 ExecFile("http://www.serialgear.com/ftdi-chip-drivers.cfm")\}\cf0\f0  and via Microsoft's update appear very stable and their latest chip sets are also USB 2.0 capable. Good results have been reported using a 4 port adapter from \cf1\strike usbgear\cf2\strike0\f2\{link=*! ExecFile("\f0 http://www.usbgear.com"\f2 )\}\cf0\f0  on a Vista 64 installation.The USBG-4X232FTDI from serial gear above provides 4 serial ports, has LEDs to show serial activity, has approved drivers for Vista and has been verified with Paclink on both Win XP and Vista.
\par \cf3\b Note:\cf0\b0  Moving the USB connection from one physical USB port to another \i MAY\i0  change the port number of the serial port and this will require changing the settings using that serial port in Paclink.  It is also normally possible to use the Device manager in Windows to re assign the COM port numbers. In general however it is best not to change USB ports for USB to serial adapters.
\par \pard\f1 
\par }
390
Scribble390
Native KISS Driver
KISS operation;Native KISS driver;



Writing



FALSE
25
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}{\f2\fnil\fprq1\fcharset0 Courier New;}}
{\colortbl ;\red0\green0\blue255;\red0\green0\blue0;\red0\green128\blue0;\red128\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Native KISS Driver\cf0\b0\f1\fs20 
\par 
\par \f0 Beginning Feb 2009 Paclink contains a native KISS Driver (no third party software required) written by Peter Woods, N6PRW of the WL2K development team. This driver allows operation of most TNCs that support KISS mode including those Tasco chip modems used in some radios (Kenwood TS-2000, TM-D710, TM-D700, TH-D7 and the ALINCO radios DR-135T, 235T, and 435T). To use the KISS mode driver simply select one of the following in the TNC Selection of the Packet TNC Channel configuration menu:
\par \lang1024\f2  \cf2\f0 TM-D700 int, TM-D710 int, TH-D7, TS-2000 int, ALINCO int, or Generic KISS
\par Example .aps files will be shown for each of the above. These files may require some slight edits to handle certain radios and parameters but should be very close as is. The  files contain two sections one for 1200 baud and one for 9600 baud and the appropriate section is read based on the channel selected on-air baud rate. are The example files are heavily commented to simplify this customization. If you customize a file be sure to use a plain text editor and save it as a different name in the Paclink\\Data directory with a .aps extension. Then simply point your Native KISS Driver packet channel to that modified .aps file. Consult your TNC manual to determine:
\par 1) How to set your on-air baud rate. A typical command is HBAUD 1200 or HBAUD 9600
\par 2) Which commands are required to put the TNC into KISS mode. Typical commands are:
\par \tab KISS ON
\par \tab RESTART
\par but these may be different for some KISS TNCs. These commands are placed in the .aps file between the KISSSTART and KISSEND markers.
\par 
\par If using one of the radios with the older Tasco modem chips (TS-2000, TM-D700, TH-D7, ALINCO DR-135T/235T/435T)  you  will probably have to use small packet sizes (MAXFRAMESIZE <= 128) and small values of maximum outstanding frames (MAXFRAMES = 1)  to accommodate the very small serial buffers in these early Tasco chip TNCs. This will reduce maximum throughput but otherwise work OK.
\par 
\par If you have problems using the native KISS driver capture the session and any exceptions in the Log files and post to the Paclink Yahoo group. If you generate a modified .aps file that you feel will be useful to others be sure and comment it and post it to the Yahoo group and we will include it as an example in future releases.
\par 
\par If you require the TNC/Radio sharing available in third party software like AGWPE and BPQ you will not be able to use Paclink's native mode KISS driver but should continue using those programs and the appropriate Paclink setup.
\par 
\par For radios that share the same serial port for both radio control and the internal TNC  (e.g. Kenwood TS-2000) you will not be able to support simultaneous radio control and internal TNC operation. 
\par \cf0\lang1033\f1 
\par \cf3\strike\f0 Packet TNC Channels\cf4\strike0\{linkID=130\}
\par \cf3\strike Packet AGW Channels\cf4\strike0\{linkID=140\}
\par \cf0\f1 
\par }
420
Scribble420
Credits
Credits;



Writing



FALSE
17
{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil Arial;}}
{\colortbl ;\red0\green0\blue255;\red255\green0\blue0;}
\viewkind4\uc1\pard\cf1\b\fs32 Credits
\par \cf2\i\fs20 Thanks and credit is given to the following where their public source code was used or adapted:
\par \cf0\b0\i0 
\par Phil Karn, KA9Q        NASA Voyager (r = 1/2, K=7) Viterbi encoder/decoder in C adapted to .NET by Randy Miller of the Winlink Development Team.
\par 
\par Murphy McCauley      VB FFT Release 2-B adapted to .NET. Based on the work of Don Cross
\par 
\par Simon Rockliff\tab          Reed-Solomon Encoder/Decoder adapted from its original 1991 C to VB.net.
\par 
\par \cf1\b\fs32 
\par 
\par \cf0\b0\f1\fs20 
\par 
\par 
\par }
0
0
0
45
1 Paclink
2 Paclink Overview=Scribble10
2 Updates=Scribble330
2 Documentation=Scribble350
2 Amateur Radio Safety Foundation=Scribble20
2 License=Scribble320
2 Contacts=Scribble300
1 Configuration
2 Installation=Scribble30
2 Site Properties=Scribble40
2 AGW Engine Properties=Scribble70
2 Accounts=Scribble80
2 Callsign Accounts=Scribble90
2 Tactical Address Accounts=Scribble100
2 Editing Account Properties=Scribble75
2 Email Clients=Scribble310
2 Channels=Scribble110
2 Telnet Channels=Scribble120
2 Packet TNC Channels=Scribble130
2 Packet AGW Channels=Scribble140
2 HF Pactor Channels=Scribble150
2 HF WINMOR Channels=Scribble160
2 TNC Initialization Files=Scribble170
2 Packet Connection Scripts=Scribble180
1 Operation
2 Main Display=Scribble340
2 CMS/RMS Polling=Scribble200
2 Secure Login=Scribble60
2 Establishing HF Connections=Scribble190
2 Whitelist=Scribble280
2 Message Limitations=Scribble210
2 Logs=Scribble220
2 Message Routing=Scribble270
2 Addressing Messages=Scribble290
2 Message Precedence=Scribble360
2 Paclink Overview=Scribble10
2 Troubleshooting=Scribble230
2 Simple Terminal=Scribble50
2 USB to Serial Adapters=Scribble380
2 PTC II with AGW Packet Engine=Scribble410
2 Sound Card Selection=Scribble410
1 Internals
2 Implementation=Scribble240
2 Directories and Files=Scribble250
2 Paclink Settings=Scribble260
6
*InternetLink
16711680
Courier New
0
10
1
....
0
0
0
0
0
0
*ParagraphTitle
-16777208
Arial
0
11
1
B...
0
0
0
0
0
0
*PopupLink
-16777208
Arial
0
8
1
....
0
0
0
0
0
0
*PopupTopicTitle
16711680
Arial
0
10
1
B...
0
0
0
0
0
0
*TopicText
-16777208
Arial
0
10
1
....
0
0
0
0
0
0
*TopicTitle
16711680
Arial
0
16
1
B...
0
0
0
0
0
0
