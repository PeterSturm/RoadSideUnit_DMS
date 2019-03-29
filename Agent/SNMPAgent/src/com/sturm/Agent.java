package com.sturm;

/**
 * Created by: Péter Sturm
 * Date: 2019-03-23
 */

import org.snmp4j.CommunityTarget;
import org.snmp4j.PDU;
import org.snmp4j.ScopedPDU;
import org.snmp4j.UserTarget;
import org.snmp4j.agent.*;
import org.snmp4j.agent.example.Snmp4jDemoMib;
import org.snmp4j.agent.io.ImportModes;
import org.snmp4j.agent.mo.*;
import org.snmp4j.agent.mo.ext.AgentppSimulationMib;
import org.snmp4j.agent.mo.snmp.*;
import org.snmp4j.agent.mo.snmp4j.example.Snmp4jHeartbeatMib;
import org.snmp4j.agent.security.MutableVACM;
import org.snmp4j.log.LogAdapter;
import org.snmp4j.log.LogFactory;
import org.snmp4j.mp.MPv3;
import org.snmp4j.mp.MessageProcessingModel;
import org.snmp4j.mp.SnmpConstants;
import org.snmp4j.security.*;
import org.snmp4j.security.nonstandard.PrivAES256With3DESKeyExtension;
import org.snmp4j.smi.*;
import org.snmp4j.transport.DefaultUdpTransportMapping;


import java.io.File;
import java.io.IOException;
import java.util.List;
import java.util.SortedMap;
import java.util.TreeMap;

public class Agent  extends BaseAgent {

    private static final LogAdapter logger =
            LogFactory.getLogger(BaseAgent.class);
    public String address;
    public String trapListenerAddress;
    private OctetString trapUserName;
    private OctetString trapAuth;
    private OctetString trapPriv;
    private UserTarget target;
    private Snmp4jHeartbeatMib heartbeatMIB;

    public Agent(File bootCounterFile, File configFile)
    {
        super(bootCounterFile, configFile,
                new CommandProcessor(new OctetString(MPv3.createLocalEngineID())));
        SecurityProtocols.getInstance().addPrivacyProtocol(new PrivAES256With3DESKeyExtension());

    }

    public void start(String address, String trapAddress, String trapusername, String trapaut, String trappriv)
    {
        if (address == null || address == "")
            address = "127.0.0.1/161";

        trapListenerAddress = trapAddress;
        trapUserName = new OctetString(trapusername);
        trapAuth = new OctetString(trapaut);
        trapPriv = new OctetString(trappriv);

        try {
            this.address = address;

            init();
            configTrap();
            //agent.loadConfig(ImportModes.REPLACE_CREATE);
            addShutdownHook();
            getServer().addContext(new OctetString("public"));
            finishInit();

            run();
            sendColdStartNotification();

            /*while (true) {
                try {
                    Thread.sleep(1000);
                }
                catch (InterruptedException ex1) {
                    break;
                }
            }*/
        }
        catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    private void configTrap()
    {
        target = new UserTarget();
        target.setVersion(SnmpConstants.version3);
        target.setSecurityLevel(SecurityLevel.AUTH_PRIV);
        target.setSecurityName(trapUserName);
        target.setAddress(new UdpAddress(trapListenerAddress));
    }

    public void sendTrap()
    {
        ScopedPDU pdu = new ScopedPDU();
        pdu.setType(PDU.TRAP);
        pdu.setContextEngineID(agent.getContextEngineID());
        pdu.add(new VariableBinding(SnmpConstants.sysUpTime, new TimeTicks(1)));
        pdu.add(new VariableBinding(SnmpConstants.snmpTrapOID, new OID(".1.3.6.1.2.1.1.8")));
        try {
            session.send(pdu, target);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void registerSnmpMIBs() {
        heartbeatMIB = new Snmp4jHeartbeatMib(super.getNotificationOriginator(),
                new OctetString(),
                super.snmpv2MIB.getSysUpTime());
        super.registerSnmpMIBs();
    }

    @Override
    protected void registerManagedObjects() {
        try {
            server.register(createStaticIfTable(), null);
            heartbeatMIB.registerMOs(server, null);
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
        UsmUser user = new UsmUser(new OctetString("sturm"),
                AuthSHA.ID,
                new OctetString("authpass012"),
                PrivDES.ID,
                new OctetString("privpass012"));

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
                new OctetString("sturm"),
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

        vacmMIB.addViewTreeFamily(new OctetString("fullReadView"), new OID("1.3"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullWriteView"), new OID("1.3"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
        vacmMIB.addViewTreeFamily(new OctetString("fullNotifyView"), new OID("1.3"),
                new OctetString(), VacmMIB.vacmViewIncluded,
                StorageType.nonVolatile);
    }

    @Override
    protected void addCommunities(SnmpCommunityMIB snmpCommunityMIB) {
        Variable[] com2sec = new Variable[] {
                new OctetString("public"),              // community name
                new OctetString("sturm"),              // security name
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

    private static DefaultMOTable createStaticIfTable() {
        MOTableSubIndex[] subIndexes =
                new MOTableSubIndex[] { new MOTableSubIndex(SMIConstants.SYNTAX_INTEGER) };
        MOTableIndex indexDef = new MOTableIndex(subIndexes, false);
        MOColumn[] columns = new MOColumn[8];
        int c = 0;
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifIndex
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_OCTET_STRING,
                        MOAccessImpl.ACCESS_READ_ONLY);// ifDescr
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifType
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifMtu
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_GAUGE32,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifSpeed
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_OCTET_STRING,
                        MOAccessImpl.ACCESS_READ_ONLY);// ifPhysAddress
        columns[c++] =
                new MOMutableColumn(c, SMIConstants.SYNTAX_INTEGER,     // ifAdminStatus
                        MOAccessImpl.ACCESS_READ_WRITE, null);
        columns[c++] =
                new MOColumn(c, SMIConstants.SYNTAX_INTEGER,
                        MOAccessImpl.ACCESS_READ_ONLY);     // ifOperStatus

        DefaultMOTable ifTable =
                new DefaultMOTable(new OID("1.3.6.1.2.1.2.2.1"), indexDef, columns);
        MOMutableTableModel model = (MOMutableTableModel) ifTable.getModel();
        Variable[] rowValues1 = new Variable[] {
                new Integer32(1),
                new OctetString("eth0"),
                new Integer32(6),
                new Integer32(1500),
                new Gauge32(100000000),
                new OctetString("00:00:00:00:01"),
                new Integer32(1),
                new Integer32(1)
        };
        Variable[] rowValues2 = new Variable[] {
                new Integer32(2),
                new OctetString("loopback"),
                new Integer32(24),
                new Integer32(1500),
                new Gauge32(10000000),
                new OctetString("00:00:00:00:02"),
                new Integer32(1),
                new Integer32(1)
        };
        model.addRow(new DefaultMOMutableRow2PC(new OID("1"), rowValues1));
        model.addRow(new DefaultMOMutableRow2PC(new OID("2"), rowValues2));
        ifTable.setVolatile(true);
        return ifTable;
    }
}
