<h1>Device Management System for C-ITS RSUs</h1>

This project made on my Bsc thesis. This is a device management and monitoring system that can handle large number of devices.
The system uses SNMP protocol to communicate with the devices.

<h2>Architecture</h2>
A system consists of three layers.
</br>
</br>

![System Archtitecture](https://github.com/PeterSturm/RoadSideUnit_DMS/blob/master/docs/snmp_dms_architecture.jpg)

<h3>SNMP Simulator</h3>
A javafx based application that can simulate SNMP Agents and allows to plug in real RSU devices behind the simulated agents.
<h3>SNMPManager</h3>
ASP.Net Core Web API that functions as an SNMP Manager. With multiple Web APIs deployed, each one responsible for a subset of RSUs.
</br>
This Web API follows the Clean Architecture:
</br>
</br>

<img alt="Web API Archtitecture" src="https://github.com/PeterSturm/RoadSideUnit_DMS/blob/master/docs/WebAPI_Core_diagram_croped.jpg" width="50%" height="50%"/>

<h3>Dashboard</h3>
ASP.Net Core WebApp that manages the deployed SNMPManagers.
