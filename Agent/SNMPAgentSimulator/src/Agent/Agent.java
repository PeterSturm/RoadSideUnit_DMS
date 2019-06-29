package Agent;

/**
 * Created by: Péter Sturm
 * Date: 2019-03-23
 */

import org.snmp4j.PDU;
import org.snmp4j.ScopedPDU;
import org.snmp4j.UserTarget;
import org.snmp4j.agent.*;
import org.snmp4j.agent.mo.*;
import org.snmp4j.agent.mo.snmp.*;
import org.snmp4j.agent.security.MutableVACM;
import org.snmp4j.mp.MPv3;
import org.snmp4j.mp.MessageProcessingModel;
import org.snmp4j.mp.SnmpConstants;
import org.snmp4j.security.*;
import org.snmp4j.security.nonstandard.PrivAES256With3DESKeyExtension;
import org.snmp4j.smi.*;
import org.snmp4j.transport.DefaultUdpTransportMapping;


import java.io.File;
import java.io.IOException;
import java.util.*;

public class Agent  extends BaseAgent {
    public String address;
    public int port;
    public String trapListenerAddress;
    private OctetString trapUserName;
    private OctetString trapAuth;
    private OctetString trapPriv;
    private UserTarget target;
    private OctetString localEngineID;

    public void setMibObjects(ArrayList<MibObject> mibObjects) {
        this.mibObjects = mibObjects;
    }

    private ArrayList<MibObject> mibObjects;

    public Agent(File bootCounterFile, File configFile)
    {
        super(bootCounterFile, configFile,
                new CommandProcessor((new OctetString(MPv3.createLocalEngineID()).substring(0,9))));
        SecurityProtocols.getInstance().addPrivacyProtocol(new PrivAES256With3DESKeyExtension());

        localEngineID = agent.getContextEngineID();
    }

    public void start(String address, int port, String trapAddress, String trapusername, String trapaut, String trappriv)
    {
        if (address == null || address == "")
            address = "127.0.0.1";

        address += "/" + String.format("%d", port);
        this.port = port;

        trapListenerAddress = trapAddress;
        trapUserName = new OctetString(trapusername);
        trapAuth = new OctetString(trapaut);
        trapPriv = new OctetString(trappriv);

        try {
            this.address = address;
            init();
            configTrap();
            addShutdownHook();
            getServer().addContext(new OctetString("public"));
            finishInit();

            run();
            sendColdStartNotification();
        }
        catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    private void configTrap()
    {
        // Configure UserTagret with the proper SNMPv3 credentials
        target = new UserTarget();
        target.setVersion(SnmpConstants.version3);
        target.setSecurityLevel(SecurityLevel.AUTH_PRIV);
        target.setSecurityName(trapUserName);
        // Setup the Trap message endpoint address
        target.setAddress(new UdpAddress(trapListenerAddress));
        session.setLocalEngine(localEngineID.getValue(), 0, 0);
    }

    public void sendTrap()
    {
        // Create PDU with "heartbeat"
        ScopedPDU pdu = new ScopedPDU();
        pdu.setType(PDU.TRAP);
        pdu.setContextEngineID(localEngineID);
        pdu.add(new VariableBinding(new OID(".1.3.6.1.2.1.1.8"), new OctetString("hearbeat")));

        // Try send SNMP Trap message with the cretaed PDU
        try {
            session.send(pdu, target);
            System.out.println(address + " sent trap to: " + trapListenerAddress);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    protected void registerSnmpMIBs() {
        super.registerSnmpMIBs();
    }

    @Override
    protected void initTransportMappings() throws IOException {
        transportMappings = new DefaultUdpTransportMapping[] {
                new DefaultUdpTransportMapping(new UdpAddress("0.0.0.0/"+ String.format("%d", port))) };
    }

    @Override
    protected void registerManagedObjects() {
        try {
            // Check if the local MibObject List is instantiated
            if(mibObjects != null)
                // Create a DefaultMOTable type (Managed Object Table) table
                // Based on the given MibObjects
                // and Register it, so the SNMP Agent can work with it
                server.register(createMibTable(mibObjects), null);
        }
        catch (DuplicateRegistrationException ex) {
            ex.printStackTrace();
        }
    }

    @Override
    protected void unregisterManagedObjects() {

    }

    @Override
    protected void addUsmUser(org.snmp4j.security.USM usm) {
        // Create SNMPv3 user for SNMP GET, SET, ... requests
        // SNMP Managers can make request to this agnet with these credentials
        UsmUser user = new UsmUser(new OctetString("admin"),
                AuthSHA.ID,
                new OctetString("authpass012"),
                PrivDES.ID,
                new OctetString("privpass012"));

        // Create SNMPv3 user for SNMP Trapv2 message sending
        // This agent can send Trap messages to managers with these credentials
        UsmUser trapuser = new UsmUser(trapUserName
                ,AuthSHA.ID
                ,trapAuth
                ,PrivDES.ID
                ,trapPriv);

        usm.addUser(user.getSecurityName(), null, user);
        usm.addUser(trapuser.getSecurityName(), null, trapuser);
    }

    @Override
    protected void addNotificationTargets(SnmpTargetMIB snmpTargetMIB, SnmpNotificationMIB snmpNotificationMIB) {
        snmpTargetMIB.addDefaultTDomains();

        snmpTargetMIB.addTargetAddress(new OctetString("notification"),
                TransportDomains.transportDomainUdpIpv4,
                new OctetString(new UdpAddress(trapListenerAddress).getValue()),
                200, 1,
                new OctetString("notify"),
                new OctetString("v3"),
                StorageType.permanent);
        snmpTargetMIB.addTargetParams(new OctetString("v3"),
                MessageProcessingModel.MPv3,
                SecurityModel.SECURITY_MODEL_USM,
                new OctetString("public"),
                SecurityLevel.NOAUTH_NOPRIV,
                StorageType.permanent);
        snmpNotificationMIB.addNotifyEntry(new OctetString("default"),
                new OctetString("notify"),
                SnmpNotificationMIB.SnmpNotifyTypeEnum.trap,
                StorageType.permanent);
    }

    @Override
    protected void addViews(VacmMIB vacmMIB) {
        vacmMIB.addGroup(SecurityModel.SECURITY_MODEL_USM,
                new OctetString("admin"),
                new OctetString("allagroup"),
                StorageType.nonVolatile);

        vacmMIB.addAccess(new OctetString("allagroup"), new OctetString(),
                SecurityModel.SECURITY_MODEL_USM,
                SecurityLevel.AUTH_PRIV,
                MutableVACM.VACM_MATCH_EXACT,
                new OctetString("fullReadView"),
                new OctetString("fullWriteView"),
                new OctetString("fullNotifyView"),
                StorageType.nonVolatile);

        vacmMIB.addViewTreeFamily(new OctetString("fullReadView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullWriteView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullNotifyView"), new OID("0.1"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
    }

    @Override
    protected void addCommunities(SnmpCommunityMIB snmpCommunityMIB) {
        Variable[] com2sec = new Variable[] {
                new OctetString("public"),              // community name
                new OctetString("admin"),              // security name
                getAgent().getContextEngineID(),        // local engine ID
                new OctetString("public"),              // default context name
                new OctetString(),                      // transport tag
                new Integer32(StorageType.nonVolatile), // storage type
                new Integer32(RowStatus.active)         // row status
        };
        SnmpCommunityMIB.SnmpCommunityEntryRow row =
                snmpCommunityMIB.getSnmpCommunityEntry().createRow(
                        new OctetString("public2public").toSubIndex(true), com2sec);
        snmpCommunityMIB.getSnmpCommunityEntry().addRow(row);
    }

    private DefaultMOTable createMibTable(ArrayList<MibObject> mibObjects)
    {
        // Create table -->
        MOTableSubIndex[] subIndexes = new MOTableSubIndex[] {
                new MOTableSubIndex(SMIConstants.SYNTAX_INTEGER)
        };
        MOTableIndex indexDef = new MOTableIndex(subIndexes, false);

        MOColumn[] columns = new MOColumn[2];
        int columnIndex = 0;
        // Create two cloumns for the two data types that will be stored
        columns[columnIndex++] = DefaultMOFactory
                .getInstance()
                .createColumn(8
                                , SMIConstants.SYNTAX_INTEGER32
                                , MOAccessImpl.ACCESS_READ_WRITE);
        columns[columnIndex++] = DefaultMOFactory
                .getInstance()
                .createColumn(9
                                , SMIConstants.SYNTAX_OCTET_STRING
                                , MOAccessImpl.ACCESS_READ_WRITE);

        // Create the table with the previously defined columns
        // and root OID of 0.1.15628.4.1
        DefaultMOTable table = new DefaultMOTable(new OID("0.1.15628.4.1")
                                                    , indexDef
                                                    , columns);
        // <-- Create table

        // Create Rows -->
        DefaultMOTableModel<DefaultMOTableRow> model = (DefaultMOTableModel<DefaultMOTableRow>) table.getModel();

        ArrayList<Variable[]> rows = new ArrayList<Variable[]>();
        // Iterate through the MibObject that given by the RSUAgent
        // create a row for each object,
        // put the data in the corresponding column
        for(MibObject obj : mibObjects)
        {
            switch (obj.Type)
            {
                case Integer32:
                    model.addRow(new DefaultMOMutableRow2PC(new OID(obj.Id)
                            , new Variable[]
                            {
                                    new Integer32((int)obj.Value),
                                    new OctetString("")
                            }));
                    break;
                case OctetString:
                    model.addRow(new DefaultMOMutableRow2PC(new OID(obj.Id)
                            , new Variable[]
                            {
                                    new Integer32(0),
                                    new OctetString((String) obj.Value)
                            }));
                    break;
            }
        }
        // <-- Create Rows

        table.setVolatile(true);
        return table;
    }
}
